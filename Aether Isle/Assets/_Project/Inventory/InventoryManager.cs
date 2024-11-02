using CustomInspector;
using Inventory;
using Save;
using UnityEngine;





    public class InventoryManager : MonoBehaviour
    {
        //[SerializeField] ItemDatabase database;
        //public Item[] items = new Item[20];

        private void Awake()
        {
            InitInventory();
        }

        void InitInventory()
        {
           // foreach (Item item in database.items)
            {
                //if (SaveSystem.SaveData.inventory.TryGetValue(item.index, out int value))
                {
                    //items[value] = item;
                }
            }
        }

        public void AddItem()
        {

        }

        public void RemoveItem()
        {

        }

        public void SwapItems()
        {

        }
    }

