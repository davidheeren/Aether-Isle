using UnityEngine;
using DamageSystem;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ProjectileDamage : MonoBehaviour
    {
        public LayerMask damageMask { get; private set; }

        DamageApplier applier;

        public void Spawn(LayerMask damageMask, DamageData damageData, ActorComponents source, Vector2 pos, float rot, Transform parent = null)
        {
            ProjectileDamage spawn = Instantiate(this, pos, Quaternion.Euler(0, 0, rot));

            if (parent != null) spawn.transform.parent = parent; // Projectiles do not move properly without a rigidbody when they have a moving parent

            spawn.applier = new DamageApplier(damageMask, damageData, DamageApplier.DamageDirection.Rotation, source, transform);
            spawn.damageMask = damageMask;

            // Not sure why we need the tag
            //if (!spawn.CompareTag("Projectile")) 
            //    Debug.LogWarning("Projectiles tag is not Projectile");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            applier.Damage(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            applier.Damage(collision.collider);
        }
    }
}
