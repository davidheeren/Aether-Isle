using UnityEngine;

namespace Pathfinding
{
    public class PathfindingTestLegacy1 : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float speed = 1;
        [SerializeField] int testCount = 100;
        [SerializeField] bool drawPath = true;

        PathfinderLegacy1 pathfinder;

        Vector2[] path = new Vector2[0];

        int targetIndex = 0;

        private void Awake()
        {
            pathfinder = new PathfinderLegacy1(PathGridLegacy1.Instance);
        }

        private void Update()
        {
            TestUpdatePath();

            if (path == null)
                return;

            Draw();

            if (path.Length == 0)
                return;

            Vector3 distTo = (Vector3)path[targetIndex] - transform.position;

            if (distTo.sqrMagnitude < 0.25f)
            {
                if (targetIndex == path.Length - 1)
                    return;
                else
                    targetIndex++;
            }

            transform.position += distTo.normalized * speed * Time.deltaTime;
        }

        private void Draw()
        {
            if (!drawPath)
                return;

            for (int i = 1; i < path.Length; i++)
            {
                Debug.DrawLine(path[i - 1], path[i], Color.blue);
            }
        }

        void UpdatePath()
        {
            targetIndex = 0;
            path = pathfinder.FindPath(transform.position, target.position);
        }

        void TestUpdatePath()
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < testCount; i++)
            {
                UpdatePath();
            }

            Debug.Log("Path found in " + sw.ElapsedMilliseconds + " ms");
        }
    }
}
