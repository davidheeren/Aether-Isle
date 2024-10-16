using Game;
using Stats;
using UnityEngine;

namespace DamageSystem
{
    public abstract class Damage
    {
        public DamageData baseData { get; private set; }
        public ActorComponents components { get; private set; }
        public ActorComponents sourceComponents { get; private set; }
        public Vector2 direction {  get; private set; }

        public Damage(DamageData baseData, ActorComponents components, ActorComponents sourceComponents, Vector2 direction)
        {
            this.baseData = baseData;
            this.components = components;
            this.sourceComponents = sourceComponents;
            this.direction = direction;
        }

        protected void ApplyDamageStunKnockback(int damage, float stunTime, float speed, Vector2 direction)
        {
            speed = sourceComponents.stats.GetStat(StatType.knockbackApply, speed);
            speed = components.stats.GetStat(StatType.knockbackReceive, speed);
            components.rb.velocity = direction * speed;

            stunTime = sourceComponents.stats.GetStat(StatType.stunTimeApply, stunTime);
            stunTime = components.stats.GetStat(StatType.stunTimeReceive, stunTime);
            components.health.Damage(baseData.damage, stunTime, sourceComponents);
        }

        public abstract void Enter();
        public virtual void Update() { }
        public abstract bool ShouldExit();
        public virtual void Exit() { }


        public bool CompareData(Damage damage)
        {
            return damage.baseData == baseData;
        } 
    }
}
