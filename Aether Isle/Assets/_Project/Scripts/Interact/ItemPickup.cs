using Game;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    public class ItemPickup : InteractableMB
    {
        [SerializeField] ItemData item;

        private void OnValidate()
        {
            if (item == null) return;

#if UNITY_EDITOR
            // Delay the call to ensure it safely updates in the editor.
            EditorApplication.delayCall += () =>
            {
                if (this == null) return; // Some error when making prefab
                GetComponent<SpriteRenderer>().sprite = item.sprite;
            };
#endif
        }

        public override bool CanContinue()
        {
            return false;
        }

        public ItemPickup Spawn(Vector2 position, ItemData item)
        {
            ItemPickup spawn = Instantiate(this, position, Quaternion.identity);

            spawn.spriteRenderer.sprite = item.sprite;
            spawn.item = item;

            return spawn;
        }

        public override void EnterInteract(ActorComponents components)
        {
            base.EnterInteract(components);

            if (item == null) return;

            if (!components.TryGetComponent<PlayerInventoryController>(out PlayerInventoryController controller))
            {
                Debug.LogError("Player does not have an InventoryManager attached");
                return;
            }

            for (int i = controller.hotbarRange.Start.Value; i < controller.hotbarRange.End.Value; i++)
            {
                if (controller.Model.GetItem(i) == null)
                {
                    controller.Model.SetItem(i, new InventoryItem(item, 1));
                    print($"Added {item.id} to hotbar");
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
