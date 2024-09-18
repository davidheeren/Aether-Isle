using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Data/FindTargetTaskData")]
    public class FindTargetTaskData : ScriptableObject
    {
        [Header("Vars")]
        public float updateTime = 0.1f;
        public float unAlertDetectionRadius = 8;
        public float alertDetectionRadius = 12;
        public float rememberTargetTime = 2;
        public float rememberAggravateTargetTime = 5;
        public LayerMask targetMask;
        public LayerMask losMask;

        [Header("Debug")]
        public bool drawRadius;
        public bool drawLOS;
    }
}
