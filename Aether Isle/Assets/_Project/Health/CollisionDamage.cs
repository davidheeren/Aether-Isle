using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDamage : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] DamageStats damage;
        [SerializeField] bool useRotation;
        [SerializeField] bool instantiatedByParent;

        Collider2D col;

        public void SetCollider(Collider2D col)
        {
            this.col = col;
        }

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
                health.Damage(damage, col, dir);
            }
        }
    }
}
