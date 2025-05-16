using Game;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingTest : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float speed = 1;
        //[SerializeField] int testCount = 100;
        [SerializeField] bool drawPath = true;

        //Pathfinder pathfinder;
        PathRequestManager pathRequestManager;

        WaypointHelper waypointHelper = new WaypointHelper(new Vector2[0]);

        //int targetIndex = 0;

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private void Start()
        {
            //pathfinder = new Pathfinder(PathGrid.Instance);
            pathRequestManager = PathRequestManager.Instance;

            pathRequestManager.FindPath(transform.position, target.position, OnPathReceived);
            sw.Start();
        }

        private void Update()
        {
            Vector2? waypoint = waypointHelper.GetCurrentWaypoint(transform.position);

            if (waypoint == null)
                return;

            if (drawPath)
                waypointHelper.DrawWaypoints();

            Vector3 dir = ((Vector3)waypoint - transform.position).normalized;

            transform.position += dir * speed * Time.deltaTime;
        }

        void OnPathReceived(Vector2[] path)
        {
            waypointHelper.SetWaypoints(path);

            print($"Callback path received in: {sw.ElapsedMilliseconds} ms");
            sw.Restart();

            pathRequestManager.FindPath(transform.position, target.position, OnPathReceived);
        }

        void UpdatePath()
        {
            //path = pathfinder.FindPath(transform.position, target.position);
        }

        //void UpdatePathLoop()
        //{
        //    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

        //    for (int i = 0; i < testCount; i++)
        //    {
        //        UpdatePath();
        //    }

        //    Debug.Log("Legacy Path found in " + sw.ElapsedMilliseconds + " ms");
        //}
    }
}
