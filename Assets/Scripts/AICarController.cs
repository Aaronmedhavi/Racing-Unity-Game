using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float maxSpeed = 100f; // Adjust based on your game's scale

    [Header("Sensors")]
    public float sensorLength = 5f;
    public LayerMask obstacleLayerMask;

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
        Sensors();
        ApplySteering();
        Drive();
        UpdateWheelPoses();
        CheckWaypointDistance();
    }

    void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * 1f;
        bool obstacleDetected = false;

        // Front sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength, obstacleLayerMask))
        {
            Debug.DrawLine(sensorStartPos, hit.point, Color.red);
            obstacleDetected = true;
        }

        // If obstacle detected, adjust steering
        if (obstacleDetected)
        {
            frontLeftWheelCollider.steerAngle = maxSteeringAngle;
            frontRightWheelCollider.steerAngle = maxSteeringAngle;
        }
    }

    void ApplySteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float steeringInput = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        frontLeftWheelCollider.steerAngle = steeringInput;
        frontRightWheelCollider.steerAngle = steeringInput;
    }

    void Drive()
    {
        float speed = rb.velocity.magnitude * 3.6f; // Convert to km/h

        // Calculate the angle to the next waypoint
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float steeringInput = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;

        // Adjust speed based on steering angle
        float speedFactor = Mathf.Clamp(1 - (Mathf.Abs(steeringInput) / maxSteeringAngle), 0.5f, 1f);
        float adjustedMaxSpeed = maxSpeed * speedFactor;

        if (speed < adjustedMaxSpeed)
        {
            frontLeftWheelCollider.motorTorque = maxMotorTorque;
            frontRightWheelCollider.motorTorque = maxMotorTorque;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0f;
            frontRightWheelCollider.motorTorque = 0f;
        }
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
    void OnDrawGizmos()
    {
        if (waypoints.Length > 0)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(waypoints[i].position, 1f);

                    if (i > 0)
                    {
                        Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
                    }
                }
            }
        }
    }

}
