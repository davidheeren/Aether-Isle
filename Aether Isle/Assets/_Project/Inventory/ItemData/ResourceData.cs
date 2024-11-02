using Game;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Resource")]
    public class ResourceData : ItemData
    {
        public override bool Stackable => true;
    }
}
