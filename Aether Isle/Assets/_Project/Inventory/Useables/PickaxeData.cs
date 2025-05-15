using UnityEngine;
using DamageSystem;
using SpriteAnimator;
using Game;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Pickaxe")]
    public class PickaxeData : ToolData
    {
        public DamageData damageData;
        public ProjectileDamage projectile;
        public float speed = 1;
        public SpriteAnimation animation;
        public AudioClip swingSFX;
        public AudioClip equipSFX;
        public float range = 1.5f;

        public override Useable CreateUseable(ActorComponents components)
        {
            return new Pickaxe(this, components);
        }
    }
}

