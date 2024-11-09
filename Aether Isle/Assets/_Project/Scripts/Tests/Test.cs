using UnityEngine;
using Utilities;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [SerializeField] LayerMask damageMask;
        AimAssist aimAssist;
        IAimDirection aimDirection;

        private void Awake()
        {
            aimAssist = new AimAssist(damageMask, 20, 1);
            aimDirection = GetComponent<IAimDirection>();
        }

        private void OnDrawGizmos()
        {
            if (aimAssist == null) return;

            Collider2D col = aimAssist.GetClosestCollider(transform.position, aimDirection.AimDirection);

            if (col == null) return;

            Gizmos.DrawWireCube(col.transform.position, Vector3.one * 0.5f);
        }
    }
}
