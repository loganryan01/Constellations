// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerController"",
            ""id"": ""85ba3357-0453-4eb4-a55e-e11d6fac16bd"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5eafea6b-cfaf-4e31-b7fa-1f8c556d496c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a5328d8c-1dfd-41bd-8cf1-0f5d48d1a6e8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""6782a700-e5a7-411e-a42b-d4caf7609c8b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cadc1aa6-7bea-446d-9216-f5e696331458"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""a69c1a85-a0ee-44ca-a597-0809ca8f0c8d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8d2a3012-ef22-4ea7-80a9-c80757afada6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d7cf43bb-d3b6-490a-92b7-96b3eec1eb4a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""305a58b0-c560-49b1-9dbf-2388731abfe3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""537ab348-078c-4d0f-ad6e-12047b155906"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""366a5dcb-fff8-4883-9992-e2cd68f9c156"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ScalePuzzle"",
            ""id"": ""14d1cd27-83bd-4f52-a465-2a3cc02f23e3"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""7e9c8387-51f1-4f24-bf76-03db0dfe8eeb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Deselect"",
                    ""type"": ""Button"",
                    ""id"": ""ff563810-32c6-44db-98bd-29c45d85b0d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""7e76f579-0e56-45f9-bdf9-ebbe1888da82"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3bb60902-5d49-4b92-ba6c-99b0f9abf168"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8da398b-d31a-45c3-8852-99deabe0507b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Deselect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ed9fa11-2922-4584-8ec3-e39e987d7d78"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MazePuzzle"",
            ""id"": ""fc0e3ed0-17f9-415e-ad20-9a341e91878d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1f9bfcc4-e28c-4c2b-90d8-95bb1cafc6a6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""f3c589bd-c199-4c71-9d9b-cae27be4c80c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6bca4b40-0e25-41c2-83dd-ca0356a97c50"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d00496cf-98ed-4cef-8e53-f2374aa07c5a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e8b85067-d956-4f0d-8f9c-361dd9bf9c30"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""087cc621-023b-46a6-bcde-d48a914d0e70"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShock4GampadiOS>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerController
        m_PlayerController = asset.FindActionMap("PlayerController", throwIfNotFound: true);
        m_PlayerController_Movement = m_PlayerController.FindAction("Movement", throwIfNotFound: true);
        m_PlayerController_Look = m_PlayerController.FindAction("Look", throwIfNotFound: true);
        m_PlayerController_Interact = m_PlayerController.FindAction("Interact", throwIfNotFound: true);
        // ScalePuzzle
        m_ScalePuzzle = asset.FindActionMap("ScalePuzzle", throwIfNotFound: true);
        m_ScalePuzzle_Select = m_ScalePuzzle.FindAction("Select", throwIfNotFound: true);
        m_ScalePuzzle_Deselect = m_ScalePuzzle.FindAction("Deselect", throwIfNotFound: true);
        m_ScalePuzzle_Reset = m_ScalePuzzle.FindAction("Reset", throwIfNotFound: true);
        // MazePuzzle
        m_MazePuzzle = asset.FindActionMap("MazePuzzle", throwIfNotFound: true);
        m_MazePuzzle_Movement = m_MazePuzzle.FindAction("Movement", throwIfNotFound: true);
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

    // PlayerController
    private readonly InputActionMap m_PlayerController;
    private IPlayerControllerActions m_PlayerControllerActionsCallbackInterface;
    private readonly InputAction m_PlayerController_Movement;
    private readonly InputAction m_PlayerController_Look;
    private readonly InputAction m_PlayerController_Interact;
    public struct PlayerControllerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControllerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerController_Movement;
        public InputAction @Look => m_Wrapper.m_PlayerController_Look;
        public InputAction @Interact => m_Wrapper.m_PlayerController_Interact;
        public InputActionMap Get() { return m_Wrapper.m_PlayerController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControllerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControllerActions instance)
        {
            if (m_Wrapper.m_PlayerControllerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnLook;
                @Interact.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_PlayerControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public PlayerControllerActions @PlayerController => new PlayerControllerActions(this);

    // ScalePuzzle
    private readonly InputActionMap m_ScalePuzzle;
    private IScalePuzzleActions m_ScalePuzzleActionsCallbackInterface;
    private readonly InputAction m_ScalePuzzle_Select;
    private readonly InputAction m_ScalePuzzle_Deselect;
    private readonly InputAction m_ScalePuzzle_Reset;
    public struct ScalePuzzleActions
    {
        private @PlayerInputActions m_Wrapper;
        public ScalePuzzleActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_ScalePuzzle_Select;
        public InputAction @Deselect => m_Wrapper.m_ScalePuzzle_Deselect;
        public InputAction @Reset => m_Wrapper.m_ScalePuzzle_Reset;
        public InputActionMap Get() { return m_Wrapper.m_ScalePuzzle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ScalePuzzleActions set) { return set.Get(); }
        public void SetCallbacks(IScalePuzzleActions instance)
        {
            if (m_Wrapper.m_ScalePuzzleActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnSelect;
                @Deselect.started -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnDeselect;
                @Deselect.performed -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnDeselect;
                @Deselect.canceled -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnDeselect;
                @Reset.started -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnReset;
                @Reset.performed -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnReset;
                @Reset.canceled -= m_Wrapper.m_ScalePuzzleActionsCallbackInterface.OnReset;
            }
            m_Wrapper.m_ScalePuzzleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Deselect.started += instance.OnDeselect;
                @Deselect.performed += instance.OnDeselect;
                @Deselect.canceled += instance.OnDeselect;
                @Reset.started += instance.OnReset;
                @Reset.performed += instance.OnReset;
                @Reset.canceled += instance.OnReset;
            }
        }
    }
    public ScalePuzzleActions @ScalePuzzle => new ScalePuzzleActions(this);

    // MazePuzzle
    private readonly InputActionMap m_MazePuzzle;
    private IMazePuzzleActions m_MazePuzzleActionsCallbackInterface;
    private readonly InputAction m_MazePuzzle_Movement;
    public struct MazePuzzleActions
    {
        private @PlayerInputActions m_Wrapper;
        public MazePuzzleActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_MazePuzzle_Movement;
        public InputActionMap Get() { return m_Wrapper.m_MazePuzzle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MazePuzzleActions set) { return set.Get(); }
        public void SetCallbacks(IMazePuzzleActions instance)
        {
            if (m_Wrapper.m_MazePuzzleActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_MazePuzzleActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_MazePuzzleActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_MazePuzzleActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_MazePuzzleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public MazePuzzleActions @MazePuzzle => new MazePuzzleActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IPlayerControllerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IScalePuzzleActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnDeselect(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
    }
    public interface IMazePuzzleActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
}
