using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [SerializeField] Image[] slots;
        
        PlayerInventoryController controller;

        public void OnPlayerPawn(GameEventData data)
        {
            controller = data.GetData<GameObject>().GetComponent<PlayerInventoryController>();

            for (int i = 0; i < slots.Length; i++)
            {
                InventoryItem item = controller.Model.items[i];
                if (item != null)
                    slots[i].sprite = item.item.sprite;
            }
        }
    }
}
