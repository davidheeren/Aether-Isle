using Input;
using UnityEngine.InputSystem;
using Utilities;
using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    public class InputManager : Singleton<InputManager>
    {
        public GameInput input { get; private set; }

        public ControlScheme currentControlScheme { get; private set; } = ControlScheme.None;

        public event Action<ControlScheme> OnControlSchemeChange;

        void Awake()
        {
            input = new GameInput();
            input.Enable();
        }

        IEnumerator Start()
        {
            // The mouse input get used the first frame so we want to ignore that for setting the control scheme
            yield return null;

            input.Game.Get().actionTriggered += OnActionTriggered;
            input.UI.Get().actionTriggered += OnActionTriggered;
            input.Scene.Get().actionTriggered += OnActionTriggered;
        }

        private void OnDestroy()
        {
            input.Game.Get().actionTriggered -= OnActionTriggered;
            input.UI.Get().actionTriggered -= OnActionTriggered;
            input.Scene.Get().actionTriggered -= OnActionTriggered;
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
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

        public void EnableGameInput(bool enable)
        {
            if (enable)
            {
                input.Game.Enable();

            }
            else
                input.Game.Disable();
        }

        public void EnableSceneInput(bool enable)
        {
            if (enable)
                input.Scene.Enable();
            else
                input.Scene.Disable();
        }

        public void EnableUIInput(bool enable)
        {
            if (enable)
                input.UI.Enable();
            else
                input.UI.Disable();
        }
        public void EnableInput(bool enable)
        {
            if (enable)
            {
                input.Game.Enable();

            }
            else
                input.Game.Disable();
        }

        public enum ControlScheme
        {
            None,
            Keyboard,
            Gamepad
        }
    }
}
