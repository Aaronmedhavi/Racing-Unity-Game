using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float[] gearThresholds = { 10f, 25f, 40f, 60f, 80f }; // Speed for automatic gear changes

    public enum DrivingMode { Manual, Automatic }
    public DrivingMode currentDrivingMode = DrivingMode.Manual;

    private Gamepad gamepad;  // Gamepad reference

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        gamepad = Gamepad.current;

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
        if (Input.GetKeyDown(KeyCode.M) || (gamepad != null && gamepad.dpad.up.wasPressedThisFrame))
        {
            ToggleDrivingMode();
        }
        currentAcceleration = Input.GetAxis("Vertical");
        if (gamepad != null)
        {
            currentAcceleration = gamepad.rightTrigger.ReadValue();
        }

        // Brake with both keyboard and gamepad
        isBraking = Input.GetKey(KeyCode.Space);
        if (gamepad != null)
        {
            isBraking = gamepad.leftTrigger.ReadValue() > 0.1f;
        }

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
        if (Input.GetKeyDown(KeyCode.E) || (gamepad != null && gamepad.rightShoulder.wasPressedThisFrame))
        {
            ShiftUp();
        }
        else if (Input.GetKeyDown(KeyCode.Q) || (gamepad != null && gamepad.leftShoulder.wasPressedThisFrame))
        {
            ShiftDown();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ShiftToReverse();
        }
        else if (Input.GetKeyDown(KeyCode.N))
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
        if (currentSpeed < 1f) 
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

        float dynamicMotorForce = motorForce;
        if (currentDrivingMode == DrivingMode.Automatic && rb.velocity.magnitude * 3.6f < 10f) 
        {
            dynamicMotorForce = motorForce * 2f; 
        }

        if (gear != 0)
        {
            ApplyMotorTorque(currentAcceleration * Mathf.Abs(dynamicMotorForce) * Mathf.Sign(gear) * gearMultiplier);
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
        if (gamepad != null)
        {
            currentSteerAngle = maxSteerAngle * gamepad.leftStick.x.ReadValue(); 
        }

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
