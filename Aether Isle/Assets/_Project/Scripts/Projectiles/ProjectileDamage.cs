using UnityEngine;
using DamageSystem;
using Inventory;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ProjectileDamage : MonoBehaviour
    {

        private Axe axe;
        
        public LayerMask damageMask { get; private set; }

        DamageApplier applier;

        public ProjectileDamage Spawn(LayerMask damageMask, DamageData damageData, ActorComponents source, Vector2 pos, float rot, Transform parent = null)
        {
            ProjectileDamage spawn = Instantiate(this, pos, Quaternion.Euler(0, 0, rot));

            if (parent != null) spawn.transform.parent = parent; // Projectiles do not move properly without a rigidbody when they have a moving parent

            spawn.applier = new DamageApplier(damageMask, damageData, DamageApplier.DamageDirection.Rotation, source, spawn.transform);
            spawn.damageMask = damageMask;

            return spawn;
        }


        public void Initialize(Axe axe)
        {
           this.axe = axe;
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
