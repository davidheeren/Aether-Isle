using DamageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(ActorComponents))]
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth  = 3;
        [SerializeField] float rechargeValue = 0;
        [SerializeField] float rechargeWaitTime = 10;
        [SerializeField] float invulnerableTime = 0;
        public float CurrentHealth => healthValue.Value;
        public float MaxHealth => healthValue.maxValue;

        [NonSerialized] public bool canTakeDamage = true; // Won't allow current health to change
        public bool isInvulnerable { get; private set; } = false; // Won't allow new damage types to be added
        public bool isDead { get; private set; } = false;

        public event Action<float, float, ActorComponents> OnDamage;
        public event Action<float> OnHealthAdd;
        public event Action OnDie;

        Dictionary<Type, Damage> damages = new Dictionary<Type, Damage>();
        ActorComponents components;

        Timer invulnerableTimer;

        RechargingValue healthValue;

        private void Awake()
        {
            components = GetComponent<ActorComponents>();

            healthValue = new RechargingValue(maxHealth, rechargeValue, rechargeWaitTime);
            healthValue.OnAddValue += OnRechargeValueAdd;

            if (invulnerableTime > 0)
                invulnerableTimer = new Timer(invulnerableTime).Stop();
        }

        #region Update Loop
        private void Update()
        {
            UpdateInvulnerableTimer();
            UpdateDamage();
            healthValue.UpdateRecharge();
        }

        // Listens to recharging value
        private void OnRechargeValueAdd(float add)
        {
            OnHealthAdd?.Invoke(add);
        }

        // Testing for now need to call params event
        public void RestoreHealth()
        {
            healthValue.RestoreValue();
        }

        public void AddHealth(float add)
        {
            healthValue.AddValue(add);
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
        public void Damage(float damage, float stunTime, ActorComponents sourceComponents)
        {
            if (!canTakeDamage || isDead)
                return;

            healthValue.SubtractValue(damage);
            OnDamage?.Invoke(damage, stunTime, sourceComponents);

            // Apply Death
            if (healthValue.Value == 0)
            {
                isDead = true;
                OnDie?.Invoke();
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
            if (isInvulnerable || isDead || !canTakeDamage)
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
