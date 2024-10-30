using Game;
using UnityEngine;

namespace Inventory
{
    public class Sword : Tool
    {
        SwordData data;

        Vector2 dir;

        public Sword(SwordData data, ActorComponents components) : base(data, components)
        {
            this.data = data;
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

            dir = components.aimDirection.AimDirection;

            SFXManager.Instance.PlaySFXClip(data.swingSFX, components.transform.position);

            //data.projectile.Spawn(components, components.damageMask.DamageMask, components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            data.projectile.Spawn(components.damageMask.DamageMask, data.damageData, components,components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

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
