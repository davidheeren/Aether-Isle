using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

            // Swapping item images between slots
            if (draggableItem.initialParent != transform)
            {
                Transform currentImage = transform.GetChild(0);
                currentImage.SetParent(draggableItem.initialParent);
                currentImage.position = draggableItem.initialParent.position;
            }

            draggableItem.parentAfterDrag = transform;
            dropped.transform.position = transform.position;
        }
    }
}
