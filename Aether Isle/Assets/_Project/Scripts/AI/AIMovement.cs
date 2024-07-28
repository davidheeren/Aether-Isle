using CustomInspector;
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

<<<<<<< Updated upstream
        [SerializeField] int rayCount = 25;
        [SerializeField] float rayDist = 2;
        [SerializeField] float avoidanceMultiplier = 10;
        [SerializeField] LayerMask avoidanceMask;

        [Header("Debug")]
        [SerializeField] bool drayRays;
        [SerializeField] bool useFast;

        Vector2 targetPos;

        Ref<Transform> targetRef = new Ref<Transform>();

=======
        ObstacleAvoidance obstacleAvoidance;
        Pathfinder pathfinder;

>>>>>>> Stashed changes
        CharacterComponents components;
        TargetInfo targetInfo;

<<<<<<< Updated upstream
        Timer timer;
        Transform target;

        public void Setup(Ref<Transform> targetRef, CharacterComponents componenets)
        {
            this.targetRef = targetRef;
            this.components = componenets;

            timer = new Timer(evaluateDirTime);
=======
        Timer evaluatePathTimer;

        Vector2[] path;
        int pathIndex;
        Vector2 offset;


        public void Setup(TargetInfo targetInfo, ObstacleAvoidance obstacleAvoidance, CharacterComponents components)
        {
            this.targetInfo = targetInfo;
            this.obstacleAvoidance = obstacleAvoidance;
            this.components = components;

            evaluatePathTimer = new Timer(evaluateDirTime);

            pathfinder = new Pathfinder(PathGrid.Instance);
>>>>>>> Stashed changes
        }

        public void Enter()
        {
<<<<<<< Updated upstream
            UpdateTargetPos();
            timer.Reset();
=======
            evaluatePathTimer?.ForceDone();
>>>>>>> Stashed changes
        }

        /// <summary>
        /// Updates the AI movement by checking if the target direction timer is done, updating the path, and moving towards the target position.
        /// </summary>
        /// <param name="speed">The speed at which the AI should move</param>
        public void Update(float speed)
        {
<<<<<<< Updated upstream
            if (timer.isDone)
            {
                UpdateTargetPos();
                timer.Reset();
            }

            Vector2 dir = targetPos - (Vector2)components.transform.position;

            if (dir.sqrMagnitude < 0.01f) // less that 0.1 units
                return;

            dir = dir.normalized + Avoidance() * avoidanceMultiplier;
            dir.Normalize();
            components.movement.Move(dir * speed);
=======
            if (!targetInfo.isActive)
                Debug.LogError("Target is not active");

            // If the target direction timer is done, update the path and reset the timer
            if (evaluatePathTimer.isDone)
            {
                UpdatePath();
                evaluatePathTimer.Reset();
            }

            // Return if the path is null
            if (path == null)
                return;

            // Check if the path is not empty
            if (path.Length != 0)
            {
                // Check if the AI is close to the next waypoint in the path
                if ((path[pathIndex] - (Vector2)components.transform.position).sqrMagnitude < nextWaypointDist * nextWaypointDist)
                {
                    // Move to the next waypoint if available
                    if (pathIndex < path.Length - 1)
                        pathIndex++;
                }
            }

            // Determine the target position for the AI to move towards
            Vector2 targetPos = pathIndex == path.Length - 1 || path.Length == 0 ? targetInfo.SmartPosition.Value + offset : path[pathIndex];

            // Return if the AI is already close to the target position
            if ((targetPos - (Vector2)components.transform.position).sqrMagnitude < 0.2f * 0.2f) // less than 0.2 units
                return;

            // Move the AI towards the target position using obstacle avoidance and the specified speed
            components.movement.Move(obstacleAvoidance.GetDir(targetPos) * speed);

            // Draw the current path for debugging purposes
            pathfinder.DrawPath();
>>>>>>> Stashed changes
        }

        void UpdateTargetPos()
        {
            offset = Vector2.zero;
            if (targetInfo.hasLOS)
                offset = UnityEngine.Random.insideUnitCircle * randomOffsetRange;

<<<<<<< Updated upstream
            if (target == null)
                return;

            /*
            // If there target is less then the range of the random offset, then there is no offset
            Vector2 offset = Vector2.zero;
            if (((Vector2)transform.position - (Vector2)target.position).sqrMagnitude > randomOffsetRange * randomOffsetRange)
                offset = UnityEngine.Random.insideUnitCircle * randomOffsetRange;
            /*/

            // I like that the enemies don't run straight toward you when they get close. It throws the player off

            Vector2 offset = UnityEngine.Random.insideUnitCircle * randomOffsetRange;
            targetPos = (Vector2)target.position + offset;
        }

        Vector2 Avoidance()
        {
            Vector2 pos = components.transform.position;

            Vector2 avoidance = Vector2.zero;

            for (int i = 0; i < rayCount; i++)
            {
                float angle = i * Mathf.PI * 2 / rayCount;

                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                RaycastHit2D hit = Physics2D.Raycast(pos, dir, rayDist, avoidanceMask);

                if (hit.collider == null)
                {
                    if (drayRays)
                        Debug.DrawRay(pos, dir * rayDist, Color.red);

                    continue;
                }

                if (drayRays)
                    Debug.DrawLine(pos, hit.point, Color.green);

                Vector2 hitVector = hit.point - pos;
                hitVector = !useFast ? hitVector.normalized / hitVector.magnitude / rayCount : hitVector.Reciprocal() / rayCount;

                avoidance -= hitVector;
            }

            if (drayRays)
                Debug.DrawRay(pos, avoidance * avoidanceMultiplier, Color.blue);

            return avoidance;
=======
            path = pathfinder.FindPath(components.transform.position, targetInfo.SmartPosition.Value);
            pathIndex = 0;
>>>>>>> Stashed changes
        }
    }
}
