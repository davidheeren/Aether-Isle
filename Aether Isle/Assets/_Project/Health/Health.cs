using DamageSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(ActorComponents))]
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public int maxHealth { get; private set; } = 3;
        [SerializeField] float invulnerableTime = 0;
        public int currentHealth { get; private set; }

        [NonSerialized] public bool canTakeDamage = true; // Won't allow current health to change
        public bool isInvulnerable { get; private set; } = false; // Won't allow new damage types to be added
        public bool isDead { get; private set; } = false;

        public event Action<int, float, ActorComponents> OnDamageParams;
        public event Action OnDamage;
        public event Action OnDie;

        Dictionary<Type, Damage> damages = new Dictionary<Type, Damage>();
        ActorComponents components;

        Timer invulnerableTimer;

        private void Awake()
        {
            components = GetComponent<ActorComponents>();
            currentHealth = maxHealth;

            if (invulnerableTime > 0)
                invulnerableTimer = new Timer(invulnerableTime).Stop();
        }

        #region Update Loop
        private void Update()
        {
            UpdateInvulnerableTimer();
            UpdateDamage();
        }

        void UpdateInvulnerableTimer()
        {
            if (invulnerableTimer == null) 
                return;

            float delay = components.stats.GetStat(Stats.StatType.invulnerableTime, invulnerableTime);
            invulnerableTimer.SetDelay(delay); // Apply invulnerable time modifiers

            if (invulnerableTimer.IsDone)
            {
                isInvulnerable = false;
                invulnerableTimer.Stop();
                components.spriteRenderer.SetPropertyFloat("_MainAlpha", 1f);
            }
        }

        void UpdateDamage()
        {
            List<Type> keysToRemove = new List<Type>();

            foreach (KeyValuePair<Type, Damage> damage in damages)
            {
                if (damage.Value.ShouldExit())
                    keysToRemove.Add(damage.Key);
                else
                    damage.Value.Update();
            }

            RemoveDamage(keysToRemove.ToArray());
        }
        #endregion

        #region Damage Health
        public void Damage(int damage, float stunTime, ActorComponents sourceComponents)
        {
            if (!canTakeDamage || isDead)
                return;

            currentHealth -= damage;

            // Invoke Events
            OnDamageParams?.Invoke(damage, stunTime, sourceComponents);
            OnDamage?.Invoke();

            // Apply Death
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                OnDie?.Invoke();
                //StartCoroutine(nameof(ClearDamages)); // Change to coroutine if needed later
            }
            else
            {
                // SetInvulnerable
                if (invulnerableTimer != null)
                {
                    isInvulnerable = true;
                    invulnerableTimer.Reset();
                    components.spriteRenderer.SetPropertyFloat("_MainAlpha", 0.75f);
                }
            }
        }
        #endregion

        #region Add, Remove, Clear, Contains Damages
        public void AddDamage(DamageData damageData, ActorComponents sourceComponents, Vector2 direction)
        {
            Damage damage = damageData.CreateDamage(components, sourceComponents, direction);
            AddDamage(damage);
        }

        public void AddDamage(Damage damage)
        {
            if (isInvulnerable || isDead)
                return;

            if (damage == null) { Debug.LogError("Damage should never be null"); return; }

            Type type = damage.GetType();

            if (damages.ContainsKey(type))
                RemoveDamage(type);

            damage.Enter();
            damages[type] = damage;
        }

        public void RemoveDamage(params Type[] types)
        {
            foreach (Type type in types)
            {
                if (damages.TryGetValue(type, out Damage damage))
                {
                    damage.Exit();
                    damages.Remove(type);
                }
            }
        }

        public void ClearDamages()
        {
            RemoveDamage(damages.Keys.ToArray());
        }

        public bool ContainsDamage(Type type)
        {
            return damages.ContainsKey(type);
        }
        #endregion
    }
}
