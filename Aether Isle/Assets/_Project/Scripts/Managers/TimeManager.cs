using Save;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utilities;

namespace Game
{
    public class TimeManager : Singleton<TimeManager>
    {
        //private void Awake()
        //{
        //    InputManager.Instance.input.Scene.Pause.performed += OnRestart;
        //}

        private void OnDestroy()
        {
            //if (InputManager.HasInstance())
            //    InputManager.Instance.input.Scene.Pause.performed -= OnRestart;
        }

        //private void OnRestart(UnityEngine.InputSystem.InputAction.CallbackContext context)
        //{
        //    //SceneSwitcher.Instance.Restart();
        //    InputManager.Instance.input.Disable();
        //    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}

        private void Update()
        {
            if (InputManager.Instance.input.Scene.Pause.WasPressedThisFrame())
            {
                SceneSwitcher.Instance.Restart();
            }
        }

        private void OnApplicationQuit()
        {
            SaveSystem.Data.timeAtLastUnload += Time.time;
            //SaveSystem.SaveObject.PlayerPos = new Vector2(10, -8);
            SaveSystem.Save();
            //print("Exit Application");
        }
    }
}
