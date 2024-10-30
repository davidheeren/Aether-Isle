using CustomEventSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PlayerHotbarView : PlayerInventoryViewBase
    {
        // Only knows about the controller
        // Publishes events when the player edits the ui view

        //[SerializeField] Image[] hotbarSlots;
        [SerializeField] Image hotbarHighlight;
        
        PlayerInventoryController controller;

        public void OnPlayerPawn(GameEventData data)
        {
            controller = data.GetData<GameObject>().GetComponent<PlayerInventoryController>();

            ReadOnlySpan<InventoryItem> hotbar = controller.Hotbar;

            if (slots.Length != hotbar.Length) 
                Debug.LogError("Hotbar Slots length do not match controller");

            for (int i  = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] != null)
                    slots[i].image.sprite = hotbar[i].item.sprite;
            }

            controller.OnHotbarIndexChange += ScrollHotbar;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (controller != null)
            {
                controller.OnHotbarIndexChange += ScrollHotbar;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            controller.OnHotbarIndexChange -= ScrollHotbar;
        }

        void ScrollHotbar(int index)
        {
            hotbarHighlight.transform.position = slots[index].transform.position;
        }
    }
}
