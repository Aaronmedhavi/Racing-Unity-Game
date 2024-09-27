using UnityEngine;

public class CarController : MonoBehaviour
{
    // Input variables
    private float horizontalInput, verticalInput;
    private bool isBraking;

    // Car settings
    [Header("Car Settings")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;

    // Downforce settings
    [Header("Downforce Settings")]
    [SerializeField] private float downforceFactor = 50f;

    // Anti-roll bar settings
    [Header("Anti-Roll Bar Settings")]
    [SerializeField] private float antiRollStiffness = 5000f;

    // Wheel Colliders
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    // Wheel Transforms
    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    // Private variables
    private Rigidbody rb;

    // Initial wheel rotations
    private Quaternion frontLeftWheelInitialRotation, frontRightWheelInitialRotation;
    private Quaternion rearLeftWheelInitialRotation, rearRightWheelInitialRotation;

    // Gearbox variables
    [Header("Gearbox Settings")]
    [SerializeField] private float[] gearRatios = { -3.0f, 3.6f, 2.1f, 1.4f, 1.0f, 0.8f };
    private int currentGear = 1; // Start in first forward gear
    private float engineRPM;

    // Public properties for UI access
    public int CurrentGear
    {
        get { return currentGear; }
    }

    public float GetCurrentSpeed()
    {
        // Speed in km/h
        return rb.velocity.magnitude * 3.6f;
    }

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Store the initial local rotations of the wheels
        frontLeftWheelInitialRotation = frontLeftWheelTransform.localRotation;
        frontRightWheelInitialRotation = frontRightWheelTransform.localRotation;
        rearLeftWheelInitialRotation = rearLeftWheelTransform.localRotation;
        rearRightWheelInitialRotation = rearRightWheelTransform.localRotation;

        // Lower the center of mass for stability
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);

