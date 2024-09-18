using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Data/ObstacleAvoidanceData")]
    public class ObstacleAvoidanceData : ScriptableObject
    {
        public int rayCount = 20;
        public float rayDist = 2;
        public float avoidanceMultiplier = 5;
        public LayerMask avoidanceMask;

        [Header("Debug")]
        public bool drayRays;
    }
}
