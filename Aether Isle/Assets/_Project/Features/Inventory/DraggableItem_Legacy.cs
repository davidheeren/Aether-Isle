using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Legacy
{
    public class DraggableItem_Legacy : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image image;
        [HideInInspector] public Transform initialParent;
        [HideInInspector] public Transform parentAfterDrag;

        private Vector2 initialPos;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Save initial slot and position
            initialPos = transform.position;
            initialParent = transform.parent;
            parentAfterDrag = transform.parent;

            // Move image to canvas so it renders on top of other slots
            transform.SetParent(transform.root, true);
            transform.SetAsLastSibling();

            // We're already dragging this image, don't let it block raycast
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        // This is happening after OnDrop in InventorySlot potentially changed parentAfterDrag
        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
            // Slot didn't change? Restore initial position
            if (initialParent == parentAfterDrag)
            {
                transform.position = initialPos;
            }

            // Make it draggable again
            image.raycastTarget = true;
        }
    }
}
