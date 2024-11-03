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

<<<<<<< HEAD
            timer = new Timer(damage.stunTime);

            if (dir != null)
                components.rb.linearVelocity = dir.Value * damage.knockbackSpeed;
=======
            timer = new Timer(stunTime);
>>>>>>> b8623f768dc5b72aae3f97972c9fcd5ce03603f4

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
