using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name + " Singleton");
                    _instance = obj.AddComponent<T>();
                }

                return _instance;
            }
        }

        public static T RawInstance
        {
            get
            {
                //if ((object)_instance == null) Debug.LogError("Should not be using destroyed instance unless you know for sure there will be one");
                return _instance;
            }
        }

        public static bool HasInstance()
        {
            return _instance != null;
        }

        public static bool TryGetInstance(out T instance)
        {
            instance = _instance;

            return _instance != null;
        }
    }
}
