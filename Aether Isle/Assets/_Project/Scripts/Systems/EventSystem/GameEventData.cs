using UnityEngine;

namespace EventSystem
{
    public class GameEventData
    {
        object data;

        public GameEventData(object data)
        {
            SetData(data);
        }

        public void SetData(object data)
        {
            this.data = data;
        }

        public T GetData<T>()
        {
            if (data == null)
                return default;

            if (data is T)
                return (T)data;

            Debug.LogError("Type: " + typeof(T) + " is not the same as data: " + data);
            return default;
        }
    }
}
