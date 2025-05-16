using UnityEngine;

namespace Inventory
{
    public abstract class ItemData : ScriptableObject
    {
        public string id;
        public Sprite sprite;

        public virtual bool Stackable => false;

        [ContextMenu("Set id to name")]
        private void SetIdToName()
        {
            id = name;
        }

        [ContextMenu("Set id to type")]
        private void SetIdToType()
        {
            id = GetType().Name;
        }
    }
}
