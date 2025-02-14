using SpatialPartition;
using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class FindTargetTask : Task
    {
        FindTargetTaskData data;
        ActorComponents components;
        TargetInfo targetInfo;

        Timer lookForTargetTimer;
        Timer rememberTargetTimer;

        // For Debuging
        Vector2 losStart;
        Vector2 losEnd;
        Color losColor;

        public FindTargetTask(FindTargetTaskData data, ActorComponents components, TargetInfo targetInfo, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
            this.targetInfo = targetInfo;

            lookForTargetTimer = new Timer(data.updateTime);
            rememberTargetTimer = new Timer(1).ForceDone();

            components.health.OnDamageParams += OnDamage;
        }

        private void OnDamage(int damage, float stunTime, ActorComponents source)
        {
            if (source == null) return;
            if (source.target == null) return;
            if (!source.target.isActiveAndEnabled) return;

            targetInfo.SetNewTarget(source.target, CheckLOS(source?.col, data.alertDetectionRadius));

            rememberTargetTimer.SetDelay(data.rememberAggravateTargetTime).Reset();

            //Debug.Log("Hit aggravated: " + components.gameObject.name);

            // Loops over all aggravateable objects of its own layer

            foreach (Target t in TargetSpatialManager.Instance.FindEntriesInRadius(components.transform.position, data.unAlertDetectionRadius, AggravateTargetFilter))
            {
                if (t.TryGetComponent<IAggravate>(out IAggravate aggravate))
                {
                    aggravate.Aggravate(source.target);
                }
            }


            //Collider2D[] overlaps = Physics2D.OverlapCircleAll(components.transform.position, data.unAlertDetectionRadius, components.gameObject.layer.GetLayerMask());

            //foreach (Collider2D overlap in overlaps)
            //{
            //    if (overlap.TryGetComponent<IAggravate>(out IAggravate aggravate))
            //    {
            //        aggravate.Aggravate(source.target);
            //    }
            //}
        }

        bool AggravateTargetFilter(Target t)
        {
            return components.gameObject.layer == t.gameObject.layer;
        }

        public void Aggravate(Target target)
        {
            // Only aggravates if the current target is not active
            // If the chase state is locked while the LOS is broken it will aggravate to the new target

            if (targetInfo.isActive) return;

            targetInfo.SetNewTarget(target, CheckLOS(target.col, data.alertDetectionRadius));
            rememberTargetTimer.SetDelay(data.rememberTargetTime).Reset();

            //Debug.Log("Alert Aggravated: " + components.gameObject.name);
        }

        protected override void DoTask()
        {
            if (lookForTargetTimer.IsDone)
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
            //Collider2D[] cols = Physics2D.OverlapCircleAll(components.transform.position, data.unAlertDetectionRadius, data.targetMask);

            //foreach (Collider2D col in cols)
            //{
            //    if (!CheckLOS(col, data.unAlertDetectionRadius)) continue;

            //    if (col.TryGetComponent<Target>(out Target target))
            //    {
            //        if (!target.isActiveAndEnabled) continue;

            //        targetInfo.SetNewTarget(target, true);
            //        rememberTargetTimer.SetDelay(data.rememberTargetTime).Reset();
            //        break;
            //    }
            //}

            Target t = TargetSpatialManager.Instance.FindClosestEntryInRadius(components.transform.position, data.unAlertDetectionRadius, NewTargetFilter);

            if (t == null)
            {
                losColor = new Color(0, 0, 0, 0);
                return;
            }

            targetInfo.SetNewTarget(t, true);
            rememberTargetTimer.SetDelay(data.rememberTargetTime).Reset();
        }

        bool NewTargetFilter(Target target)
        {
            return data.targetMask.Compare(target.gameObject.layer)
                && CheckLOS(target.col, data.unAlertDetectionRadius)
                && target.isActiveAndEnabled;
        }

        void CheckCurrentTarget()
        {
            bool los = CheckLOS(targetInfo.target.col, data.alertDetectionRadius);
            targetInfo.UpdateLOS(los);

            if (los)
                rememberTargetTimer.Reset();

            if (!los && rememberTargetTimer.IsDone)
            {
                targetInfo.DisableTarget();
                //GetNewTarget();
            }
        }

        bool CheckLOS(Collider2D col, float radius)
        {
            RaycastHit2D losHit = Physics2D.Raycast(components.transform.position, (col.transform.position - components.transform.position).normalized, 
                radius + 0.1f, data.losMask | data.targetMask);

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
            if (data.drawLOS)
                Debug.DrawLine(losStart, losEnd, losColor);
        }

        public static void DrawRadius(FindTargetTaskData data, Vector2 pos)
        {
            if (data == null) return;
            if (!data.drawRadius) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, data.unAlertDetectionRadius);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, data.alertDetectionRadius);
        }
    }
}
