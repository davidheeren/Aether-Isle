using UnityEngine;
using StateTree;
using SpriteAnimator;
using Utilities;
using Stats;

namespace Game
{
    public class AgentWanderState : State
    {
        Data data;
        ObstacleAvoidance obstacleAvoidance;
        ActorComponents components;

        Timer timer;

        Vector2 targetDir;
        Vector2 home;

        public AgentWanderState(Data data, ObstacleAvoidance obstacleAvoidance, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            timer = new Timer(data.randomDirectionTime).Stop();
            home = components.transform.position;
        }

        [System.Serializable]
        public class Data
        {
            public float speedMultiplier = 0.65f;
            public float randomDirectionTime = 1.5f;
            public float randomDirectionDegrees = 180;
            public float rangeDistance = 15;
            public bool updateHome = false;
            public SpriteAnimation animation;
        }

        protected override void EnterState()
        {
            base.EnterState();

            timer.Reset();
            UpdateTargetDir();

            if (data.updateHome) 
                home = components.transform.position;
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            if (timer.IsDone)
            {
                UpdateTargetDir();
                timer.Reset();
            }

            Vector2 dir = obstacleAvoidance.GetDirectionFromDirection(targetDir);

            float speed = components.stats.GetStat(StatType.moveSpeed) * data.speedMultiplier;
            components.movement.Move(dir * speed);
        }

        void UpdateTargetDir()
        {
            Vector2 currentDir = components.movement.targetVelocity;

            float angle = Mathf.Atan2(currentDir.y, currentDir.x);

            float angleOffset = Random.Range(-data.randomDirectionDegrees / 2, data.randomDirectionDegrees / 2) * Mathf.Deg2Rad;

            targetDir = Maths.DirectionFromAngle(angle + angleOffset);

            // Weight towards home
            Vector2 homeDir = home - (Vector2)components.transform.position;
            targetDir += homeDir / data.rangeDistance;
            targetDir.Normalize();
        }
    }
}
