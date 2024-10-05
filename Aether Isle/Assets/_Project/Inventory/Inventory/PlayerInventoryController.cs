using CustomInspector;
using Game;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(UniqueID))]
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField] ItemDatabase database;
        [SerializeField, Button(nameof(LogInventory))] int inventoryLength = 8;

        UniqueID uniqueID;

        InventoryModel model;

        public InventoryModel Model => model;

        void Awake()
        {
            uniqueID = GetComponent<UniqueID>();
            model = new InventoryModel(uniqueID.ID, inventoryLength, database);

            
        }

        void LogInventory()
        {
            foreach (var item in model.items)
            {
                if (item == null) continue;
                print(item.item.displayName);
            }
        }


        private void OnDestroy()
        {
            model.Dispose();
        }
    }
}
