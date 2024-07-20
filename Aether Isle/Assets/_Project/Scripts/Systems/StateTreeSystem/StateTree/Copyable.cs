using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public class Copyable
    {
        public Copyable() { }
        public Copyable(Copyable copy)
        {
            // Copies all serialized fields if not null
            if (copy != null)
                CopyClass(copy);
        }

        void CopyClass(Copyable copy)
        {
            string json = JsonUtility.ToJson(copy);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
