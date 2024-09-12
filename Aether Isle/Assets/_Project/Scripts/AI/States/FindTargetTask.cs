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
        [SerializeField] float unAlertDetectionRadius = 8;
        [SerializeField] float alertDetectionRadius = 12;
        [SerializeField] float rememberTargetTime = 2;
        [SerializeField] float rememberAggravateTargetTime = 5;
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask losMask;

        [Header("Debug")]
        [SerializeField] bool drawRadius;
        [SerializeField] bool drawLOS;

        CharacterComponents components;

        TargetInfo targetInfo;

        Timer lookForTargetTimer;
        Timer rememberTargetTimer;

        // For Debuging
        Vector2 losStart;
        Vector2 losEnd;
        Color losColor;

        private FindTargetTask() : base(null) { }
        public FindTargetTask Init(CharacterComponents components, TargetInfo targetInfo, Node child = null)
        {
            InitializeTask(child);

            this.targetInfo = targetInfo;
            this.components = components;
            lookForTargetTimer = new Timer(updateTime);
            rememberTargetTimer = new Timer(1).ForceDone();

            components.health.OnDamageParams += OnDamage;

            return this;
        }

        private void OnDamage(DamageStats damage, Collider2D col, Collider2D source, Vector2? dir)
        {
            // Sets its own target to the new damage
            targetInfo.SetTarget(source, CheckLOS(source, alertDetectionRadius));
            rememberTargetTimer.SetDelay(rememberAggravateTargetTime).Reset();

            //Debug.Log("Hit aggravated: " + components.gameObject.name);

            // Loops over all aggravateable objects of its own layer
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(components.transform.position, unAlertDetectionRadius, components.gameObject.layer.GetLayerMask());

            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.TryGetComponent<IAggravate>(out IAggravate aggravate))
                {
                    aggravate.Aggravate(source);
                }
            }
        }

        public void Aggravate(Collider2D col)
        {
            // Only aggravates if the current target is null
            // If the chase state is locked while the LOS is broken it will aggravate to the new target

            if (targetInfo.isActive) return;

            targetInfo.SetTarget(col, CheckLOS(col, alertDetectionRadius));
            rememberTargetTimer.SetDelay(rememberTargetTime).Reset();

            Debug.Log("Alert Aggravated: " + components.gameObject.name);
        }

        protected override void DoTask()
        {
            if (lookForTargetTimer.isDone)
            {
                if (!targetInfo.isActive)
                    GetNewTarget();
                else
                    CheckCurrentTarget();

                lookForTargetTimer.Reset();
            }

            //Debug.Log("Target: " + targetInfo.gameObject?.name + " Time left: " + rememberTargetTimer?.currentDeltaTime);

            DrawLOS();
        }

        void GetNewTarget()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(components.transform.position, unAlertDetectionRadius, targetMask);

            foreach (Collider2D col in cols)
            {
                if (CheckLOS(col, unAlertDetectionRadius))
                {
                    targetInfo.SetTarget(col, true);
                    rememberTargetTimer.SetDelay(rememberTargetTime).Reset();    
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
                targetInfo.SetTarget(null, false);
            }
        }

        bool CheckLOS(Collider2D col, float radius)
        {
            RaycastHit2D losHit = Physics2D.Raycast(components.transform.position, (col.transform.position - components.transform.position).normalized, radius + 0.1f, losMask | targetMask);

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
        }
    }
} 
