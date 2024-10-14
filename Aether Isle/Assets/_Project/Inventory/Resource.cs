using Game;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Resource")]
    public class Resource : Item
    {
        public void Use(ActorComponents components)
        {
            Debug.Log($"Use: {id} resource");
        }
    }
}
