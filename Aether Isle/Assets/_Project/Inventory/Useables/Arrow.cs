using DamageSystem;
using Game;
using UnityEngine;
using Utilities;

namespace Inventory
{
    public class Arrow : Tool
    {
        ArrowData data;

        IAimDirection aimDirection;
        IDamageMask damageMask;

        AimAssist aimAssist;

        public Arrow(ArrowData data, ActorComponents components) : base(data, components)
        {
            this.data = data;

            if (!components.TryGetComponent<IAimDirection>(out aimDirection))
                throw new System.Exception("Item's component should contain IAimDirection");

            if (!components.TryGetComponent<IDamageMask>(out damageMask))
                throw new System.Exception("Item's component should contain IDamageMask");

            aimAssist = new AimAssist(damageMask.DamageMask, LayerMask.GetMask("Obstacle"), 20); // Could serialize obstacleMask later
        }

        public override void Enter()
        {
            base.Enter();

            Vector2 dir = aimAssist.GetAimAssistDirection(components.transform.position, aimDirection.AimDirection, data.arrowSpeed, 0.9f);

            ProjectileDamage spawn = data.projectile.Spawn(damageMask.DamageMask, data.damageData, components, components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

            if (spawn.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                sr.sprite = data.sprite;
            else Debug.LogWarning("Arrow has no SpriteRenderer");

            if (spawn.TryGetComponent<ArrowProjectile>(out ArrowProjectile arrow))
                arrow.speed = data.arrowSpeed;
            else Debug.LogWarning("Arrow has no ArrowProjectile");
        }
    }
}
