using UnityEngine;
using Input;
using Utilities;

namespace Game
{
    public class InputManager : Singleton<InputManager>
    {
        public GameInput input;

        void Awake()
        {
            input = new GameInput();
            input.Enable();
        }
    }
}
