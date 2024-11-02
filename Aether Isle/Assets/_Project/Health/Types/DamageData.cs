using Game;
using UnityEngine;

namespace DamageSystem
{
    public abstract class DamageData : ScriptableObject
    {
        public int damage = 1;
        public float knockbackSpeed = 15;
        public float stunTime = 0.25f;

        public abstract Damage CreateDamage(ActorComponents components, ActorComponents sourceComponents, Vector2 direction);
    }
}
