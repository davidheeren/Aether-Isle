using System;

namespace Inventory
{
    public class InventoryItem
    {
        public readonly Item item;
        public readonly int count;

        public readonly Type itemType;

        public InventoryItem(Item item, int count)
        {
            this.item = item;
            this.count = count;

            itemType = item.GetType();
        }
    }
}
