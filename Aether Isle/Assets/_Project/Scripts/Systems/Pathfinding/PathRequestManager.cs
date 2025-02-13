using System;
using System.Collections.Concurrent;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(PathGrid))]
    public class PathRequestManager : MonoBehaviour
    {
        private static PathRequestManager _instance;
        public static PathRequestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject p = PathGrid.Instance.gameObject;

                    if (!p.TryGetComponent<PathRequestManager>(out _instance))
                        _instance = p.AddComponent<PathRequestManager>();
                }

                return _instance;
            }
        }

        Pathfinder pathfinder;

        ConcurrentQueue<PathRequest> requests = new ConcurrentQueue<PathRequest>();
        ConcurrentQueue<PathResponse> responses = new ConcurrentQueue<PathResponse>();

        volatile bool isRunning = true; // Ensures thread exits when stopping
        // The volatile keyword in C# tells the compiler and CPU not to optimize access to a variable, ensuring that all threads always see its most recent value

        private void Awake()
        {
            if (!TryGetComponent<PathGrid>(out PathGrid pathGrid))
            {
                Debug.LogError("Path Request Manager needs a Path Grid Component");
                return;
            }

            pathfinder = new Pathfinder(pathGrid);

            ThreadPool.QueueUserWorkItem(ProcessPathRequests);
        }

        private void OnDestroy()
        {
            isRunning = false;
        }

        private void Update()
        {
            // Process responses on the main thread
            while (responses.TryDequeue(out PathResponse response))
            {
                response.callback.Invoke(response.path);
            }
        }

        public void FindPath(Vector2 start, Vector2 end, Action<Vector2[]> callback)
        {
            if (callback == null)
            {
                Debug.LogError("Callback cannot be null");
                return;
            }

            requests.Enqueue(new PathRequest(start, end, callback));
        }

        public void ProcessPathRequests(object state)
        {
            while(isRunning)
            {
                if (requests.Count > 0 && requests.TryDequeue(out PathRequest request))
                {
                    //Thread.Sleep(1000); //for testing main thread performance with a slow algorithm on a separate thread

                    Vector2[] path = pathfinder.FindPath(request.start, request.end);
                    responses.Enqueue(new PathResponse(path, request.callback));
                }
                else
                {
                    Thread.Sleep(10); // Prevents excessive CPU usage when no requests exist
                }
            }
        }

        private struct PathRequest
        {
            public Vector2 start;
            public Vector2 end;
            public Action<Vector2[]> callback;

            public PathRequest(Vector2 start, Vector2 end, Action<Vector2[]> callback)
            {
                this.start = start;
                this.end = end;
                this.callback = callback;
            }
        }

        private struct PathResponse
        {
            public Vector2[] path;
            public Action<Vector2[]> callback;

            public PathResponse(Vector2[] path, Action<Vector2[]> callback)
            {
                this.path = path;
                this.callback = callback;
            }
        }
    }
}
