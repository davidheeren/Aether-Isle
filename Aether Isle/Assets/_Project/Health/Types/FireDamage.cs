using Game;
using UnityEngine;
using Utilities;

namespace DamageSystem
{
    public class FireDamage : Damage 
    {
        // TODO: Fire buffs debuffs
        FireDamageData data;

        Timer tickTimer;
        int tickCount = 0;

        public FireDamage(FireDamageData data, ActorComponents components, ActorComponents sourceComponents, Vector2 direction) : base(data, components, sourceComponents, direction)
        {
            this.data = data;
        }

        public override void Enter()
        {
            ApplyDamageStunKnockback(data.damage, data.stunTime, data.knockbackSpeed, direction);
            components.fireAnimator.PlayFirstAnimation();

            tickTimer = new Timer(data.fireTickDelay);
        }

        public override void Update()
        {
            if (tickTimer.IsDone)
            {
                ApplyDamageStunKnockback(data.fireTickDamage, data.fireTickStunTime, 0, Vector2.zero);
                tickCount++;
                tickTimer.Reset();
            }
        }

        public override bool ShouldExit()
        {
            return tickCount >= data.fireTickCount;
        }

        public override void Exit()
        {
            components.fireAnimator.Stop();
        }
    }
}
