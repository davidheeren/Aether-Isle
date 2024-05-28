using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public class Copyable
    {
        public Copyable(string copyJson)
        {
            // Copies all serialized fields if not null
            if (!String.IsNullOrEmpty(copyJson))
                JsonUtility.FromJsonOverwrite(copyJson, this);
        }

        /// <summary>
        /// Gets the json string of all serialized field values
        /// </summary>
        /// <returns></returns>
        public string CopyJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
