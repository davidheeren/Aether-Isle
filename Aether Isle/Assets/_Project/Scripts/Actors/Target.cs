using SpatialPartition;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class Target : MonoBehaviour, ISpatialGridEntry
    {
        Vector2 lastKnownPosition;
        public Vector2 Position => positionEnabled ? transform.position : lastKnownPosition;
        public int Layer => Layer;

        bool positionEnabled = true;

        public Collider2D col { get; private set; }
        public bool isPlayer { get; private set; }
        public bool isAlive { get; private set; } = true;

        Health health;

        [ContextMenu("EnablePos")]
        public void EnablePosition() => positionEnabled = true;
        [ContextMenu("DisablePos")]
        public void DisablePosition()
        {
            positionEnabled = false;
            lastKnownPosition = transform.position;
        }

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            isPlayer = CompareTag("Player");

            if (TryGetComponent<Health>(out health))
            {
                health.OnDie += OnDie;
            }
        }

        void OnDie()
        {
            isAlive = false;
        }
    }
}
