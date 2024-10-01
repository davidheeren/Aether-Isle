using UnityEngine;
using StateTree;
using SpriteAnimator;
using Stats;
using CustomInspector;

namespace Game
{
    public class AgentPatrolState : State
    {
        Data data;
        ObjectStats stats;
        ObstacleAvoidance obstacleAvoidance;
        ActorComponents components;

        WaypointHelper waypointHelper;

        public AgentPatrolState(Data data, ObjectStats stats, ObstacleAvoidance obstacleAvoidance, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.stats = stats;
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            waypointHelper = new WaypointHelper(data.waypoints, reverse: data.reverse, nextWaypointRange: 1);
        }

        [System.Serializable]
        public class Data
        {
            public float speedMultiplier = 0.5f;
            public SpriteAnimation animation;
            public bool reverse;
            [TooltipBox("If this array has 0 entries it can be overridden by the spawner")]
            public Vector2[] waypoints;
        }

        public void TrySetWaypoints(Vector2[] waypoints)
        {
            if (waypointHelper.Length > 0) return;
            waypointHelper.SetWaypoints(waypoints);
        }

        protected override bool CanEnterState()
        {
            // If there are no waypoints we cannot enter this state
            return waypointHelper.Length != 0;
        }

        protected override void EnterState()
        {
            base.EnterState();

            waypointHelper.SetCurrentIndexToClosest(components.transform.position);
            components.animator.Play(data.animation);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            Vector2? targetPos = waypointHelper.GetCurrentWaypoint(components.transform.position);
            if (targetPos == null) return; // This shouldn't happen because of Enter but just in case

            Vector2 dir = obstacleAvoidance.GetDirectionFromPoint(targetPos.Value);

            float speed = stats.GetStat(StatType.moveSpeed) * data.speedMultiplier;
            components.movement.Move(dir * speed);
        }
    }
}
