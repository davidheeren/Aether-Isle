using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utilities;

namespace Legacy
{
    public class InventoryManager_Legacy : Singleton<InventoryManager_Legacy>
    {
        public GameObject invWindow;
        public GameObject slotHolder;
        public GameObject[] slots;

        public List<Item_Legacy> items = new List<Item_Legacy>();

        private int invSize = 6 * 3;

        public void Start()
        {
            slots = new GameObject[invSize];

            for (int i = 0; i < invSize; i++)
            {
                slots[i] = slotHolder.transform.GetChild(i).gameObject;
            }

            //InputManager.Instance.input.Scene.ToggleInventory.performed += OnInventoryToggle;

            RefreshUI();
            ToggleInput();
        }

        private void OnInventoryToggle(InputAction.CallbackContext context)
        {
            invWindow.SetActive(!invWindow.activeInHierarchy);
            ToggleInput();
        }

        private void ToggleInput()
        {
            if (invWindow.activeInHierarchy)
            {
                InputManager.Instance.input.Game.Disable();
                InputManager.Instance.input.UI.Enable();
            }
            else
            {
                InputManager.Instance.input.UI.Disable();
                InputManager.Instance.input.Game.Enable();
            }
        }

        public void RefreshUI()
        {
            for (int i = 0; i < invSize; i++)
            {
                try
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemIcon;
                }
                catch
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
            }
        }

        public void Add(Item_Legacy item)
        {
            items.Add(item);
        }

        public void Remove(Item_Legacy item)
        {
            items.Remove(item);
        }
    }
}
