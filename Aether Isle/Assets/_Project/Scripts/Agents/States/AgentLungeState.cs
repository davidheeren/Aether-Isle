using UnityEngine;
using StateTree;
using SpriteAnimator;
using Utilities;

namespace Game
{
    public class AgentLungeState : State
    {
        Data data;
        TargetInfo targetInfo;
        ActorComponents components;
        int? lockDepth;

        Timer windupTimer;
        Timer lungeTimer;

        Vector2 lungeDir;

        public AgentLungeState(Data data, TargetInfo targetInfo, ActorComponents components, int? lockDepth, Node child = null) : base(child)
        {
            this.data = data;
            this.targetInfo = targetInfo;
            this.components = components;
            this.lockDepth = lockDepth;

            windupTimer = new Timer(data.windupTime).Stop();
            lungeTimer = new Timer(data.lungeTime).Stop();
        }

        [System.Serializable]
        public class Data
        {
            public float range = 5;
            public float cooldownTime = 1;

            public float windupTime = 0.25f;
            public SpriteAnimation windupAnimation;

            public float lungeTime = 0.5f;
            public float speed = 5;
            public SpriteAnimation lungeAnimation;
        }

        protected override bool CanEnterState()
        {
            if (lungeTimer.IsDone) return false;
            if (!targetInfo.hasLOS) return false;

            if ((targetInfo.target.position - (Vector2)components.transform.position).sqrMagnitude > data.range * data.range)
                return false;

            return true;
        }

        protected override void EnterState()
        {
            base.EnterState();

            windupTimer.Reset();

            components.animator.Play(data.windupAnimation);
            components.animator.Pause();

            LockSuperStates(lockDepth, true);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            if (windupTimer.IsDone)
            {
                windupTimer.Stop();
                lungeTimer.Reset();

                components.animator.Play(data.lungeAnimation);
                lungeDir = (targetInfo.target.position - (Vector2)components.transform.position).normalized;
            }

            if (lungeTimer.IsStopped) 
                return;

            components.movement.Move(lungeDir * data.speed);

            if (lungeTimer.IsDone)
            {
                LockSuperStates(lockDepth, false);
            }
        }

        protected override void ExitState()
        {
            base.ExitState();

            lungeTimer.Reset().Stop();
        }
    }
}
