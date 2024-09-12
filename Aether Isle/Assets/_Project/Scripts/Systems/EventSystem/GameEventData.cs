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

        public static implicit operator GameEventData(int data) { return new GameEventData(data); }
        //public static implicit operator int(GameEventData data) { return data.GetData<int>(); }

        public static implicit operator GameEventData(float data) { return new GameEventData(data); }
        //public static implicit operator float(GameEventData data) { return data.GetData<float>(); }

        public static implicit operator GameEventData(bool data) { return new GameEventData(data); }
        //public static implicit operator bool(GameEventData data) { return data.GetData<bool>(); }

        public static implicit operator GameEventData(Vector3 data) { return new GameEventData(data); }
        //public static implicit operator Vector3(GameEventData data) { return data.GetData<Vector3>(); }

        public static implicit operator GameEventData(Vector2 data) { return new GameEventData(data); }
        //public static implicit operator Vector2(GameEventData data) { return data.GetData<Vector2>(); }

        public static implicit operator GameEventData(Quaternion data) { return new GameEventData(data); }
        //public static implicit operator Quaternion(GameEventData data) { return data.GetData<Quaternion>(); }

        public static implicit operator GameEventData(GameObject data) { return new GameEventData(data); }
        //public static implicit operator GameObject(GameEventData data) { return data.GetData<GameObject>(); }

        public static implicit operator GameEventData(Transform data) { return new GameEventData(data); }
        //public static implicit operator Transform(GameEventData data) { return data.GetData<Transform>(); }

        public static implicit operator GameEventData(Rigidbody data) { return new GameEventData(data); }
        //public static implicit operator Rigidbody(GameEventData data) { return data.GetData<Rigidbody>(); }

        public static implicit operator GameEventData(Rigidbody2D data) { return new GameEventData(data); }
        //public static implicit operator Rigidbody2D(GameEventData data) { return data.GetData<Rigidbody2D>(); }

        public static implicit operator GameEventData(Collider data) { return new GameEventData(data); }
        //public static implicit operator Collider(GameEventData data) { return data.GetData<Collider>(); }

        public static implicit operator GameEventData(Collider2D data) { return new GameEventData(data); }
        //public static implicit operator Collider2D(GameEventData data) { return data.GetData<Collider2D>(); }
    }
}
