using CustomInspector;
using Game;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(UniqueID))]
    public class PlayerInventoryController : MonoBehaviour
    {
        [Button(nameof(SetTestItems))]
        [SerializeField] ItemDatabase database;
        [SerializeField, Button(nameof(LogInventory))] int inventoryLength = 8;
        [SerializeField] Item[] testItems;

        UniqueID uniqueID;

        InventoryModel model;

        public InventoryModel Model => model;

        void Awake()
        {
            uniqueID = GetComponent<UniqueID>();
            model = new InventoryModel(uniqueID.ID, inventoryLength, database);
        }

        void SetTestItems()
        {
            print("Set Items");
            model.TestSetItems(testItems);
        }

        void LogInventory()
        {
            foreach (var item in model.items)
            {
                if (item == null) continue;
                print(item.item.id);
            }
        }


        private void OnDestroy()
        {
            model.Dispose();
        }
    }
}
