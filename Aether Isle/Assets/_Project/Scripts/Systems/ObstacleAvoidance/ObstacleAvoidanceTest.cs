using UnityEngine;

namespace Game
{
    public class ObstacleAvoidanceTest : MonoBehaviour
    {
        [SerializeField] ObstacleAvoidanceData data;
        [SerializeField] Transform target;
        [SerializeField] float speed = 3;

        ObstacleAvoidance obstacleAvoidance;

        private void Awake()
        {
            obstacleAvoidance = new ObstacleAvoidance(data, transform);
        }

        private void Update()
        {
            if (target == null)
                return;

            transform.position += (Vector3)obstacleAvoidance.GetDirectionFromPoint(target.position) * speed * Time.deltaTime;

        }
    }
}
