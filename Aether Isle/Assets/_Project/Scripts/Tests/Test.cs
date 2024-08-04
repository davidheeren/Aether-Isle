using EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [SerializeField] GameEvent gameEvent;

        int index;

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                index++;
                gameEvent.Raise();
            }
        }

        public void ReceiveEvent(GameEventData data)
        {
            Debug.Log(data.GetData<float>());
        }
    }
}
