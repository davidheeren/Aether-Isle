using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Test : MonoBehaviour
    {
        private InputAction action;

        private void Awake()
        {
            action = InputManager.Instance.input.Game.Attack;
        }
    }
}
