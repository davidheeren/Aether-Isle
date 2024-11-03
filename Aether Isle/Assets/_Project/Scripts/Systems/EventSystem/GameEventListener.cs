using UnityEngine;
using UnityEngine.Events;

namespace CustomEventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] GameEvent gameEvent;

        public UnityEvent<GameEventData> response;

        private void OnEnable()
        {
            if (gameEvent != null)
                gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent != null)
                gameEvent.UnregisterListener(this);
        }

        public void LogEvent()
        {
            print("Event of type: " + gameEvent.name + " was raised");
        }
    }
}
