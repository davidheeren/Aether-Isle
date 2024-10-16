using Game;
using Stats;
using UnityEngine;

namespace DamageSystem
{
    [CreateAssetMenu(menuName = "Damage/Ice Damage")]
    public class IceDamageData : DamageData
    {
        public float iceDuration = 3;
        public StatModifier iceModifier;

        public override Damage CreateDamage(ActorComponents components, ActorComponents sourceComponents, Vector2 direction)
        {
            return new IceDamage(this, components, sourceComponents, direction);
        }
    }
}
