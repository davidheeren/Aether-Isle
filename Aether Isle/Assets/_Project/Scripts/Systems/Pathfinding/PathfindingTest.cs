using UnityEngine;

namespace Pathfinding
{
    public class PathfindingTest : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float speed = 1;
        [SerializeField] float repeat = 0.5f;
        [SerializeField] bool drawPath = true;

        Pathfinder pathfinder;

        Vector2[] path = new Vector2[0];

        int targetIndex = 0;

        private void Awake()
        {
            pathfinder = new Pathfinder(PathGrid.Instance);

            InvokeRepeating(nameof(UpdatePath), 0, repeat);
        }

        private void Update()
        {
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
    }
}
