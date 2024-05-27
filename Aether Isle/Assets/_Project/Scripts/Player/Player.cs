using CustomInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Movement))]
    public class Player : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5;

        [field: SerializeField, ReadOnly] public Vector2 aimDir { get; private set; } = Vector2.up;
        bool isMouseAim = false;
        bool canMoveMouse = false; // So that first mouse movement is ignored

        Movement movement;

        void Awake()
        {
            movement = GetComponent<Movement>();

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
                aimDir = (InputManager.Instance.input.Game.MousePosition.ReadValue<Vector2>() - (Vector2)Camera.main.WorldToScreenPoint(transform.position)).normalized;
            else
            {
                Vector2 newAimDir = InputManager.Instance.input.Game.AimDir.ReadValue<Vector2>();
                if (newAimDir != Vector2.zero)
                    aimDir = InputManager.Instance.input.Game.AimDir.ReadValue<Vector2>();
            }
        }


        void FixedUpdate()
        {
            movement.MoveVelocity(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * maxSpeed);
        }
    }
}
