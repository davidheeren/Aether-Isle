using System;
using UnityEngine;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 3;
        public int currentHealth { get; private set; }

        [NonSerialized] public bool canTakeDamage = true;

        public delegate void _OnDamage(DamageStats damage, Vector2? dir);
        public event _OnDamage OnDamage;

        public event Action OnDie;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void Damage(DamageStats damage, Vector2? dir = null)
        {
            if (!canTakeDamage)
                return;

            currentHealth -= damage.damage;

            OnDamage?.Invoke(damage, dir);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie?.Invoke();
            }
        }
    }
}
