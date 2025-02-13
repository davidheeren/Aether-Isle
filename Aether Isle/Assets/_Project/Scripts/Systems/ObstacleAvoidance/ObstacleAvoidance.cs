using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game
{
    //public class ObstacleAvoidance
    //{
    //    ObstacleAvoidanceData data;
    //    Transform transform;

    //    public ObstacleAvoidance(ObstacleAvoidanceData data, Transform transform)
    //    {
    //        this.data = data;
    //        this.transform = transform;
    //    }

    //    public Vector2 GetDirectionFromPoint(Vector2 targetPos)
    //    {
    //        Vector2 dir = targetPos - (Vector2)transform.position;

    //        return GetDirectionFromDirection(dir);
    //    }

    //    /// <summary>
    //    /// Input not have to be normalized, Returns normalized
    //    /// </summary>
    //    /// <param name="dir"></param>
    //    /// <returns></returns>
    //    public Vector2 GetDirectionFromDirection(Vector2 dir)
    //    {
    //        dir.Normalize();

    //        dir += GetAvoidance();

    //        dir.Normalize();

    //        if (data.drawRays)
    //            Debug.DrawRay(transform.position, dir * 1, Color.white);

    //        return dir;
    //    }

    //    public Vector2 GetAvoidance()
    //    {
    //        NativeArray<RaycastHit2D> hits = new NativeArray<RaycastHit2D>(data.rayCount, Allocator.TempJob);

    //        ObstacleAvoidanceJob job = new ObstacleAvoidanceJob
    //        {
    //            position = transform.position,
    //            rayCount = data.rayCount,
    //            rayDist = data.rayDist,
    //            avoidanceMask = data.avoidanceMask,

    //            hits = hits
    //        };

    //        JobHandle handle = job.Schedule(data.rayCount, 1);
    //        handle.Complete();

    //        Vector2 avoidance = Vector2.zero;
    //        float rayCountRecip = 1 / data.rayCount;

    //        foreach (RaycastHit2D hit in hits)
    //        {
    //            if (hit.collider == null)
    //            {
    //                continue;
    //            }

    //            if (data.drawRays)
    //                Debug.DrawLine(transform.position, hit.point, Color.green);

    //            Vector2 hitVector = hit.point - (Vector2)transform.position;
    //            hitVector = Reciprocal(hitVector) * rayCountRecip;

    //            avoidance -= hitVector;
    //        }

    //        hits.Dispose();

    //        avoidance *= data.avoidanceMultiplier;

    //        if (data.drawRays)
    //            Debug.DrawRay(transform.position, avoidance, Color.blue);

    //        return avoidance;
    //    }

    //    private Vector2 Reciprocal(Vector2 vector)
    //    {
    //        float sqrMag = vector.sqrMagnitude;

    //        if (sqrMag == 0)
    //            return Vector3.zero;

    //        float invSqrMag = 1.0f / sqrMag;

    //        return vector * invSqrMag;
    //    }

    //    [BurstCompile]
    //    private struct ObstacleAvoidanceJob : IJobParallelFor
    //    {
    //        // Input
    //        public Vector2 position;
    //        public int rayCount;
    //        public float rayDist;
    //        public LayerMask avoidanceMask;

    //        // Output
    //        public NativeArray<RaycastHit2D> hits;

    //        public void Execute(int index)
    //        {
    //            float angle = index * Mathf.PI * 2 / rayCount;
    //            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

    //            hits[index] = Physics2D.Raycast(position, dir, rayDist, avoidanceMask);
    //        }
    //    }
    //}
    public class ObstacleAvoidance
    {
        ObstacleAvoidanceData data;
        Transform transform;

        public ObstacleAvoidance(ObstacleAvoidanceData data, Transform transform)
        {
            this.data = data;
            this.transform = transform;
        }

        public Vector2 GetDirectionFromPoint(Vector2 targetPos)
        {
            Vector2 dir = targetPos - (Vector2)transform.position;

            return GetDirectionFromDirection(dir);
        }

        /// <summary>
        /// Input not have to be normalized, Returns normalized
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Vector2 GetDirectionFromDirection(Vector2 dir)
        {
            dir.Normalize();

            dir += GetAvoidance() * data.avoidanceMultiplier;

            dir.Normalize();

            if (data.drawRays)
                Debug.DrawRay(transform.position, dir * 1, Color.white);

            return dir;
        }

        public Vector2 GetAvoidance()
        {
            Vector2 pos = transform.position;

            Vector2 avoidance = Vector2.zero;

            float rayCountRecip = 1f / data.rayCount;

            for (int i = 0; i < data.rayCount; i++)
            {
                float angle = i * Mathf.PI * 2 / data.rayCount;

                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                RaycastHit2D hit = Physics2D.Raycast(pos, dir, data.rayDist, data.avoidanceMask);

                if (hit.collider == null)
                {
                    if (data.drawRays)
                        Debug.DrawRay(pos, dir * data.rayDist, Color.red);

                    continue;
                }

                if (data.drawRays)
                    Debug.DrawLine(pos, hit.point, Color.green);

                Vector2 hitVector = hit.point - pos;
                hitVector = Reciprocal(hitVector) * rayCountRecip;

                avoidance -= hitVector;
            }

            if (data.drawRays)
                Debug.DrawRay(pos, avoidance * data.avoidanceMultiplier, Color.blue);

            return avoidance;
        }

        private Vector2 Reciprocal(Vector2 vector)
        {
            float sqrMag = vector.sqrMagnitude;

            if (sqrMag == 0)
                return Vector3.zero;

            float invSqrMag = 1.0f / sqrMag;

            return vector * invSqrMag;
        }
    }
}
