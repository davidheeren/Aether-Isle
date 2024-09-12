using UnityEngine;

namespace StateTree
{
    // DO NOT USE
    public class Copyable
    {
        public Copyable(Copyable copy)
        {
            if (copy == null) return;
            Copy(copy);
        }

        /// <summary>
        /// Sets this class to a new class with the same public/serialized fields as the copy reference
        /// <typeparam name="T"></typeparam>
        /// <param name="copy">The reference to copy public/serialized fields</param>
        private void Copy(Copyable copy)
        {
            string copyJson = JsonUtility.ToJson(copy);
            if (string.IsNullOrEmpty(copyJson))
            {
                //Debug.LogError($"Failed to serialize )} to JSON.");
                return;
            }

            //Debug.Log(copyJson);
            JsonUtility.FromJsonOverwrite(copyJson, this);
        }
    }
}
