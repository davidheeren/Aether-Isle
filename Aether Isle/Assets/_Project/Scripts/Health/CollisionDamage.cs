using UnityEngine;
using Utilities;

namespace Game
{
    public class CollisionDamage : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] DamageStats damage;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (layerMask.Compare(collision.gameObject.layer) && collision.TryGetComponent<Health>(out Health health))
            {
                health.Damage(damage, transform.right);
            }
        }
    }
}
