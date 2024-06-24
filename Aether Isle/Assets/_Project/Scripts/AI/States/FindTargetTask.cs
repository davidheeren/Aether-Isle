using StateTree;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class FindTargetTask : Task
    {
        [Header("Vars")]
        [SerializeField] float updateTime = 0.1f;
        [SerializeField] float detectionRadius = 8;
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask losMask;

        [Header("Debug")]
        [SerializeField] bool drawRadius;
        [SerializeField] bool drawLOS;

        Transform transform;
        Ref<Transform> targetRef;

        Collider2D targetCollider;
        Timer timer;

        // For Debuging
        Vector2 losStart;
        Vector2 losEnd;
        Color losColor;

        private FindTargetTask() : base(null, null) { }
        public FindTargetTask(string copyJson, Transform transform, Ref<Transform> target, Node child = null) : base(copyJson, child)
        {
            this.targetRef = target;
            this.transform = transform;
            timer = new Timer(updateTime);
        }

        public void DrawRadius(Vector2 pos)
        {
            if (!drawRadius) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, detectionRadius);
        }

        protected override void DoTask()
        {
            if (timer.isDone)
            {
                if (targetRef.value == null)
                    GetNewTarget();
                else
                    CheckCurrentTarget();

                timer.Reset();
            }

            DrawLOS();
        }

        void GetNewTarget()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetMask);

            foreach (Collider2D col in cols)
            {
                if (CheckLOS(col))
                {
                    targetRef.value = col.transform;
                    targetCollider = col;
                    break;
                }
            }

            if (cols.Length == 0)
                losColor = new Color(0, 0, 0, 0);
        }

        void CheckCurrentTarget()
        {
            if (!CheckLOS(targetCollider))
            {
                targetRef.value = null;
                targetCollider = null;
            }
        }

        bool CheckLOS(Collider2D col)
        {
            RaycastHit2D losHit = Physics2D.Raycast(transform.position, (col.transform.position - transform.position).normalized, detectionRadius + 0.5f, losMask | targetMask);

            if (losHit.collider == null)
            {
                losColor = new Color(0, 0, 0, 0);
                return false;
            }

            losStart = transform.position;
            losEnd = losHit.point;

            if (losHit.collider != col)
            {
                losColor = Color.red;

                return false;
            }

            losColor = Color.green;
            return true;
        }

        void DrawLOS()
        {
            if (drawLOS)
                Debug.DrawLine(losStart, losEnd, losColor);
        }
    }
} 
