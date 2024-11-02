using DamageSystem;
using UnityEngine;
using Utilities;

namespace Game
{
    public class DamageApplier
    {
        readonly LayerMask damageMask;
        readonly DamageData damageData;
        readonly DamageDirection damageDirection;
        readonly ActorComponents source;
        readonly Transform transform;

        public DamageApplier(LayerMask damageMask, DamageData damageData, DamageDirection damageDirection, ActorComponents source, Transform transform)
        {
            this.damageMask = damageMask;
            this.damageData = damageData;
            this.damageDirection = damageDirection;
            this.source = source;
            this.transform = transform;
        }

        public void Damage(Collider2D collision)
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
