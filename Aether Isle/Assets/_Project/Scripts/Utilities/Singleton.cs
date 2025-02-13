using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance => GetInstanceHelper(_instance);

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

        public static T GetInstanceHelper(T instance)
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<T>();
            if (_instance == null)
            {
                //Debug.Log("Created Singleton of type: " + typeof(T));
                GameObject obj = new GameObject(typeof(T) + " Singleton");
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        }
    }
}
