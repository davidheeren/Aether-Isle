using Pathfinding;
using UnityEngine;
using Utilities;

namespace Game
{
    /// <summary>
    /// Helps move an agent to a destination using pathfinding and obstacle avoidance
    /// </summary>
    public class ActorMoveToPoint
    {
        ObstacleAvoidance obstacleAvoidance;
        CharacterComponents components;

        Pathfinder pathfinder;
        WaypointHelper waypointHelper;

        public ActorMoveToPoint(ObstacleAvoidance obstacleAvoidance, CharacterComponents components, PathGrid pathGrid = null)
        {
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            pathfinder = new Pathfinder(pathGrid);
            waypointHelper = new WaypointHelper(null, loop: false);
        }

        public void UpdatePath(Vector2 target)
        {
            Vector2[] path = pathfinder.FindPath(components.transform.position, target);
            waypointHelper.SetWaypoints(path).Reset();
        }

        public void Move(float speed)
        {
            Vector2? targetPos = waypointHelper.GetCurrentWaypoint(components.transform.position);

            if (targetPos == null) return;

            components.movement.Move(obstacleAvoidance.GetDirection(targetPos.Value) * speed);

            waypointHelper.DrawWaypoints();
        }
    }
}