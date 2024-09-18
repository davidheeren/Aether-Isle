using Pathfinding;
using UnityEngine;
using Utilities;

namespace Game
{
    /// <summary>
    /// Helps move an agent to a destination using pathfinding and obstacle avoidance
    /// </summary>
    public class CharacterMovement
    {
        // TODO: CLEAN THIS MESS UP
        Data data;
        ObstacleAvoidance obstacleAvoidance;
        Pathfinder pathfinder;

        TargetInfo targetInfo;
        CharacterComponents components;

        Timer targetDirTimer;

        Vector2[] path;
        int pathIndex;

        const float nextWaypointDist = 0.5f;

        public CharacterMovement(Data data, TargetInfo targetInfo, ObstacleAvoidance obstacleAvoidance, CharacterComponents components)
        {
            this.data = data;
            this.targetInfo = targetInfo;
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            if (data.evaluateDirTime > 0)
                targetDirTimer = new Timer(data.evaluateDirTime);

            pathfinder = new Pathfinder(PathGrid.Instance);
        }

        [System.Serializable]
        public class Data
        {
            public float evaluateDirTime = 0.5f;
        }

        public void Enter()
        {
            targetDirTimer?.ForceDone();
        }

        public void Update(float speed)
        {
            if (targetDirTimer != null)
            {
                if (targetDirTimer.isDone)
                {
                    UpdatePath();
                    targetDirTimer.Reset();
                }
            }
            else
                UpdatePath();

            if (path == null)
            {
                Debug.Log("Path is null");
                return;
            }

            bool isAtEndOfPath = false;

            if (path.Length == 0)
                isAtEndOfPath = true;
            else
            {
                if ((path[pathIndex] - (Vector2)components.transform.position).sqrMagnitude <= nextWaypointDist * nextWaypointDist)
                {
                    if (pathIndex < path.Length - 2)
                        pathIndex++;
                }
            }

            if (pathIndex == path.Length - 1) isAtEndOfPath = true;

            Vector2 targetPos = isAtEndOfPath ? (Vector2)targetInfo.GetKnownPosition(components.transform.position) : path[pathIndex];

            if ((targetPos - (Vector2)components.transform.position).sqrMagnitude < 0.1f * 0.1f) // less than 0.1 units
                return;

            components.movement.Move(obstacleAvoidance.GetDir(targetPos) * speed);

            pathfinder.DrawPath();
        }

        void UpdatePath()
        {
            path = pathfinder.FindPath(components.transform.position, targetInfo.GetKnownPosition(components.transform.position));
            pathIndex = 0;
        }
    }
}
