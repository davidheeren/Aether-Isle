using UnityEngine;
using System.Collections.Generic;

namespace EventSystem
{
    [CreateAssetMenu(menuName = "EventSystem/Event")]
    public class GameEvent : ScriptableObject
    {
        List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise(GameEventData data = null)
        {
            if (data == null)
                data = new GameEventData(null);

            foreach (GameEventListener listener in listeners)
            {
                listener.response.Invoke(data);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        [ContextMenu("Log Listeners")]
        void LogListeners()
        {
            Debug.Log(listeners.Count);
        }
    }
}
