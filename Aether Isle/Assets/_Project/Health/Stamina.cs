using System;
using UnityEngine;
using Utilities;

namespace Game
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] float maxStamina = 100;
        [SerializeField] float rechargeValue = 10;
        [SerializeField] float rechargeWaitTime = 1;

        public float CurrentStamina => staminaValue.Value;
        public float MaxStamina => staminaValue.maxValue;

        private RechargingValue staminaValue;

        public event Action OnStaminaChanged;

        private void Awake()
        {
            staminaValue = new RechargingValue(maxStamina, rechargeValue, rechargeWaitTime);

            staminaValue.OnAddValue += OnAddRechargeValue;
            staminaValue.OnSubtractValue += OnSubtractRechargeValue;
        }

        
        void Update()
        {
            staminaValue.UpdateRecharge();
        }

        public void AddStamina(float add)
        {
            staminaValue.AddValue(add);
        }

        public void SubtractStamina(float subtract)
        {
            staminaValue.SubtractValue(subtract);
        }

        private void OnAddRechargeValue(float add) => OnStaminaChanged?.Invoke();

        private void OnSubtractRechargeValue(float subtract) => OnStaminaChanged?.Invoke();
    }
}
