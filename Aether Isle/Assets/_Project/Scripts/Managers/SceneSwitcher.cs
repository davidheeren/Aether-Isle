using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using UnityEngine.SceneManagement;
using System;

namespace Game
{
    public class SceneSwitcher : Singleton<SceneSwitcher>
    {
        private void Restart(InputAction.CallbackContext context) => Restart();

        public void Restart()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            print("Quit Application"); // Application.Quit() does nothing in the editor
            Application.Quit();
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
            InputManager.Instance.input.Disable();
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
            InputManager.Instance.input.Disable();
        }
    }
}
