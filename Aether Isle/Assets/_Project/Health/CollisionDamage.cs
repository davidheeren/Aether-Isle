using CustomInspector;
using DamageSystem;
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
        [SerializeField] DamageData damageData;
        [TooltipBox("This will use the rotation of this instead of the dir of the collision")]
        [SerializeField] DamageDirection damageDirection;

        [NonSerialized] public ActorComponents source; // Will be its own collider if not a projectile

        private void Awake()
        {
            source = GetComponent<ActorComponents>();

            if (damageData == null)
                Debug.LogError("Damage Data is null", gameObject);
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
                Vector2 dir = GetDirection(collision);
                
                health.AddDamage(damageData, source, dir);
            }
        }

        Vector2 GetDirection(Collider2D collision)
        {
            return damageDirection switch
            {
                DamageDirection.Rotation => transform.up,
                DamageDirection.DirectionOfImpact => (collision.transform.position - transform.position).normalized,
                _ => Vector2.zero
            };
        }

        public enum DamageDirection
        {
            Rotation,
            DirectionOfImpact
        }
    }
}
