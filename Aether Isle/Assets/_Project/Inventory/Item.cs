using CustomInspector;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        public string id;
        public Sprite sprite;
    }
}
