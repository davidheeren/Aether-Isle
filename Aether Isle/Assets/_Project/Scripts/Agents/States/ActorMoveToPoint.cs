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
        ActorComponents components;

        //Pathfinder pathfinder;
        PathRequestManager pathRequestManager;
        WaypointHelper waypointHelper;

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public ActorMoveToPoint(ObstacleAvoidance obstacleAvoidance, ActorComponents components, PathGrid pathGrid = null)
        {
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            //pathfinder = new Pathfinder(pathGrid);
            pathRequestManager = PathRequestManager.Instance;
            waypointHelper = new WaypointHelper(null, loop: false);
        }

        public void UpdatePath(Vector2 target)
        {
            //Vector2[] path = pathfinder.FindPath(components.transform.position, target);
            //waypointHelper.SetWaypoints(path).ResetIndex();

            sw.Restart();
            pathRequestManager.FindPath(components.transform.position, target, OnPathFound);
        }

        private void OnPathFound(Vector2[] path)
        {
            waypointHelper.SetWaypoints(path).ResetIndex();

            Debug.Log($"Actor received callback path received in: {sw.ElapsedMilliseconds} ms");
        }

        public void Move(float speed)
        {
            Vector2? targetPos = waypointHelper.GetCurrentWaypoint(components.transform.position);

            if (targetPos == null) return;

            components.movement.Move(obstacleAvoidance.GetDirectionFromPoint(targetPos.Value) * speed);

            waypointHelper.DrawWaypoints();
        }
    }
}
