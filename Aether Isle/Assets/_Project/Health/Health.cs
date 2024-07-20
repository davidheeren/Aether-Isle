using System;
using UnityEngine;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 3;
        public int currentHealth { get; private set; }

        [NonSerialized] public bool canTakeDamage = true;
        public bool isDead { get; private set; } = false;

        public Action<DamageStats, Collider2D, Vector2?> OnDamageParams;
        public Action OnDamage;

        public Action OnDie;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void Damage(DamageStats damage, Collider2D col, Vector2? dir = null)
        {
            if (!canTakeDamage || isDead)
                return;

            currentHealth -= damage.damage;

            OnDamageParams?.Invoke(damage, col, dir);
            OnDamage?.Invoke();

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                OnDie?.Invoke();
            }
        }
    }
}
