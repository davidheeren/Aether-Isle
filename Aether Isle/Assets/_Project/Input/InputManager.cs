using Input;
using UnityEngine.InputSystem;
using Utilities;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class InputManager : Singleton<InputManager>
    {
        public GameInput input { get; private set; }

        public ControlScheme currentControlScheme { get; private set; } = ControlScheme.None;

        bool ignoreFirstUpdateInputForScheme = true;

        void Awake()
        {
            input = new GameInput();
            input.Enable();

            input.Game.Get().actionTriggered += OnActionTriggered;
        }

        IEnumerator Start()
        {
            // The mouse input get used the first frame so we want to ignore that for setting the control scheme
            yield return null;
            ignoreFirstUpdateInputForScheme = false;
        }

        private void OnDestroy()
        {
            input.Game.Get().actionTriggered += OnActionTriggered;
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            // NOTE: In input editor, for each control scheme edit it so that it has the correct devices with "required" turned off

            InputDevice device = context.control.device;
            InputControlScheme? scheme = InputControlScheme.FindControlSchemeForDevice(device, input.controlSchemes);

            if (!scheme.HasValue)
            {
                currentControlScheme = ControlScheme.None;
                return;
            }

            if (ignoreFirstUpdateInputForScheme) return;

            if (scheme.Value.name == "Gamepad")
                currentControlScheme = ControlScheme.Gamepad;
            else if (scheme.Value.name == "Keyboard")
                currentControlScheme = ControlScheme.Keyboard;
            else
                Debug.LogWarning("Control Scheme not found");

            //context.action.GetBindingDisplayString();
        }

        public enum ControlScheme
        {
            None,
            Keyboard,
            Gamepad
        }
    }
}
