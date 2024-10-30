using UnityEngine;

namespace Inventory
{
    public abstract class ItemData : ScriptableObject
    {
        public string id;
        public Sprite sprite;

        public virtual bool CanDisplayCount => false;
    }
}
