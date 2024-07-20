using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneSwitcher : Singleton<SceneSwitcher>
    {
        private void Awake()
        {
            InputManager.Instance.input.Scene.Restart.performed += Restart;
        }

        private void Restart(InputAction.CallbackContext context)
        {
            InputManager.Instance.input.Disable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
