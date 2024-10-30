using CustomInspector;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [Button(nameof(ValidateItems))]
        [Button(nameof(SortItems))]
        [ReadOnly] public string buttons = "buttons";

        [SerializeField] ItemData[] items;

        public ItemData GetItem(string id)
        {
            ItemData item = items.FirstOrDefault(x => x.id == id);
            if (item == null) Debug.LogError($"Item: {id} does not exist in database");
            return item;
        }

        private void SortItems()
        {
            ItemData[] sortedItems = items.OrderBy(item => item.GetType().Name)
                            .ThenBy(item => item.id)
                            .ToArray();

            items = sortedItems;
        }

        // Called when the array is modified in the inspector but before the changes are applied
        //public void OnItemsChanged() { }
        // Called when the array is modified and applied
        //public void OnItemsChangesApplied() { }
        //public void OnEditorDisable()
        //{
        //    ValidateItems();
        //}

        private void ValidateItems()
        {
            if (IsValid())
            {
                Debug.Log("All Items are valid");
            }
        }

        public bool IsValid()
        {
            bool isValid = true;

            // Check for null entries
            if (items.Contains(null))
            {
                Debug.LogError("Item Database has null values");
                isValid = false;
                return false;
            }

            // Check for duplicate items
            if (items.ToHashSet().Count != items.Length)
            {
                Debug.LogError("Item Database has duplicate items");
                isValid = false;
            }

            // Check for duplicate ids
            ItemData[] duplicateIDItems = items.GroupBy(item => item.id)
                                         .Where(group => group.Count() > 1)
                                         .SelectMany(group => group)
                                         .ToArray();

            if (duplicateIDItems.Length > 0)
            {
                Debug.LogError("Item Database has duplicate item ids");

                foreach (ItemData item in duplicateIDItems)
                {
                    Debug.LogError($"Duplicate ID => Item: {item.name} -- ID: {item.id}");
                }

                isValid = false;
            }

            // Check for empty ids
            if (items.Any(x => string.IsNullOrEmpty(x.id)))
            {
                Debug.LogError("Item Database has empty id values");
                isValid = false;
            }

            return isValid;
        }
    }
}
