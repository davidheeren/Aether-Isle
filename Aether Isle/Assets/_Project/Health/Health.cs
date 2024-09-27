using System;
using UnityEngine;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public int maxHealth { get; private set; } = 3;
        public int currentHealth { get; private set; }

        [NonSerialized] public bool canTakeDamage = true;
        public bool isDead { get; private set; } = false;

        public event Action<DamageData, Collider2D, Collider2D, Vector2?> OnDamageParams;
        public event Action OnDamage;

        public event Action OnDie;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void Damage(DamageData damage, Collider2D col, Collider2D source, Vector2? dir = null)
        {
            if (!canTakeDamage || isDead)
                return;

            currentHealth -= damage.damage;

            OnDamageParams?.Invoke(damage, col, source, dir);
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
