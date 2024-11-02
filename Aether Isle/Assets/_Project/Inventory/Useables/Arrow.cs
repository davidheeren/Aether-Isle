using DamageSystem;
using Game;
using UnityEngine;

namespace Inventory
{
    public class Arrow : Tool
    {
        ArrowData data;

        IAimDirection aimDirection;
        IDamageMask damageMask;

        public Arrow(ArrowData data, ActorComponents components) : base(data, components)
        {
            this.data = data;

            if (!components.TryGetComponent<IAimDirection>(out aimDirection))
                throw new System.Exception("Item's component should contain IAimDirection");

            if (!components.TryGetComponent<IDamageMask>(out damageMask))
                throw new System.Exception("Item's component should contain IDamageMask");
        }

        public override void Enter()
        {
            base.Enter();

            Vector2 dir = aimDirection.AimDirection;
            ProjectileDamage spawn = data.projectile.Spawn(damageMask.DamageMask, data.damageData, components, components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

            if (spawn.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            {
                sr.sprite = data.sprite;
            }

            if (spawn.TryGetComponent<ArrowProjectile>(out ArrowProjectile arrow))
            {
                arrow.speed = data.arrowSpeed;
            }
        }
    }
}
