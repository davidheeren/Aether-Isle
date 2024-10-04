using Save;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryModel : IDisposable
    {
        readonly string id;
        public readonly int length;
        readonly ItemDatabase database;

        public readonly InventoryItem[] items;

        public InventoryModel(string id, int length, ItemDatabase database)
        {
            this.id = id;
            this.length = length;
            this.database = database;

            items = new InventoryItem[length];
            InitializeItems();

            SaveSystem.OnSave += SaveItems;
        }

        void InitializeItems()
        {
            if (SaveSystem.Data.inventories.TryGetValue(id, out HashSet<SerializedItem> sItems))
            {
                if (sItems == null) return;

                foreach (SerializedItem sItem in sItems)
                {
                    InventoryItem item = new InventoryItem(database.GetItem(sItem.databaseIndex), sItem.count);
                    if (sItem.inventoryIndex >= 0 && sItem.inventoryIndex < length)
                        items[sItem.inventoryIndex] = item;
                    else
                        Debug.LogError("Serialized Inventory Item not in range");
                }
            }
        }

        void SaveItems()
        {
            HashSet<SerializedItem> sItems = new HashSet<SerializedItem>();

            for (int i = 0; i < length; i++)
            {
                if (items[i] != null)
                {
                    int databaseIndex = database.GetIndex(items[i].item);

                    SerializedItem sItem = new SerializedItem(items[i].count, databaseIndex, i);

                    sItems.Add(sItem);
                }
            }

            SaveSystem.Data.inventories[id] = sItems;
            Debug.Log("Saved Inventory");
        }

        public void Dispose()
        {
            SaveSystem.OnSave -= SaveItems;
        }
    }
}
