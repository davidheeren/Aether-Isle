using CustomInspector;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDamage : MonoBehaviour
    {
        [TooltipBox("This will be overwritten by a DamageProjectile")]
        public LayerMask damageMask;
        [SerializeField] DamageStats damage;
        [TooltipBox("This will use the rotation of this instead of the dir of the collision")]
        [SerializeField] bool useRotation;

        Collider2D col;
        [NonSerialized] public Collider2D source; // Will be its own collider if not a projectile

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            source = col;
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
            if (collision == source) return; // Avoid projectiles damaging source

            if (damageMask.Compare(collision.gameObject.layer) && collision.TryGetComponent<Health>(out Health health))
            {
                Vector2 dir = useRotation ? transform.up : (collision.transform.position - transform.position).normalized;
                health.Damage(damage, col, source, dir);
            }
        }
    }
}
