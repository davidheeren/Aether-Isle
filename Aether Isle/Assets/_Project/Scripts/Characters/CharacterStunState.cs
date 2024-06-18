using StateTree;
using System;
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
        Animator animator;

        Timer timer;

        DamageStats damage;
        Vector2? dir;

        bool wasDamaged;

        public CharacterStunState(Health health, Rigidbody2D rb, int? lockDepth, Animator animator, Node child = null) : base(null, child)
        {
            this.health = health;
            this.rb = rb;
            this.lockDepth = lockDepth;
            this.animator = animator;

            canReenter = true;

            health.OnDamage += OnDamage;
            health.OnDie += OnDie;
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

        private void OnDie()
        {
            canReenter = false;
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(lockDepth, true);

            timer = new Timer(damage.stunTime);

            if (dir != null)
                rb.velocity = dir.Value * damage.knockbackSpeed;

            animator.enabled = false;
        }

        protected override void UpdateState()
        {
            if (timer.isDone)
                LockSuperStates(lockDepth, false);
        }

        protected override void ExitState()
        {
            base.ExitState();

            animator.enabled = true;
        }
    }
}
