using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("Waypoints")]
    private Transform[] waypoints;
    public int currentWaypointIndex = 0;

    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;
    public float maxBrakeTorque = 3000f;
    public float maxSteeringAngle = 30f;
    public float brakeSensitivity = 50f;
    public int lookAheadIndex = 1;
    public float maxSpeed = 100f; 
    public float waypointThreshold = 5f;

    private Rigidbody rb;

    // Wheel Colliders
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    // Wheels Transforms (for visual rotation)
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        waypoints = WaypointManager.Instance.waypoints;
    }

    void FixedUpdate()
    {
        ApplySteering();
        Drive();
        UpdateWheelPoses();
        CheckWaypointDistance();
        UpdateCurrentWaypoint();
    }

    void ApplySteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float steeringInput = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        steeringInput = Mathf.Clamp(steeringInput, -maxSteeringAngle, maxSteeringAngle);
        frontLeftWheelCollider.steerAngle = steeringInput;
        frontRightWheelCollider.steerAngle = steeringInput;
    }

    void Drive()
    {
        float speed = rb.velocity.magnitude * 3.6f;

        int lookAheadWaypointIndex = (currentWaypointIndex + lookAheadIndex) % waypoints.Length;
        Vector3 lookAheadVector = transform.InverseTransformPoint(waypoints[lookAheadWaypointIndex].position);
        float lookAheadSteeringAngle = Mathf.Atan2(lookAheadVector.x, lookAheadVector.z) * Mathf.Rad2Deg;
        float desiredSpeed = Mathf.Clamp(maxSpeed - (Mathf.Abs(lookAheadSteeringAngle) * 2f), 30f, maxSpeed); float speedDifference = speed - desiredSpeed;
        float brake = Mathf.Clamp(speedDifference * brakeSensitivity, 0f, maxBrakeTorque);

        float motor = 0f;
        float tractionControl = Mathf.Clamp01((maxSpeed - speed) / maxSpeed);
        motor = maxMotorTorque * tractionControl;

        if (speed < desiredSpeed)
        {
            motor = maxMotorTorque * tractionControl;
            brake = 0f;
        }
        else
        {
            motor = 0f;
            brake = Mathf.Clamp((speed - desiredSpeed) * brakeSensitivity, 0f, maxBrakeTorque);
        }

        // Apply motor torque
        frontLeftWheelCollider.motorTorque = motor;
        frontRightWheelCollider.motorTorque = motor;

        // Apply brake torque
        frontLeftWheelCollider.brakeTorque = brake;
        frontRightWheelCollider.brakeTorque = brake;
        rearLeftWheelCollider.brakeTorque = brake;
        rearRightWheelCollider.brakeTorque = brake;
    }

    void CheckWaypointDistance()
    {
        float distance = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);
        if (distance < 5f) // Adjust threshold as needed
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheelTransform, frontLeftWheelCollider, new Vector3(-90, 0, -90));
        UpdateWheelPose(frontRightWheelTransform, frontRightWheelCollider, new Vector3(90, 0, -90));
        UpdateWheelPose(rearLeftWheelTransform, rearLeftWheelCollider, new Vector3(-90, 0, -90));
        UpdateWheelPose(rearRightWheelTransform, rearRightWheelCollider, new Vector3(90, 0, -90));
    }

    private void UpdateWheelPose(Transform wheelTransform, WheelCollider wheelCollider, Vector3 rotationOffset)
    {
        Vector3 pos;
        Quaternion quat;
        wheelCollider.GetWorldPose(out pos, out quat);
        wheelTransform.position = pos;
        wheelTransform.rotation = quat * Quaternion.Euler(rotationOffset);
    }

    void UpdateCurrentWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
