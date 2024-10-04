using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    [RequireComponent(typeof(Image))]
    public class UIInventorySlot : MonoBehaviour
    {
        [SerializeField] int index;

        Image image;

        void Awake()
        {
            image = GetComponent<Image>();
        }
    }
}
