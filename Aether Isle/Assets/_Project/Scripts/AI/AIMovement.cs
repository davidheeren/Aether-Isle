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
        [Header("Vars")]
        [SerializeField] float randomOffsetRange = 2;
        [SerializeField] float evaluateDirTime = 0.25f;
        [SerializeField] float nextWaypointDist = 1;

        [SerializeField] ObstacleAvoidance obstacleAvoidance;
        Pathfinder pathfinder;

        Ref<Transform> targetRef = new Ref<Transform>();
        CharacterComponents components;

        Timer targetDirTimer;
        Transform target;

        Vector2[] path;
        int pathIndex;
        Vector2 offset;


        public void Setup(Ref<Transform> targetRef, CharacterComponents components)
        {
            this.targetRef = targetRef;
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
            if (path.Length == 0)
            {
                Debug.Log("Path length is 0");
                return;
            }

            if ((path[pathIndex] - (Vector2)components.transform.position).sqrMagnitude < nextWaypointDist * nextWaypointDist)
            {
                if (pathIndex < path.Length - 1)
                    pathIndex++;
            }

            Vector2 targetPos = pathIndex == path.Length - 1 ? (Vector2)target.position + offset : path[pathIndex];

            if ((targetPos - (Vector2)components.transform.position).sqrMagnitude < 0.01f) // less than 0.1 units
                return;

            components.movement.Move(obstacleAvoidance.GetDir(targetPos) * speed);

            pathfinder.DrawPath();
        }

        void UpdatePath()
        {
            if (targetRef.value != null)
                target = targetRef.value;

            if (target == null)
                return;

            offset = UnityEngine.Random.insideUnitCircle * randomOffsetRange;

            path = pathfinder.FindPath(components.transform.position, target.position);
            pathIndex = 0;
        }
    }
}
