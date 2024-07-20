using StateTree;
using System;
using UnityEngine;
using Utilities;
using static Unity.VisualScripting.Member;

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
        EventSwitch eventSwitch;

        DamageStats damage;
        Vector2? dir;


        public CharacterStunState Create(bool canDamage, int? lockDepth, CharacterComponents components, Node child = null)
        {
            CreateState(child);

            this.canDamage = canDamage;
            this.lockDepth = lockDepth;

            this.components = components;

            canReenter = true;
            eventSwitch = new EventSwitch(ref components.health.OnDamage);

            components.health.OnDamageParams += OnDamage;
            components.health.OnDie += OnDie;

            return this;
        }

        protected override bool CanEnterState()
        {
            return eventSwitch.happened;
        }

        private void OnDamage(DamageStats damage, Collider2D col, Vector2? dir)
        {
            this.damage = damage;
            this.dir = dir;
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
