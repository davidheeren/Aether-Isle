using UnityEngine;

namespace Save
{
    public static class SerializationExtensions
    {
        public static T CopyJson<T>(this object obj)
        {
            return JsonUtility.FromJson<T>(JsonUtility.ToJson(obj));
        }
    }
}
