using System.Linq;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        public Item[] items;

        Item[] oldItems = new Item[0];

        // Called when the array is modified in the inspector but before the changes are applied
        public void OnItemsChanged()
        {
            oldItems = new Item[items.Length];
            items.CopyTo(oldItems, 0);
        }

        // Called when the array is modified and applied
        public void OnItemsChangesApplied()
        {
            SetOldItemIndices();
            SetItemIndices();
        }

        void SetOldItemIndices()
        {
            foreach (Item oldItem in oldItems)
            {
                if (oldItem == null) 
                    continue;

                // If the updated array contains an old item, the old item has been removed
                if (!items.Contains(oldItem))
                {
                    oldItem.index = -1;
                }
            }
        }

        void SetItemIndices()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                    continue;

                items[i].index = i;
            }
        }

        public void OnEditorDisable()
        {
            bool isValid = true;

            // Check for duplicates
            if (items.ToHashSet().Count != items.Length)
            {
                Debug.LogError("Item Database has duplicate items");
                isValid = false;
            }

            // Check for null entries
            if (items.Contains(null))
            {
                Debug.LogError("Item Database has null values");
                isValid = false;
            }

            // Let user know database is valid
            if (isValid)
            {
                Debug.Log("Changed Item Database successfully");
            }
        }
    }
}
