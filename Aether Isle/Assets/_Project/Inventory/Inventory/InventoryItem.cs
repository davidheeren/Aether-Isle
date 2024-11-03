using Game;
using System;

namespace Inventory
{
    public class InventoryItem
    {
        public readonly ItemData item;
        public readonly int count;

        public readonly Type itemType;

        public InventoryItem(ItemData item, int count)
        {
            this.item = item;
            this.count = count;

            itemType = item.GetType();
        }
    }
}
