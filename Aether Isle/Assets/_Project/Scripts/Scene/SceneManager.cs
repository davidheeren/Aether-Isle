using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Game
{
    public class SceneManager : Singleton<SceneManager>
    {
        private void Awake()
        {
            InputManager.Instance.input.Scene.Restart.performed += Restart;
        }

        private void Restart(InputAction.CallbackContext context)
        {
            InputManager.Instance.input.Disable();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
