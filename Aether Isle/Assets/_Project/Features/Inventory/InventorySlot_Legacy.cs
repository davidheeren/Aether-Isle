using UnityEngine;
using UnityEngine.EventSystems;

namespace Legacy
{
    public class InventorySlot_Legacy : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem_Legacy draggableItem = dropped.GetComponent<DraggableItem_Legacy>();

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
