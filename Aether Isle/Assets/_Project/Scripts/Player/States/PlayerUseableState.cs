using UnityEngine;
using StateTree;
using Inventory;
using System;

namespace Game
{
    public class PlayerUseableState : State
    {
        ActorComponents components;
        PlayerInventoryController controller;

        Useable currentUseable;

        public PlayerUseableState(ActorComponents components, PlayerInventoryController controller, Node child = null) : base(child)
        {
            this.components = components;
            this.controller = controller;
            controller.OnHotbarItemChange += ChangeHotbar;
        }

        protected override void Destroy()
        {
            base.Destroy();

            if (controller != null)
                controller.OnHotbarItemChange -= ChangeHotbar;
        }

        protected override bool CanEnterState()
        {
            if (currentUseable == null)
                return false;

            return currentUseable.ShouldUse();
        }

        protected override void EnterState()
        {
            base.EnterState();

            currentUseable?.Enter();
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            currentUseable?.Update();
        }

        protected override void ExitState()
        {
            base.ExitState();

            currentUseable?.Exit();
        }

        private void ChangeHotbar(InventoryItem item)
        {
            currentUseable?.UnEquip();

            if (item == null)
            {
                currentUseable = null;
                return;
            }

            UseableData data = item.item as UseableData;

            if (data == null)
            {
                Debug.LogWarning("Item is not useable in hotbar");
                currentUseable = null;
                return;
            }

            currentUseable = data.CreateUseable(components);
            currentUseable.Equip();
        }
    }
}
