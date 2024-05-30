using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerAimDirection : MonoBehaviour
    {
        public Vector2 aimDir { get; private set; } = Vector2.up;
        bool isMouseAim = false;
        bool canMoveMouse = false; // So that first mouse movement is ignored

        Camera cam;

        void Awake()
        {
            cam = Camera.main;

            InputManager.Instance.input.Game.MousePosition.performed += OnMouseMove;
            InputManager.Instance.input.Game.AimDir.performed += OnRightStickMove;
        }

        void OnMouseMove(InputAction.CallbackContext context)
        {
            // This input action is performed once on start for some reason
            if (canMoveMouse)
                isMouseAim = true;
            else
                canMoveMouse = true;
        }

        void OnRightStickMove(InputAction.CallbackContext context)
        {
            isMouseAim = false;
        }

        private void Update()
        {
            UpdateAimDir();
        }

        void UpdateAimDir()
        {
            if (isMouseAim)
                aimDir = (InputManager.Instance.input.Game.MousePosition.ReadValue<Vector2>() - (Vector2)cam.WorldToScreenPoint(transform.position)).normalized;
            else
            {
                Vector2 newAimDir = InputManager.Instance.input.Game.AimDir.ReadValue<Vector2>();
                if (newAimDir != Vector2.zero)
                    aimDir = InputManager.Instance.input.Game.AimDir.ReadValue<Vector2>();
            }
        }
    }
}
