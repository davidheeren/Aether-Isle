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

        CharacterComponents components;

        Ref<Transform> targetRef;
        Collider2D targetCollider;

        Timer lookForTargetTimer;

        // For Debuging
        Vector2 losStart;
        Vector2 losEnd;
        Color losColor;

        public FindTargetTask Create(CharacterComponents components, Ref<Transform> targetRef, Node child = null)
        {
            CreateTask(child);

            this.targetRef = targetRef;
            this.components = components;
            lookForTargetTimer = new Timer(updateTime);

            components.health.OnDamageParams += OnDamage;

            return this;
        }

        private void OnDamage(DamageStats damage, Collider2D col, Vector2? dir)
        {
            SetTarget(col);
        }

        public void DrawRadius(Vector2 pos)
        {
            if (!drawRadius) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, detectionRadius);
        }


        protected override void DoTask()
        {
            if (lookForTargetTimer.isDone)
            {
                if (targetRef.value == null)
                    GetNewTarget();
                else
                    CheckCurrentTarget();

                lookForTargetTimer.Reset();
            }

            DrawLOS();
        }

        void GetNewTarget()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(components.transform.position, detectionRadius, targetMask);

            foreach (Collider2D col in cols)
            {
                if (CheckLOS(col))
                {
                    SetTarget(col);
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
                SetTarget(null);
            }
        }

        bool CheckLOS(Collider2D col)
        {
            RaycastHit2D losHit = Physics2D.Raycast(components.transform.position, (col.transform.position - components.transform.position).normalized, detectionRadius + 0.5f, losMask | targetMask);

            if (losHit.collider == null)
            {
                losColor = new Color(0, 0, 0, 0);
                return false;
            }

            losStart = components.transform.position;
            losEnd = losHit.point;

            if (losHit.collider != col)
            {
                losColor = Color.red;

                return false;
            }

            losColor = Color.green;
            return true;
        }

        void SetTarget(Collider2D col)
        {
            targetRef.value = col == null ? null : col.transform;
            targetCollider = col;
        }

        void DrawLOS()
        {
            if (drawLOS)
                Debug.DrawLine(losStart, losEnd, losColor);
        }
    }
} 
