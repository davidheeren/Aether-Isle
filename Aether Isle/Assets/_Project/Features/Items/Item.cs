using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "new Item", menuName = "Item")]
    public class Item: ScriptableObject
    {
        public string itemName;
        public Sprite itemIcon;
    }
}
