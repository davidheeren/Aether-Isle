using SpriteAnimator;
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
        [SerializeField] SpriteAnimation stunAnimation;


        bool canDamage;
        int? lockDepth;

        CharacterComponents components;

        Timer timer;
        EventSwitch eventSwitch;

        DamageStats damage;
        Vector2? dir;

        private CharacterStunState() : base(null) { }

        public CharacterStunState Init(bool canDamage, int? lockDepth, CharacterComponents components, Node child = null)
        {
            InitializeState(child);

            this.canDamage = canDamage;
            this.lockDepth = lockDepth;

            this.components = components;

            canReenter = true;
            eventSwitch = new EventSwitch((action) => components.health.OnDamage += action);

            components.health.OnDamageParams += OnDamage;
            components.health.OnDie += OnDie;

            return this;
        }

        protected override bool CanEnterState()
        {
            return eventSwitch.happened;
        }

        private void OnDamage(DamageStats damage, Collider2D col, Collider2D source, Vector2? dir)
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

            if (stunAnimation != null)
                components.animator.Play(stunAnimation);
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
