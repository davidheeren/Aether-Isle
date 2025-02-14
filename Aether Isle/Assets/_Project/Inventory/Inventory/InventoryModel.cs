using Save;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryModel
    {
        // Only holds data
        // Doesn't know about the view or controller
        // Has events and methods to change internal data

        readonly string id;
        readonly ItemDatabase database;

        readonly InventoryItem[] items;
        public int Length => items.Length;
        public ReadOnlySpan<InventoryItem> Items => items;

        public event Action OnModelChanged;

        public InventoryModel(string id, int length, ItemDatabase database)
        {
            this.id = id;
            this.database = database;

            items = new InventoryItem[length];
            InitializeItems();

            SaveSystem.OnSave += SaveItems;
        }

        public void Clear()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = null;
            }
        }

        public InventoryItem GetItem(int index)
        {
            ValidateRange(index);

            return items[index];
        }

        public void SetItem(int index, InventoryItem item)
        {
            ValidateRange(index);

            items[index] = item;

            OnModelChanged?.Invoke();
        }

        public ReadOnlySpan<InventoryItem> GetSpan(int start, int end)
        {
            ValidateRange(start);
            ValidateRange(end - 1);

            return items.AsSpan(start, end);
        }

        public ReadOnlySpan<InventoryItem> GetSpan(Range range)
        {
            ValidateRange(range.Start.Value);
            ValidateRange(range.End.Value - 1);

            return items.AsSpan(range);
        }

        public void SetSpan(int start, ReadOnlySpan<InventoryItem> newItems)
        {
            if (newItems == null) throw new ArgumentNullException("Items span cannot be null");

            int end = start + newItems.Length;
            ValidateRange(start);
            ValidateRange(end- 1);

            for (int i = 0; i < newItems.Length; i++)
            {
                items[i + start] = newItems[i];
            }

            OnModelChanged?.Invoke();
        }

        public void SetSpan(Range range, ReadOnlySpan<InventoryItem> newItems)
        {
            if (range.End.Value - range.Start.Value != newItems.Length) throw new ArgumentException("Range length is not equal to span length");

            SetSpan(range.Start.Value, newItems);
        }

        public delegate InventoryItem EditInventory(int index, InventoryItem item);
        public void EditSpan(int start, int end, EditInventory action)
        {
            ValidateRange(start);
            ValidateRange(end - 1);

            int j = 0;
            for (int i = start; i < end; i++)
            {
                items[i] = action.Invoke(j, items[i]);
                j++;
            }

            OnModelChanged?.Invoke();
        }

        public void EditSpan(Range range, EditInventory action)
        {
            EditSpan(range.Start.Value, range.End.Value, action);
        }

        public void SwapItems(int aIndex, int bIndex)
        {
            ValidateRange(aIndex);
            ValidateRange(bIndex);

            InventoryItem temp = items[aIndex];
            items[aIndex] = items[bIndex];
            items[bIndex] = temp;

            OnModelChanged?.Invoke();
        }

        #region Save and Load
        void InitializeItems()
        {
            if (SaveSystem.Data.inventories.TryGetValue(id, out HashSet<SerializedItem> sItems))
            {
                if (sItems == null) return;

                foreach (SerializedItem sItem in sItems)
                {
                    ItemData itemData = database.GetItem(sItem.id);
                    if (itemData == null) continue;

                    InventoryItem item = new InventoryItem(itemData, sItem.count);
                    if (sItem.inventoryIndex >= 0 && sItem.inventoryIndex < Length)
                        items[sItem.inventoryIndex] = item;
                    else
                        Debug.LogError("Serialized Inventory Item not in range");
                }
            }
        }

        void SaveItems()
        {
            HashSet<SerializedItem> sItems = new HashSet<SerializedItem>();

            for (int i = 0; i < Length; i++)
            {
                if (items[i] != null)
                {
                    SerializedItem sItem = new SerializedItem(items[i].item.id, items[i].count, i);

                    sItems.Add(sItem);
                }
            }

            SaveSystem.Data.inventories[id] = sItems;
            Debug.Log("Saved Inventory");

            SaveSystem.OnSave -= SaveItems;
        }
        #endregion

        void ValidateRange(int index)
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("Index is out of range of inventory");
        }
    }
}
