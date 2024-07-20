using UnityEngine;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [SerializeField] Collider2D col;

        private void Update()
        {
            Vector3 bottomLeft = col.bounds.center - col.bounds.extents;
            Vector3 topRight = col.bounds.center + col.bounds.extents;

            Debug.DrawLine(bottomLeft, topRight);
        }
    }
}
