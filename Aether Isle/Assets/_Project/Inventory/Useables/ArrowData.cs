using DamageSystem;
using Game;
using UnityEngine;
using UnityEngine.Search;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Arrow")]
    public class ArrowData : ToolData
    {
        public float arrowSpeed = 10;
        public DamageData damageData;
        [SearchContext("p: t:prefab")]
        public ProjectileDamage projectile;

        public override Useable CreateUseable(ActorComponents components)
        {
            return new Arrow(this, components);
        }
    }
}
