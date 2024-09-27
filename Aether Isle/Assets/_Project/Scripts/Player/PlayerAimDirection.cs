using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerAimDirection : MonoBehaviour
    {
        public Vector2 aimDir { get; private set; } = Vector2.up;
        public bool isLocked { get; private set; } = false;

        Camera cam;

        void Awake()
        {
            cam = Camera.main;

            InputManager.Instance.input.Game.LockAim.performed += (InputAction.CallbackContext ctx) => isLocked = true;
        }

        private void Update()
        {
            UpdateAimDir();
        }

        void UpdateAimDir()
        {
            if (InputManager.Instance.currentControlScheme == InputManager.ControlScheme.Keyboard)
            {
                isLocked = false;
                aimDir = (InputManager.Instance.input.Game.MousePosition.ReadValue<Vector2>() - (Vector2)cam.WorldToScreenPoint(transform.position)).normalized;
            }
            else if (InputManager.Instance.currentControlScheme == InputManager.ControlScheme.Gamepad)
            {
                Vector2 newAimDir = InputManager.Instance.input.Game.AimDir.ReadValue<Vector2>();
                if (newAimDir != Vector2.zero)
                    isLocked = false;

                if (isLocked)
                {
                    Vector2 moveDir = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
                    if (moveDir != Vector2.zero)
                        aimDir = moveDir;
                }
                else
                    if (newAimDir != Vector2.zero)
                        aimDir = newAimDir;
            }
        }
    }
}
