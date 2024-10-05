using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(StateMachineUI))]
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject pauseState;

        StateMachineUI sm;

        bool isPaused;

        private void Awake()
        {
            sm = GetComponent<StateMachineUI>();
            InputManager.Instance.input.Scene.Pause.performed += OnPause;
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance())
                InputManager.Instance.input.Scene.Pause.performed -= OnPause;
        }

        private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (isPaused) return;
            isPaused = true;

            InputManager.Instance.input.Game.Disable();
            InputManager.Instance.input.UI.Enable();

            Time.timeScale = 0;
            sm.SetState(pauseState);
        }

        public void Resume()
        {
            isPaused = false;

            InputManager.Instance.input.Game.Enable();
            InputManager.Instance.input.UI.Disable();

            Time.timeScale = 1;
            sm.Clear();
        }
    }
}
