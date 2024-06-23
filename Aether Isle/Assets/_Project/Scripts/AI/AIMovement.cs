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

        [SerializeField] int rayCount = 25;
        [SerializeField] float rayDist = 2;
        [SerializeField] float avoidanceMultiplier = 10;
        [SerializeField] LayerMask avoidanceMask;

        [Header("Debug")]
        [SerializeField] bool drayRays;
        [SerializeField] bool useFast;

        Vector2 targetPos;

        Ref<Transform> targetRef = new Ref<Transform>();
        Transform transform;
        Movement movement;

        Timer timer;
        Transform target;

        public void Setup(Ref<Transform> targetRef, Transform transform, Movement movement)
        {
            this.targetRef = targetRef;
            this.transform = transform;
            this.movement = movement;

            timer = new Timer(evaluateDirTime);
        }

        public void Enter()
        {
            UpdateTargetPos();
            timer.Reset();
        }

        public void Update(float speed)
        {
            if (timer.isDone)
            {
                UpdateTargetPos();
                timer.Reset();
            }

            Vector2 dir = targetPos - (Vector2)transform.position;

            if (dir.sqrMagnitude < 0.01f) // less that 0.1 units
                return;

            dir = dir.normalized + Avoidance() * avoidanceMultiplier;
            dir.Normalize();
            movement.Move(dir * speed);
        }

        void UpdateTargetPos()
        {
            if (targetRef.value != null)
                target = targetRef.value;

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
            Vector2 pos = transform.position;

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
        }
    }
}
