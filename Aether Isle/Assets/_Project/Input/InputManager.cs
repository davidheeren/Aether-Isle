using Input;
using UnityEngine.InputSystem;
using Utilities;
using UnityEngine;
using System;
using CustomInspector;

namespace Game
{
    public class InputManager : Singleton<InputManager>
    {
        public GameInput input { get; private set; }

        [field: SerializeField, ReadOnly] public ControlScheme currentControlScheme { get; private set; } = ControlScheme.None;

        public event Action<ControlScheme> OnControlSchemeChange;

        void Awake()
        {
            input = new GameInput();
            input.Enable();

            HandleSchemeSubscriptions(true);
        }

        private void OnDestroy()
        {
            HandleSchemeSubscriptions(false);
            input.Disable();
        }

        void HandleSchemeSubscriptions(bool subscribe)
        {
            foreach (InputAction a in input.Game.Get())
            {
                if (a != input.Game.MousePosition)
                {
                    if (subscribe)
                        a.performed += SetControlScheme;
                    else
                        a.performed -= SetControlScheme;
                }
            }

            foreach (InputAction a in input.UI.Get())
            {
                if (a != input.UI.Point)
                {
                    if (subscribe)
                        a.performed += SetControlScheme;
                    else
                        a.performed -= SetControlScheme;
                }
            }

            if (subscribe)
                input.Scene.Get().actionTriggered += SetControlScheme;
            else
                input.Scene.Get().actionTriggered -= SetControlScheme;
        }

        private void SetControlScheme(InputAction.CallbackContext context)
        {
            // NOTE: In input editor, for each control scheme edit it so that it has the correct devices with "required" turned off

            InputDevice device = context.control.device;
            InputControlScheme? scheme = InputControlScheme.FindControlSchemeForDevice(device, input.controlSchemes);

            ControlScheme newControlScheme = ControlScheme.None;

            if (scheme.HasValue)
            {
                if (scheme.Value.name == "Gamepad")
                    newControlScheme = ControlScheme.Gamepad;
                else if (scheme.Value.name == "Keyboard")
                    newControlScheme = ControlScheme.Keyboard;
                else
                    Debug.LogWarning("Control Scheme not found");
            }

            // If new Control scheme is different we publish event
            if (newControlScheme != currentControlScheme)
            {
                currentControlScheme = newControlScheme;
                OnControlSchemeChange?.Invoke(currentControlScheme);
            }
            else
                currentControlScheme = newControlScheme;

            //context.action.GetBindingDisplayString
        }

        public enum ControlScheme
        {
            None,
            Keyboard,
            Gamepad
        }
    }
}
