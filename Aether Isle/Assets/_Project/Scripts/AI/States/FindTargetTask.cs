using StateTree;
using System;
using System.Data;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class FindTargetTask : Task
    {
        [Header("Vars")]
        [SerializeField] float updateTime = 0.1f;
        [SerializeField] float unAlertDetectionRadius = 8;
        [SerializeField] float alertDetectionRadius = 12;
        [SerializeField] float rememberTargetTime = 2;
        [SerializeField] float rememberAggravateTargetTime = 5;
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask losMask;

        [Header("Debug")]
        [SerializeField] bool drawRadius;
        [SerializeField] bool drawLOS;

<<<<<<< Updated upstream
        Transform transform;
        Ref<Transform> targetRef;

        Collider2D targetCollider;
        Timer timer;
=======
        CharacterComponents components;

        TargetInfo targetInfo;

        Timer lookForTargetTimer;
        Timer rememberTargetTimer;
>>>>>>> Stashed changes

        // For Debugging
        Vector2 losStart;
        Vector2 losEnd;
        Color losColor;

<<<<<<< Updated upstream
        private FindTargetTask() : base(null, null) { }
        public FindTargetTask(string copyJson, Transform transform, Ref<Transform> target, Node child = null) : base(copyJson, child)
        {
            this.targetRef = target;
            this.transform = transform;
            timer = new Timer(updateTime);
=======
        public FindTargetTask Create(CharacterComponents components, TargetInfo targetInfo, Node child = null)
        {
            CreateTask(child);

            this.targetInfo = targetInfo;
            this.components = components;
            lookForTargetTimer = new Timer(updateTime);
            rememberTargetTimer = new Timer(1).ForceDone();

            components.health.OnDamageParams += OnDamage;

            return this;
        }

        private void OnDamage(DamageStats damage, Collider2D col, Vector2? dir)
        {
            // Sets its own target to the new damager
            targetInfo.SetTarget(col, CheckLOS(col, alertDetectionRadius));
            rememberTargetTimer.SetDelay(rememberAggravateTargetTime);

            Debug.Log("Hit aggravated: " + components.gameObject.name);

            // Loops over all aggravateable objects of its own layer
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(components.transform.position, unAlertDetectionRadius, components.gameObject.layer.GetLayerMask());

            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.TryGetComponent<IAggravate>(out IAggravate aggravate))
                {
                    aggravate.Aggravate(col);
                }
            }
>>>>>>> Stashed changes
        }

        public void Aggravate(Collider2D col)
        {
            // Only aggravates if the current target is null
            // If the chase state is locked while the LOS is broken it will aggravate to the new target

            if (targetInfo.isActive) return;

            targetInfo.SetTarget(col, CheckLOS(col, alertDetectionRadius));
            rememberTargetTimer.SetDelay(rememberTargetTime);

            Debug.Log("Alert Aggravated: " + components.gameObject.name);
        }

        protected override void DoTask()
        {
            if (timer.isDone)
            {
                if (!targetInfo.isActive)
                    GetNewTarget();
                else
                    CheckCurrentTarget();

                timer.Reset();
            }

            DrawLOS();
            //Debug.Log("Remember Time: " + rememberTargetTimer.currentDeltaTime);
        }

        void GetNewTarget()
        {
<<<<<<< Updated upstream
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetMask);
=======
            Collider2D[] cols = Physics2D.OverlapCircleAll(components.transform.position, unAlertDetectionRadius, targetMask);
>>>>>>> Stashed changes

            foreach (Collider2D col in cols)
            {
                if (CheckLOS(col, unAlertDetectionRadius))
                {
<<<<<<< Updated upstream
                    targetRef.value = col.transform;
                    targetCollider = col;
=======
                    targetInfo.SetTarget(col, true);
                    rememberTargetTimer.SetDelay(rememberTargetTime);    
>>>>>>> Stashed changes
                    break;
                }
            }

            if (cols.Length == 0)
                losColor = new Color(0, 0, 0, 0);
        }

        void CheckCurrentTarget()
        {
            bool los = CheckLOS(targetInfo.collider, alertDetectionRadius);
            targetInfo.UpdateLOS(los);

            if (los)
                rememberTargetTimer.Reset();

            if (!los && rememberTargetTimer.isDone)
            {
<<<<<<< Updated upstream
                targetRef.value = null;
                targetCollider = null;
=======
                targetInfo.SetTarget(null, false);
>>>>>>> Stashed changes
            }
        }

        bool CheckLOS(Collider2D col, float radius)
        {
<<<<<<< Updated upstream
            RaycastHit2D losHit = Physics2D.Raycast(transform.position, (col.transform.position - transform.position).normalized, detectionRadius + 0.5f, losMask | targetMask);
=======
            RaycastHit2D losHit = Physics2D.Raycast(components.transform.position, (col.transform.position - components.transform.position).normalized, radius + 0.1f, losMask | targetMask);
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
=======
        #region Debug
>>>>>>> Stashed changes
        void DrawLOS()
        {
            if (drawLOS)
                Debug.DrawLine(losStart, losEnd, losColor);
        }

        public void DrawRadius(Vector2 pos)
        {
            if (!drawRadius) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, unAlertDetectionRadius);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, alertDetectionRadius);

            if (targetInfo == null) return;
            if (!targetInfo.isActive || targetInfo.hasLOS) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere((Vector3)targetInfo.SmartPosition, 0.5f);
        }
        #endregion
    }
} 
