using UnityEngine;
using Game;



namespace Inventory
{
    public class Pickaxe : Tool
    {
        PickaxeData data;

        IAimDirection aimDirection;
        IDamageMask damageMask;

        Vector2 dir;

        public Pickaxe(PickaxeData data, ActorComponents components) : base(data, components)

        {
            this.data = data;
            if (!components.TryGetComponent<IAimDirection>(out aimDirection))
                throw new System.Exception("Items component should contain IAimDirection");

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
            data.projectile.Spawn(damageMask.DamageMask, data.damageData, components, components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            components.animator.Play(data.animation);

            PerformRaycast();
        }

        public override void Update()
        {
            base.Update();

            float speed = components.stats.GetStat(Stats.StatType.moveSpeed, data.speed);
            components.movement.Move(dir * data.speed);
        }

        private void PerformRaycast()
        {
            RaycastHit2D hit = Physics2D.Raycast(components.transform.position, dir, data.range);

            if (hit.collider != null)
            {
                Ore2D MineableOre = hit.collider.GetComponent<Ore2D>();
                if (MineableOre != null)
                {
                    MineableOre.TakeDamage(1);
                    Debug.Log("Tree hit, damage applied");


                }

                else
                {
                    Debug.Log("Hit something not a tree");
                }


            }
            else
            {
                Debug.Log("No object hit");
            }
        }
    }
}
