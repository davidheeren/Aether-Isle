using UnityEngine;
using Utilities;

namespace Game
{
    public class CollisionDamage : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] DamageStats damage;
        [SerializeField] bool useRotation;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            Damage(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Damage(collision.collider);
        }

        void Damage(Collider2D collision)
        {
            if (layerMask.Compare(collision.gameObject.layer) && collision.TryGetComponent<Health>(out Health health))
            {
                Vector2 dir = useRotation ? transform.up : (collision.transform.position - transform.position).normalized;
                health.Damage(damage, dir);
            }
        }
    }
}
