using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CollisionDamage))]
    public class DamageProjectile : MonoBehaviour
    {
        CollisionDamage collisionDamage;

        void Awake()
        {
            collisionDamage = GetComponent<CollisionDamage>();
        }

        public void Spawn(Collider2D source, LayerMask damageMask, Vector2 pos, float rot, Transform parent = null)
        {
            //DamageProjectile spawn = Instantiate(this, pos, Quaternion.Euler(0, 0, rot), source.transform);
            DamageProjectile spawn = Instantiate(this, pos, Quaternion.Euler(0, 0, rot));

            if (parent != null) spawn.transform.parent = parent;

            spawn.collisionDamage.source = source;
            spawn.collisionDamage.damageMask = damageMask;
            //spawn.gameObject.layer = source.gameObject.layer;

            if (!spawn.CompareTag("Projectile"))
                Debug.LogWarning("Projectiles tag is not Projectile");
        }
    }
}
