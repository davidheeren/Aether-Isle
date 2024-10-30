using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public abstract class PlayerInventoryViewBase : MonoBehaviour
    {
        [SerializeField] protected UIInventorySlot[] slots;

        protected static List<PlayerInventoryViewBase> views = new List<PlayerInventoryViewBase>();

        protected virtual void OnEnable()
        {
            views.Add(this);
        }

        protected virtual void OnDisable()
        {
            views.Remove(this);
        }
    }
}
