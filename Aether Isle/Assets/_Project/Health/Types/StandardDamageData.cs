using Game;
using UnityEngine;

namespace DamageSystem
{
    [CreateAssetMenu(menuName = "Damage/Standard Damage")]
    public class StandardDamageData : DamageData
    {
        public override Damage CreateDamage(ActorComponents components, ActorComponents sourceComponents, Vector2 direction)
        {
            return new StandardDamage(this, components, sourceComponents, direction);
        }
    }
}
