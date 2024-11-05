using CustomInspector;
using DamageSystem;
using Game;
using SpriteAnimator;
using UnityEngine;
using UnityEngine.Search;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Sword")]
    public class SwordData : ToolData
    {
        public DamageData damageData;
        [SearchContext("p: t:prefab")]
        public ProjectileDamage projectile;
        public float speed = 1;
        public SpriteAnimation animation;
        public AudioClip swingSFX;
        public AudioClip equipSFX;

        public override Useable CreateUseable(ActorComponents components)
        {
            return new Sword(this, components);
        }
    }
}
