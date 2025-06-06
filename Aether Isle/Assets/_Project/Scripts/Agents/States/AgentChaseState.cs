using SpriteAnimator;
using StateTree;
using Stats;
using UnityEngine;
using Utilities;

namespace Game
{
    public class AgentChaseState : State
    {
        Data data;
        ActorMoveToPoint moveToPoint;
        TargetInfo targetInfo;
        ActorComponents components;

        Pathfinding.Pathfinder pathfinder;
        Timer timer;

        public AgentChaseState(Data data, ActorMoveToPoint moveToPoint, TargetInfo targetInfo, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.moveToPoint = moveToPoint;
            this.targetInfo = targetInfo;
            this.components = components;

            pathfinder = new(null);
            timer = new Timer(data.updateTime).Stop();
        }

        [System.Serializable]
        public class Data
        {
            public float updateTime = 0.25f;
            public SpriteAnimation animation;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play(data.animation);

            timer.Reset();
        }

        protected override void UpdateState()
        {
            if (timer.IsDone)
            {
                moveToPoint.UpdatePath(targetInfo.GetKnownPosition(components.transform.position));
                timer.Reset();
            }

            moveToPoint.Move(components.stats.GetStat(StatType.moveSpeed));
        }
    }
}
