using SpriteAnimator;
using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class CharacterStunState : State
    {
        [SerializeField] AudioClip stunSFX;
        [SerializeField] SpriteAnimation stunAnimation;

        bool disableDamageDuringStun;
        int? lockDepth;

        Data data;
        CharacterComponents components;

        Timer timer;
        EventSwitch eventSwitch;

        DamageStats damage;
        Vector2? dir;

        public CharacterStunState(Data data, bool disableDamageDuringStun, int? lockDepth, CharacterComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.disableDamageDuringStun = disableDamageDuringStun;
            this.lockDepth = lockDepth;

            this.components = components;

            canReenter = true;
            eventSwitch = new EventSwitch((action) => components.health.OnDamage += action);
            components.health.OnDamageParams += OnDamage;
        }

        [System.Serializable]
        public class Data
        {
            public AudioClip stunSFX;
            public SpriteAnimation stunAnimation;
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

        protected override void EnterState()
        {
            base.EnterState();

            if (disableDamageDuringStun)
                components.health.canTakeDamage = false;

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

            if (disableDamageDuringStun)
                components.health.canTakeDamage = true;

            components.animator.enabled = true;
        }

        protected override void Destroy()
        {
            base.Destroy();

            eventSwitch.Dispose((action) => components.health.OnDamage -= action);
            components.health.OnDamageParams -= OnDamage;
        }
    }
}
