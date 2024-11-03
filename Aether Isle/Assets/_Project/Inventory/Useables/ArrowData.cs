using DamageSystem;
using Game;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Arrow")]
    public class ArrowData : ToolData
    {
        public float arrowSpeed = 10;
        public DamageData damageData;
        public ProjectileDamage projectile;

        public override Useable CreateUseable(ActorComponents components)
        {
            return new Arrow(this, components);
        }
    }
}
