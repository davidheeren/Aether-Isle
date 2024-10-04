using CustomInspector;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [ReadOnly, SerializeField] public int index = -1; // Index in the Item Database

        public string displayName = "default display name";
        public Sprite sprite;

        public bool InDataBase() => index >= 0;
    }
}
