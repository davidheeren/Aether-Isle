using Game;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    [RequireComponent(typeof(UniqueID))]
    public class PlayerInventoryController : MonoBehaviour
    {
        // Responsible for manipulating the model directly
        // Listens for the view to updates on the model

        [SerializeField] ItemDatabase database;
        [SerializeField] ItemData[] testItems;

        UniqueID uniqueID;

        InventoryModel model;
        public InventoryModel Model => model;

        #region Indices
        const int helmetIndex = 0;
        const int breastPlateIndex = 1;
        const int bootIndex = 2;
        const int hotbarLength = 8;
        const int equipablesLength = 20;
        const int resourcesLength = 20;
        const int wearablesLength = 20;

        public readonly Range hotbarRange = new Range(bootIndex + 1, bootIndex + 1 + hotbarLength);

        const int inventoryLength = bootIndex + hotbarLength + equipablesLength + resourcesLength + wearablesLength + 1;

        public ReadOnlySpan<InventoryItem> Hotbar => model.GetSpan(hotbarRange);
        #endregion

        public event Action<InventoryItem> OnHotbarItemChange;
        public event Action<int> OnHotbarIndexChange;

        public int currentHotbarIndex { get; private set; }

        void Awake()
        {
            uniqueID = GetComponent<UniqueID>();
            model = new InventoryModel(uniqueID.ID, inventoryLength, database);
            testItems = new ItemData[hotbarLength];
        }

        private void OnEnable()
        {
            InputManager.Instance.input.Game.Scroll.performed += OnScroll;
            InputManager.Instance.input.Game.HotbarNumber.performed += OnHotbarNumber;
            OnScroll(0);
        }

        private void OnDisable()
        {
            if (!InputManager.HasInstance()) return;
            InputManager.RawInstance.input.Game.Scroll.performed -= OnScroll;
            InputManager.RawInstance.input.Game.HotbarNumber.performed -= OnHotbarNumber;
        }


        void OnScroll(InputAction.CallbackContext context)
        {
            int offset = -(int)context.ReadValue<float>(); // we flip positive/negative

            OnScroll(offset);
        }

        void OnScroll(int offset)
        {
            for (int i = 1; i < hotbarLength; i++)
            {
                int index = (currentHotbarIndex + hotbarLength + offset * i) % hotbarLength;
                InventoryItem item = model.GetItem(hotbarRange.Start.Value + index);

                if (item != null)
                {
                    currentHotbarIndex = index;
                    OnHotbarIndexChange?.Invoke(currentHotbarIndex);
                    OnHotbarItemChange?.Invoke(item);
                    break;
                }
            }

            //currentHotbarIndex = (currentHotbarIndex + hotbarLength + offset) % hotbarLength;
            //OnHotbarIndexChange?.Invoke(currentHotbarIndex);

            //InventoryItem item = model.GetItem(hotbarRange.Start.Value + currentHotbarIndex);
            //OnHotbarItemChange?.Invoke(item);
        }

        void OnHotbarNumber(InputAction.CallbackContext context)
        {
            int index = (int)context.ReadValue<float>() - 1;

            if (index == currentHotbarIndex)
                return;

            InventoryItem item = model.GetItem(hotbarRange.Start.Value + index);

            if (item != null)
            {
                currentHotbarIndex = index;
                OnHotbarIndexChange?.Invoke(currentHotbarIndex);
                OnHotbarItemChange?.Invoke(item);
            }
        }

        [ContextMenu("Set Test Hotbar")]
        void SetTestItems()
        {
            print("Set Items");

            model.EditSpan(hotbarRange, (i, item) => testItems[i] == null ? null : new InventoryItem(testItems[i], 1));
        }

        [ContextMenu("Log Inventory")]
        void LogInventory()
        {
            foreach (InventoryItem item in model.Items)
            {
                print(item?.item.id);
            }
        }

        public void ClearInventory()
        {
            model.Clear();
            //OnHotbarItemChange?.Invoke();
        }
    }
}
