using StateTree;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class CharacterStunState : State
    {
        [SerializeField] AudioClip stunSFX;

        bool canDamage;
        Health health;
        Rigidbody2D rb;
        int? lockDepth;
        Animator animator;

        Timer timer;

        DamageStats damage;
        Vector2? dir;

        bool wasDamaged;

        private CharacterStunState() : base(null, null) { }
        public CharacterStunState(string copyJson, bool canDamage, Health health, Rigidbody2D rb, int? lockDepth, Animator animator, Node child = null) : base(copyJson, child)
        {
            this.canDamage = canDamage;
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

            health.canTakeDamage = canDamage;

            LockSuperStates(lockDepth, true);

            timer = new Timer(damage.stunTime);

            if (dir != null)
                rb.velocity = dir.Value * damage.knockbackSpeed;

            animator.enabled = false;

            SFXManager.Instance.PlaySFXClip(stunSFX, rb.transform.position);
        }

        protected override void UpdateState()
        {
            if (timer.isDone)
                LockSuperStates(lockDepth, false);
        }

        protected override void ExitState()
        {
            base.ExitState();

            health.canTakeDamage = true;

            animator.enabled = true;
        }
    }
}