        // Setup suspension and friction
        SetupSuspension();
        AdjustFriction();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        ShiftGears();
        ApplyAntiRollBars();
        ApplyDownforce();
        ApplyBraking();
        UpdateWheels();
    }

    /// <summary>
    /// Reads the player's input.
    /// </summary>
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);

        // Debug logs
        Debug.Log($"Input - Horizontal: {horizontalInput}, Vertical: {verticalInput}, Braking: {isBraking}");
    }

    /// <summary>
    /// Handles motor torque.
    /// </summary>
    private void HandleMotor()
    {
        float adjustedMotorForce = motorForce;
        if (IsWheelSlip())
        {
            adjustedMotorForce *= 0.5f; // Traction control
            Debug.Log("Traction Control Activated.");
        }

        // Apply motor torque adjusted by gear ratio
        float torque = verticalInput * adjustedMotorForce * gearRatios[currentGear];
        rearLeftWheelCollider.motorTorque = torque;
        rearRightWheelCollider.motorTorque = torque;

        // Debug logs
        Debug.Log($"Motor Torque Applied: {torque}");
    }

    /// <summary>
    /// Handles the steering of the front wheels.
    /// </summary>
    private void HandleSteering()
    {
        float steerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;

        // Debug logs
        Debug.Log($"Steer Angle: {steerAngle}");
    }

    /// <summary>
    /// Applies braking force to all wheels.
    /// </summary>
    private void ApplyBraking()
    {
        float brakeTorque = isBraking ? brakeForce : 0f;
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;

        // Debug logs
        Debug.Log($"Brake Torque Applied: {brakeTorque}");
    }

    /// <summary>
    /// Updates the position and rotation of each wheel.
    /// </summary>
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform, frontLeftWheelInitialRotation);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform, frontRightWheelInitialRotation);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform, rearLeftWheelInitialRotation);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform, rearRightWheelInitialRotation);
    }

    /// <summary>
    /// Updates a single wheel's position and rotation.
    /// </summary>
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform, Quaternion initialRotation)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot * initialRotation;
    }

    /// <summary>
    /// Sets up the suspension for all wheels.
    /// </summary>
    private void SetupSuspension()
    {
        JointSpring suspensionSpring = new JointSpring
        {
            spring = 60000f,    // Higher stiffness for sportier feel
            damper = 8000f,     // Increased damping
            targetPosition = 0.4f // Slightly lower resting position
        };

        // Apply suspension settings to all wheels
        frontLeftWheelCollider.suspensionSpring = suspensionSpring;
        frontRightWheelCollider.suspensionSpring = suspensionSpring;
        rearLeftWheelCollider.suspensionSpring = suspensionSpring;
        rearRightWheelCollider.suspensionSpring = suspensionSpring;

        // Set the suspension distance
        frontLeftWheelCollider.suspensionDistance = 0.15f;
        frontRightWheelCollider.suspensionDistance = 0.15f;
        rearLeftWheelCollider.suspensionDistance = 0.15f;
        rearRightWheelCollider.suspensionDistance = 0.15f;
    }

    /// <summary>
    /// Adjusts the friction settings for all wheels.
    /// </summary>
    private void AdjustFriction()
    {
        WheelFrictionCurve forwardFriction = new WheelFrictionCurve
        {
            extremumSlip = 0.1f,
            extremumValue = 1f,
            asymptoteSlip = 0.5f,
            asymptoteValue = 0.8f,
            stiffness = 2.0f
        };

        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve
        {
            extremumSlip = 0.2f,
            extremumValue = 1f,
            asymptoteSlip = 0.5f,
            asymptoteValue = 0.7f,
            stiffness = 2.0f
        };

        // Apply friction settings to all wheels
        frontLeftWheelCollider.forwardFriction = forwardFriction;
        frontRightWheelCollider.forwardFriction = forwardFriction;
        rearLeftWheelCollider.forwardFriction = forwardFriction;
        rearRightWheelCollider.forwardFriction = forwardFriction;

        frontLeftWheelCollider.sidewaysFriction = sidewaysFriction;
        frontRightWheelCollider.sidewaysFriction = sidewaysFriction;
        rearLeftWheelCollider.sidewaysFriction = sidewaysFriction;
        rearRightWheelCollider.sidewaysFriction = sidewaysFriction;
    }

    /// <summary>
    /// Shifts gears based on player input.
    /// </summary>
    private void ShiftGears()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentGear < gearRatios.Length - 1)
                currentGear++;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentGear > 0)
                currentGear--;
        }

        Debug.Log($"Current Gear: {currentGear}, Gear Ratio: {gearRatios[currentGear]}");
    }

    /// <summary>
    /// Applies anti-roll bars to reduce body roll.
    /// </summary>
    private void ApplyAntiRollBars()
    {
        ApplyAntiRoll(frontLeftWheelCollider, frontRightWheelCollider);
        ApplyAntiRoll(rearLeftWheelCollider, rearRightWheelCollider);
    }

    /// <summary>
    /// Applies anti-roll force between two wheels.
    /// </summary>
    private void ApplyAntiRoll(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * antiRollStiffness;

        if (groundedL)
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);

        // Debug logs
        Debug.Log($"Anti-Roll Force Applied: {antiRollForce}");
    }

    /// <summary>
    /// Applies downforce based on the car's speed.
    /// </summary>
    private void ApplyDownforce()
    {
        float speed = rb.velocity.magnitude;
        float downforce = speed * downforceFactor;
        rb.AddForce(-transform.up * downforce);

        // Debug logs
        Debug.Log($"Downforce Applied: {downforce}");
    }

    /// <summary>
    /// Checks if any driven wheel is slipping.
    /// </summary>
    private bool IsWheelSlip()
    {
        WheelHit rearLeftHit, rearRightHit;
        bool slipL = rearLeftWheelCollider.GetGroundHit(out rearLeftHit) ? Mathf.Abs(rearLeftHit.forwardSlip) > 0.2f : false;
        bool slipR = rearRightWheelCollider.GetGroundHit(out rearRightHit) ? Mathf.Abs(rearRightHit.forwardSlip) > 0.2f : false;
        return slipL || slipR;
    }
}
