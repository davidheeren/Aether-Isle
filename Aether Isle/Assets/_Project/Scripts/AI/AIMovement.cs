using Pathfinding;
using StateTree;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class AIMovement
    {
        [SerializeField] float evaluateDirTime = 0.25f;
        const float nextWaypointDist = 0.5f;

        ObstacleAvoidance obstacleAvoidance;
        Pathfinder pathfinder;

        TargetInfo targetInfo;
        CharacterComponents components;

        Timer targetDirTimer;

        Vector2[] path;
        int pathIndex;

        public void Init(TargetInfo targetInfo, ObstacleAvoidance obstacleAvoidance, CharacterComponents components)
        {
            this.targetInfo = targetInfo;
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            if (evaluateDirTime > 0)
                targetDirTimer = new Timer(evaluateDirTime);

            obstacleAvoidance.Setup(components.transform);
            pathfinder = new Pathfinder(PathGrid.Instance);
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

            Vector2 targetPos = isAtEndOfPath ? (Vector2)targetInfo.GetSmartPosition(components.transform.position) : path[pathIndex];

            if ((targetPos - (Vector2)components.transform.position).sqrMagnitude < 0.1f * 0.1f) // less than 0.1 units
                return;

            components.movement.Move(obstacleAvoidance.GetDir(targetPos) * speed);

            pathfinder.DrawPath();
        }

        void UpdatePath()
        {
            path = pathfinder.FindPath(components.transform.position, targetInfo.GetSmartPosition(components.transform.position));
            pathIndex = 0;
        }
    }
}
