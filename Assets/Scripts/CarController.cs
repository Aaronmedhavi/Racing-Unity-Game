using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public float motorForce = 1500f;
    public float maxSteerAngle = 30f;
    public float brakeForce = 3000f;

    private float currentSteerAngle;
    private float currentAcceleration;
    private float currentBrakeForce;

    private Rigidbody rb;
    private bool isBraking = false;

    private int gear = 0;
    private float currentSpeed = 0f;
    public float[] gearThresholds = { 10f, 25f, 40f, 60f, 80f }; // Speed thresholds for automatic gear changes

    public enum DrivingMode { Manual, Automatic }
    public DrivingMode currentDrivingMode = DrivingMode.Manual;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheelPoses();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleDrivingMode();
        }

        currentAcceleration = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);

        if (currentDrivingMode == DrivingMode.Manual)
        {
            HandleManualInput();
        }
        else
        {
            HandleAutomaticInput();
        }
    }

    private void HandleManualInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Shift up
        {
            ShiftUp();
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Shift down
        {
            ShiftDown();
        }
        else if (Input.GetKeyDown(KeyCode.R)) // Shift to reverse
        {
            ShiftToReverse();
        }
        else if (Input.GetKeyDown(KeyCode.N)) // Shift to neutral
        {
            ShiftToNeutral();
        }
    }

    private void ShiftUp()
    {
        if (gear >= 0 && gear < 5)
        {
            gear++;
            Debug.Log("Shifted up to gear " + gear);
        }
    }

    private void ShiftDown()
    {
        if (gear > 1)
        {
            gear--;
            Debug.Log("Shifted down to gear " + gear);
        }
    }

    private void ShiftToReverse()
    {
        if (currentSpeed < 1f) // Only allow shifting to reverse when nearly stopped
        {
            gear = -1;
            Debug.Log("Shifted to reverse");
        }
    }

    private void ShiftToNeutral()
    {
        gear = 0;
        Debug.Log("Shifted to neutral");
    }

    private void HandleAutomaticInput()
    {
        currentSpeed = rb.velocity.magnitude * 3.6f; // Convert to km/h

        if (currentAcceleration > 0)
        {
            if (gear <= 0) gear = 1;
            for (int i = 0; i < gearThresholds.Length; i++)
            {
                if (currentSpeed > gearThresholds[i])
                {
                    gear = i + 2;
                }
                else
                {
                    break;
                }
            }
        }
        else if (currentAcceleration < 0)
        {
            if (currentSpeed < 1f)
            {
                gear = -1;
            }
            else if (gear > 1)
            {
                gear--;
            }
        }
        else if (Mathf.Abs(currentAcceleration) < 0.1f && currentSpeed < 1f)
        {
            gear = 0;
        }
    }

    private void ToggleDrivingMode()
    {
        currentDrivingMode = (currentDrivingMode == DrivingMode.Manual) ? DrivingMode.Automatic : DrivingMode.Manual;
        Debug.Log("Switched to " + currentDrivingMode + " mode");
    }

    private void HandleMotor()
    {
        currentBrakeForce = isBraking ? brakeForce : 0f;

        float gearMultiplier = (currentDrivingMode == DrivingMode.Automatic) ? (float)gear / 5f : 1f;

        if (gear != 0)
        {
            ApplyMotorTorque(currentAcceleration * Mathf.Abs(motorForce) * Mathf.Sign(gear) * gearMultiplier);
        }
        else
        {
            ApplyMotorTorque(0);
        }

        ApplyBrake();
    }

    private void ApplyMotorTorque(float motorTorque)
    {
        frontLeftWheelCollider.motorTorque = motorTorque;
        frontRightWheelCollider.motorTorque = motorTorque;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void ApplyBrake()
    {
        if (isBraking)
        {
            frontLeftWheelCollider.brakeTorque = currentBrakeForce;
            frontRightWheelCollider.brakeTorque = currentBrakeForce;
            rearLeftWheelCollider.brakeTorque = currentBrakeForce;
            rearRightWheelCollider.brakeTorque = currentBrakeForce;
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = 0f;
            frontRightWheelCollider.brakeTorque = 0f;
            rearLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftWheelCollider, new Vector3(-90, 0, -90));
        UpdateWheelPose(frontRightWheel, frontRightWheelCollider, new Vector3(90, 0, -90));
        UpdateWheelPose(rearLeftWheel, rearLeftWheelCollider, new Vector3(-90, 0, -90));
        UpdateWheelPose(rearRightWheel, rearRightWheelCollider, new Vector3(90, 0, -90));
    }

    private void UpdateWheelPose(Transform wheelTransform, WheelCollider wheelCollider, Vector3 rotationOffset)
    {
        Vector3 pos;
        Quaternion quat;
        wheelCollider.GetWorldPose(out pos, out quat);
        wheelTransform.position = pos;
        wheelTransform.rotation = quat * Quaternion.Euler(rotationOffset);
    }

    public int GetGear()
    {
        return gear;
    }
}
