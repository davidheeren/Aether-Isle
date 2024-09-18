using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class Target : MonoBehaviour
    {
        Vector2 lastKnownPosition;
        public Vector2 position => positionEnabled ? transform.position : lastKnownPosition;

        bool positionEnabled = true;

        public Collider2D col { get; private set; }
        public bool isPlayer { get; private set; }

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
        }
    }
}
