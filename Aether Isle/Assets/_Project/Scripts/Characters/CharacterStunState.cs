using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class CharacterStunState : State
    {
        // Automatically locks and returns null if 

        Health health;
        Rigidbody2D rb;
        int? lockDepth;

        Timer timer;

        DamageStats damage;
        Vector2? dir;

        bool wasDamaged;

        public CharacterStunState(Health health, Rigidbody2D rb, int? lockDepth, Node child) : base(null, child)
        {
            this.health = health;
            this.rb = rb;
            this.lockDepth = lockDepth;

            canReenter = true;

            health.OnDamage += OnDamage;
        }

        protected override bool CanEnterState()
        {
            if (wasDamaged)
            {
                wasDamaged = false;
                return true;
            }

            return false;
        }

        private void OnDamage(DamageStats damage, Vector2? dir)
        {
            this.damage = damage;
            this.dir = dir;

            wasDamaged = true;
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(lockDepth, true);

            timer = new Timer(damage.stunTime);

            if (dir != null)
                rb.velocity = dir.Value * damage.knockbackSpeed;
        }

        protected override void UpdateState()
        {
            if (timer.isDone)
                LockSuperStates(lockDepth, false);
        }
    }
}
