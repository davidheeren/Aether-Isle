using Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class AimAssist
    {
        readonly LayerMask aimMask;
        readonly LayerMask obstacleMask;
        readonly float radius;
        readonly float threshold;
        readonly float angleFac;

        public AimAssist(LayerMask aimMask, LayerMask obstacleMask, float radius, float threshold = 0.35f, float angleFac = 0.8f)
        {
            this.aimMask = aimMask;
            this.obstacleMask = obstacleMask;
            this.radius = radius;
            this.threshold = threshold;
            this.angleFac = Mathf.Clamp01(angleFac);
        }

        public Collider2D GetClosestCollider(Vector2 position, Vector2 aimDirection)
        {
            IEnumerable<Collider2D> colliders = Physics2D.OverlapCircleAll(position, radius, aimMask) // Get all colliders
                .Where(x => x.TryGetComponent<Target>(out Target t) && t.isAlive) // Filter based on Target
                .Where(x => !Raycast(x)); // Filter based on LOS

            if (!colliders.Any()) return null;

            Collider2D bestCol = colliders.Aggregate((best, current) =>
                DA(current) > DA(best) ? current : best); // Get highest DistanceAngleFactor

            return DA(bestCol) > threshold ? bestCol : null; // Filter based on DA threshold

            float DA(Collider2D col) => Maths.DistanceAngleFactor(position, aimDirection, col.transform.position, radius, angleFac);

            bool Raycast(Collider2D col)
            {
                Vector2 distTo = (Vector2)col.transform.position - position;
                return Physics2D.Raycast(position, distTo.normalized, distTo.magnitude, obstacleMask);
            }
        }

        /// <summary>
        /// Lerps between aim direction and predicted target position angle by assistAmount
        /// </summary>
        /// <returns></returns>
        public Vector2 GetAimAssistDirection(Vector2 position, Vector2 aimDirection, float projectileSpeed, float assistAmount01)
        {
            assistAmount01 = Mathf.Clamp01(assistAmount01);
            if (assistAmount01 == 0) 
                return aimDirection;

            Collider2D closestCollider = GetClosestCollider(position, aimDirection);

            if (closestCollider == null)
                return aimDirection;

            Vector2 targetPos =
                closestCollider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)
                ? Maths.TargetPrediction(closestCollider.transform.position, rb.linearVelocity, position, projectileSpeed)
                : closestCollider.transform.position;

            Vector2 targetDir = targetPos - position;
            float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            float assistAngle = Mathf.LerpAngle(aimAngle, targetAngle, assistAmount01);

            return new Vector2(Mathf.Cos(assistAngle * Mathf.Deg2Rad), Mathf.Sin(assistAngle * Mathf.Deg2Rad));
        }
    }
}
