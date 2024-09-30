using SpriteAnimator;
using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class AgentRandomAttack : State
    {
        Data data;
        TargetInfo targetInfo;
        ObstacleAvoidance obstacleAvoidance;
        ActorComponents components;

        Timer randomTimer;
        Timer posTimer;

        Vector2 offset;

        public AgentRandomAttack(Data data, TargetInfo targetInfo, ObstacleAvoidance obstacleAvoidance, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.targetInfo = targetInfo;
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            randomTimer = new Timer(data.randomOffsetTime);
        }

        [System.Serializable]
        public class Data
        {
            public float attackRange = 2f;
            public float randomOffsetTime = 0.5f;
            public float attackSpeed = 3;
            public SpriteAnimation animation;
        }

        protected override bool CanEnterState()
        {
            Vector2 deltaPos = targetInfo.target.position - (Vector2)components.transform.position;
            return deltaPos.sqrMagnitude <= data.attackRange * data.attackRange;
        }

        protected override void EnterState()
        {
            base.EnterState();

            UpdateOffset();
            randomTimer.Reset();

            components.animator.Play(data.animation);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            if (randomTimer.isDone)
            {
                UpdateOffset();
                randomTimer.Reset();
            }

            Vector2 targetDir = obstacleAvoidance.GetDirection(targetInfo.target.position + offset);

            components.movement.Move(targetDir * data.attackSpeed);
        }

        void UpdateOffset()
        {
            offset = Random.insideUnitCircle * data.attackRange * 0.9f;
        }
    }
}
