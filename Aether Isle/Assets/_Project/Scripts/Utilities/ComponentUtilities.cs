using UnityEngine;

namespace Game
{
    public static class ComponentUtilities
    {
        public static T GetRequiredComponent<T>(GameObject gameObject) where T : Component
        {
            T comp = gameObject.GetComponent<T>();

            if (comp == null)
                Debug.LogError("GameObject does not have required component of type: " + typeof(T).Name);

            return comp;
        }

        public static T GetRequiredComponentInChildren<T>(GameObject gameObject) where T : Component
        {
            T comp = gameObject.GetComponentInChildren<T>();

            if (comp == null)
                Debug.LogError("GameObject does not have required component of type: " + typeof(T).Name);

            return comp;
        }
    }
}
