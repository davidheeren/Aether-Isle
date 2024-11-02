using Game;
using UnityEngine;

namespace Inventory
{
    public class Sword : Tool
    {
        SwordData data;

        IAimDirection aimDirection;
        IDamageMask damageMask;

        Vector2 dir;

        public Sword(SwordData data, ActorComponents components) : base(data, components)
        {
            this.data = data;

            if (!components.TryGetComponent<IAimDirection>(out aimDirection))
                throw new System.Exception("Item's component should contain IAimDirection");

            if (!components.TryGetComponent<IDamageMask>(out damageMask))
                throw new System.Exception("Item's component should contain IDamageMask");
        }

        public override void Equip()
        {
            base.Equip();

            if (data.equipSFX != null)
            {
                SFXManager.Instance.PlaySFXClip(data.equipSFX, components.transform.position);
            }
        }

        public override void Enter()
        {
            base.Enter();

            dir = aimDirection.AimDirection;

            SFXManager.Instance.PlaySFXClip(data.swingSFX, components.transform.position);

            data.projectile.Spawn(damageMask.DamageMask, data.damageData, components,components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

            components.animator.Play(data.animation);
        }

        public override void Update()
        {
            base.Update();

            float speed = components.stats.GetStat(Stats.StatType.moveSpeed, data.speed);
            components.movement.Move(dir * data.speed);
        }
    }
}
