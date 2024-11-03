using Game;
using UnityEngine;
using Utilities;

namespace DamageSystem
{
    public class IceDamage : Damage
    {
        IceDamageData data;

        Timer timer;

        public IceDamage(IceDamageData data, ActorComponents components, ActorComponents sourceComponents, Vector2 direction) : base(data, components, sourceComponents, direction)
        {
            this.data = data;

            if (data.iceModifier.time != 0)
                Debug.LogError("Ice modifier duration should be 0");
        }

        public override void Enter()
        {
            ApplyDamageStunKnockback(data.damage, data.stunTime, data.knockbackSpeed, direction);

            components.stats.AddModifier(data.iceModifier);
            components.spriteRenderer.SetPropertyFloat("_IceFactor", 1);
            timer = new Timer(data.iceDuration);
        }

        public override bool ShouldExit()
        {
            return timer.IsDone;
        }

        public override void Exit()
        {
            components.stats.RemoveModifier(data.iceModifier);
            components.spriteRenderer.SetPropertyFloat("_IceFactor", 0);
        }
    }
}
