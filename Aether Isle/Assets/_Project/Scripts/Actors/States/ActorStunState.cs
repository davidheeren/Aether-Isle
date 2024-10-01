using CustomInspector;
using SpriteAnimator;
using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class ActorStunState : State
    {
        bool disableDamageDuringStun;
        int? lockDepth;

        Data data;
        ActorComponents components;

        Timer timer;
        EventSwitch eventSwitch;

        DamageData damage;
        Vector2? dir;

        public ActorStunState(Data data, bool disableDamageDuringStun, int? lockDepth, ActorComponents components, Node child = null) : base(child)
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
            [TooltipBox("If null, animator pauses")]
            public SpriteAnimation stunAnimation;
        }

        protected override bool CanEnterState()
        {
            bool happened = eventSwitch.Happened;
            return happened;
        }

        private void OnDamage(DamageData damage, Collider2D col, Collider2D source, Vector2? dir)
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

            SFXManager.Instance.PlaySFXClip(data.stunSFX, components.transform.position);

            if (data.stunAnimation != null)
                components.animator.Play(data.stunAnimation);
            else
                components.animator.Pause();
        }

        protected override void UpdateState()
        {
            if (timer.IsDone)
                LockSuperStates(lockDepth, false);
        }

        protected override void ExitState()
        {
            base.ExitState();

            if (disableDamageDuringStun)
                components.health.canTakeDamage = true;

            components.animator.Resume();
        }

        protected override void Destroy()
        {
            base.Destroy();

            eventSwitch.Dispose((action) => components.health.OnDamage -= action);
            components.health.OnDamageParams -= OnDamage;
        }
    }
}
