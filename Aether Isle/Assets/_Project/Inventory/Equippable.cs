using Game;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Equipable")]
    public class Equippable : Item
    {
        public void Use(ActorComponents components)
        {
            Debug.Log($"Use: {id} equipable");
        }
    }
}
