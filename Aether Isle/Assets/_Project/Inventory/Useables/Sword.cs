using Game;
using UnityEngine;

namespace Inventory
{
    public class Sword : Tool
    {
        SwordData data;

        IAimDirection aimDirection;
        IDamageMask damageMask;

        WearableComponents wearableComponents;

        Vector2 dir;

        public Sword(SwordData data, ActorComponents components) : base(data, components)
        {
            this.data = data;

            if (!components.TryGetComponent<IAimDirection>(out aimDirection))
                throw new System.Exception("Item's component should contain IAimDirection");

            if (!components.TryGetComponent<IDamageMask>(out damageMask))
                throw new System.Exception("Item's component should contain IDamageMask");

            wearableComponents = ComponentUtilities.GetRequiredComponentInChildren<WearableComponents>(components.gameObject);
        }

        public override void Equip()
        {
            base.Equip();

            wearableComponents.useableRenderer.sprite = data.sprite;
            wearableComponents.useableRenderer.gameObject.SetActive(true);

            if (data.equipSFX != null)
            {
                SFXManager.Instance.PlaySFXClip(data.equipSFX, components.transform.position);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();

            wearableComponents.useableRenderer.gameObject.SetActive(false);
        }

        public override void Enter()
        {
            base.Enter();

            dir = aimDirection.AimDirection;

            SFXManager.Instance.PlaySFXClip(data.swingSFX, components.transform.position);

            data.projectile.Spawn(damageMask.DamageMask, data.damageData, components,components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, components.transform);

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
