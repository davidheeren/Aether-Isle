using CustomInspector;
using DamageSystem;
using SpriteAnimator;
using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class ActorStunState : State
    {
        int? lockDepth;

        Data data;
        ActorComponents components;

        Timer timer;
        EventSwitch eventSwitch;

        float stunTime;

        public ActorStunState(Data data, int? lockDepth, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
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

        private void OnDamage(int damage, float stunTime, ActorComponents source)
        {
            this.stunTime = stunTime;
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(lockDepth, true);

            timer = new Timer(stunTime);

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
