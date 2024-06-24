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
        int? lockDepth;

        CharacterComponents components;

        Timer timer;

        DamageStats damage;
        Vector2? dir;

        bool wasDamaged;

        private CharacterStunState() : base(null, null) { }
        public CharacterStunState(string copyJson, bool canDamage, int? lockDepth, CharacterComponents components, Node child = null) : base(copyJson, child)
        {
            this.canDamage = canDamage;
            this.lockDepth = lockDepth;

            this.components = components;

            canReenter = true;

            components.health.OnDamage += OnDamage;
            components.health.OnDie += OnDie;
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

            components.health.canTakeDamage = canDamage;

            LockSuperStates(lockDepth, true);

            timer = new Timer(damage.stunTime);

            if (dir != null)
                components.rb.velocity = dir.Value * damage.knockbackSpeed;

            components.animator.enabled = false;

            SFXManager.Instance.PlaySFXClip(stunSFX, components.rb.transform.position);
        }

        protected override void UpdateState()
        {
            if (timer.isDone)
                LockSuperStates(lockDepth, false);
        }

        protected override void ExitState()
        {
            base.ExitState();

            components.health.canTakeDamage = true;

            components.animator.enabled = true;
        }
    }
}
