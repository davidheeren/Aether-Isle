using Game;
using UnityEngine;

namespace DamageSystem
{
    public class StandardDamage : Damage
    {
        StandardDamageData data;
        public StandardDamage(StandardDamageData data, ActorComponents components, ActorComponents sourceComponents, Vector2 direction) : base(data, components, sourceComponents, direction)
        {
            this.data = data;
        }

        public override bool ShouldExit()
        {
            return true;
        }

        public override void Enter()
        {
            ApplyDamageStunKnockback(data.damage, data.stunTime, data.knockbackSpeed, direction);
        }
    }
}
