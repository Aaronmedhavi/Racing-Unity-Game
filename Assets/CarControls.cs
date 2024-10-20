//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/CarControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CarControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CarControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CarControls"",
    ""maps"": [
        {
            ""name"": ""DrivingControls"",
            ""id"": ""9d347e8f-0808-45a8-82b0-060ce4f7da4c"",
            ""actions"": [
                {
                    ""name"": ""Accelerate"",
                    ""type"": ""Value"",
                    ""id"": ""e603c2b0-fd23-417c-a816-0ee07b852501"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.2)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Value"",
                    ""id"": ""1d73e866-b87e-46a4-8f25-41aa1522848b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ChangeMode"",
                    ""type"": ""Button"",
                    ""id"": ""85655bfd-e817-4420-bbcb-620963f62f8e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShiftDown"",
                    ""type"": ""Button"",
                    ""id"": ""e34c2cf8-f7f9-407a-b7f3-063adb943b3c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShiftUp"",
                    ""type"": ""Button"",
                    ""id"": ""35389a54-5cec-4281-ae2a-fa4b34d9a562"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Steering"",
                    ""type"": ""Value"",
                    ""id"": ""224439e1-218a-4604-81b6-e5906e8dc4e4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""926642b3-cd43-4a18-a28a-25cd563294b8"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c724336-bb6f-4071-ac7e-bcc320423a04"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85d4a2b0-6c0d-46a1-a61f-940b71cf43c7"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b874d12e-914c-4746-8652-36e3cd73954d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShiftDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a2f2e8b-ff0b-42d4-8a90-5025f40ddb05"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShiftUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17ad9b12-7bae-42fd-a0f6-45affdb9772d"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // DrivingControls
        m_DrivingControls = asset.FindActionMap("DrivingControls", throwIfNotFound: true);
        m_DrivingControls_Accelerate = m_DrivingControls.FindAction("Accelerate", throwIfNotFound: true);
        m_DrivingControls_Brake = m_DrivingControls.FindAction("Brake", throwIfNotFound: true);
        m_DrivingControls_ChangeMode = m_DrivingControls.FindAction("ChangeMode", throwIfNotFound: true);
        m_DrivingControls_ShiftDown = m_DrivingControls.FindAction("ShiftDown", throwIfNotFound: true);
        m_DrivingControls_ShiftUp = m_DrivingControls.FindAction("ShiftUp", throwIfNotFound: true);
        m_DrivingControls_Steering = m_DrivingControls.FindAction("Steering", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // DrivingControls
    private readonly InputActionMap m_DrivingControls;
    private List<IDrivingControlsActions> m_DrivingControlsActionsCallbackInterfaces = new List<IDrivingControlsActions>();
    private readonly InputAction m_DrivingControls_Accelerate;
    private readonly InputAction m_DrivingControls_Brake;
    private readonly InputAction m_DrivingControls_ChangeMode;
    private readonly InputAction m_DrivingControls_ShiftDown;
    private readonly InputAction m_DrivingControls_ShiftUp;
    private readonly InputAction m_DrivingControls_Steering;
    public struct DrivingControlsActions
    {
        private @CarControls m_Wrapper;
        public DrivingControlsActions(@CarControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Accelerate => m_Wrapper.m_DrivingControls_Accelerate;
        public InputAction @Brake => m_Wrapper.m_DrivingControls_Brake;
        public InputAction @ChangeMode => m_Wrapper.m_DrivingControls_ChangeMode;
        public InputAction @ShiftDown => m_Wrapper.m_DrivingControls_ShiftDown;
        public InputAction @ShiftUp => m_Wrapper.m_DrivingControls_ShiftUp;
        public InputAction @Steering => m_Wrapper.m_DrivingControls_Steering;
        public InputActionMap Get() { return m_Wrapper.m_DrivingControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DrivingControlsActions set) { return set.Get(); }
        public void AddCallbacks(IDrivingControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_DrivingControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DrivingControlsActionsCallbackInterfaces.Add(instance);
            @Accelerate.started += instance.OnAccelerate;
            @Accelerate.performed += instance.OnAccelerate;
            @Accelerate.canceled += instance.OnAccelerate;
            @Brake.started += instance.OnBrake;
            @Brake.performed += instance.OnBrake;
            @Brake.canceled += instance.OnBrake;
            @ChangeMode.started += instance.OnChangeMode;
            @ChangeMode.performed += instance.OnChangeMode;
            @ChangeMode.canceled += instance.OnChangeMode;
            @ShiftDown.started += instance.OnShiftDown;
            @ShiftDown.performed += instance.OnShiftDown;
            @ShiftDown.canceled += instance.OnShiftDown;
            @ShiftUp.started += instance.OnShiftUp;
            @ShiftUp.performed += instance.OnShiftUp;
            @ShiftUp.canceled += instance.OnShiftUp;
            @Steering.started += instance.OnSteering;
            @Steering.performed += instance.OnSteering;
            @Steering.canceled += instance.OnSteering;
        }

        private void UnregisterCallbacks(IDrivingControlsActions instance)
        {
            @Accelerate.started -= instance.OnAccelerate;
            @Accelerate.performed -= instance.OnAccelerate;
            @Accelerate.canceled -= instance.OnAccelerate;
            @Brake.started -= instance.OnBrake;
            @Brake.performed -= instance.OnBrake;
            @Brake.canceled -= instance.OnBrake;
            @ChangeMode.started -= instance.OnChangeMode;
            @ChangeMode.performed -= instance.OnChangeMode;
            @ChangeMode.canceled -= instance.OnChangeMode;
            @ShiftDown.started -= instance.OnShiftDown;
            @ShiftDown.performed -= instance.OnShiftDown;
            @ShiftDown.canceled -= instance.OnShiftDown;
            @ShiftUp.started -= instance.OnShiftUp;
            @ShiftUp.performed -= instance.OnShiftUp;
            @ShiftUp.canceled -= instance.OnShiftUp;
            @Steering.started -= instance.OnSteering;
            @Steering.performed -= instance.OnSteering;
            @Steering.canceled -= instance.OnSteering;
        }

        public void RemoveCallbacks(IDrivingControlsActions instance)
        {
            if (m_Wrapper.m_DrivingControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDrivingControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_DrivingControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DrivingControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DrivingControlsActions @DrivingControls => new DrivingControlsActions(this);
    public interface IDrivingControlsActions
    {
        void OnAccelerate(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnChangeMode(InputAction.CallbackContext context);
        void OnShiftDown(InputAction.CallbackContext context);
        void OnShiftUp(InputAction.CallbackContext context);
        void OnSteering(InputAction.CallbackContext context);
    }
}
