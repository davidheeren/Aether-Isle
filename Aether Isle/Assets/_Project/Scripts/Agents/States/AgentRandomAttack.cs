using SpriteAnimator;
using StateTree;
using Stats;
using UnityEngine;
using Utilities;

namespace Game
{
    public class AgentRandomAttack : State
    {
        Data data;
        ObjectStats stats;
        TargetInfo targetInfo;
        ObstacleAvoidance obstacleAvoidance;
        ActorComponents components;

        Timer randomTimer;
        Timer posTimer;

        Vector2 offset;

        public AgentRandomAttack(Data data, ObjectStats stats, TargetInfo targetInfo, ObstacleAvoidance obstacleAvoidance, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.stats = stats;
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
            public float speedMultiplier = 1;
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

            if (randomTimer.IsDone)
            {
                UpdateOffset();
                randomTimer.Reset();
            }

            Vector2 targetDir = obstacleAvoidance.GetDirectionFromPoint(targetInfo.target.position + offset);

            float speed = stats.GetStat(StatType.moveSpeed) * data.speedMultiplier;
            components.movement.Move(targetDir * speed);
        }

        void UpdateOffset()
        {
            offset = Random.insideUnitCircle * data.attackRange * 0.9f;
        }
    }
}
