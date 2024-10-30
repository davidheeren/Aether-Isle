using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    [RequireComponent(typeof(Image))]
    public class UIInventorySlot : MonoBehaviour
    {
        public Image image { get; private set; }

        void Awake()
        {
            image = GetComponent<Image>();
        }
    }
}
