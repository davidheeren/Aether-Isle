using CustomInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        // TODO:
        //   Make generic type
        //   Delete Duplicates and nulls methods
        //   Get all types methods
        //   Delete types with "test" in their name

        [Button(nameof(SetAllItems))]
        [Button(nameof(RemoveNullAndDuplicates))]
        [Button(nameof(RemoveTestItems))]
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


        private void SetAllItems()
        {
#if UNITY_EDITOR

            List<ItemData> allItems = new List<ItemData>();

            string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets" });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // Load the asset as an Item and add it to the list if it is valid
                ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

                if (itemData != null)
                {
                    allItems.Add(itemData);
                }
            }

            items = allItems.ToArray();
            SortItems();

#endif
        }

        private void RemoveTestItems()
        {
            List<ItemData> newItems = new List<ItemData>();

            foreach (ItemData item in items)
            {
                // Or could be item.name
                if (item.id.ToLower().Contains("test"))
                    continue;

                newItems.Add(item);
            }

            items = newItems.ToArray();
        }

        private void RemoveNullAndDuplicates()
        {
            HashSet<ItemData> newItems = new HashSet<ItemData>();

            foreach (ItemData item in items)
            {
                if (item == null)
                    continue;

                newItems.Add(item);
            }

            items = newItems.ToArray();
        }

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
