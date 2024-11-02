using CustomInspector;
using Game;
using UnityEngine;

namespace DamageSystem
{
    [CreateAssetMenu(menuName = "Damage/Fire Damage")]
    public class FireDamageData : DamageData
    {
        public int fireTickDamage = 1;
        public float fireTickStunTime = 0.1f;
        public float fireTickDelay = 0.5f;
        public int fireTickCount = 3;

        [SerializeField, ReadOnly] float totalTime = 1.5f;
        [SerializeField, ReadOnly] int totalDamage = 4;

        private void OnValidate()
        {
            totalTime = fireTickCount * fireTickDelay;
            totalDamage = damage + fireTickCount * fireTickDamage;
        }

        public override Damage CreateDamage(ActorComponents components, ActorComponents sourceComponents, Vector2 direction)
        {
            return new FireDamage(this, components, sourceComponents, direction);
        }
    }
}
