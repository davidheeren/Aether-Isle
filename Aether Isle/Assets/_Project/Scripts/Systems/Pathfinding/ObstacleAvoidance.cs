using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class ObstacleAvoidance
    {
        [SerializeField] int rayCount = 20;
        [SerializeField] float rayDist = 2;
        [SerializeField] float avoidanceMultiplier = 5;
        [SerializeField] LayerMask avoidanceMask;

        [Header("Debug")]
        [SerializeField] bool drayRays;

        Transform transform;

        public void Setup(Transform transform)
        {
            this.transform = transform;
        }

        public Vector2 GetDir(Vector2 targetPos)
        {
            Vector2 dir = targetPos - (Vector2)transform.position;
            dir.Normalize();

            dir += Avoidance() * avoidanceMultiplier;

            dir.Normalize();

            if (drayRays)
            {
                Debug.DrawRay(transform.position, dir * 1, Color.white);
            }

            return dir;
        }

        Vector2 Avoidance()
        {
            Vector2 pos = transform.position;

            Vector2 avoidance = Vector2.zero;

            float rayCountRecip = 1f / rayCount;

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
                hitVector = hitVector.Reciprocal() * rayCountRecip;

                avoidance -= hitVector;
            }

            if (drayRays)
                Debug.DrawRay(pos, avoidance * avoidanceMultiplier, Color.blue);

            return avoidance;
        }
    }
}
