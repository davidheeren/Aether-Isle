using System;
using UnityEngine;

namespace Utilities
{
    public class RechargingValue
    {
        public float Value => _value;
        private float _value;

        public readonly float maxValue;
        public readonly float rechargeValue;

        public event Action<float> OnAddValue;
        public event Action<float> OnSubtractValue;

        Timer waitTimer;

        public RechargingValue(float maxValue, float rechargeValue, float waitTime)
        {
            this.maxValue = maxValue;
            this.rechargeValue = rechargeValue;
            _value = maxValue;

            waitTimer = new Timer(waitTime);
        }

        public void AddValue(float add)
        {
            _value = Mathf.Clamp(_value + add, 0, maxValue);
            OnAddValue?.Invoke(add);
        } 

        public void SubtractValue(float subtract)
        {
            _value = Mathf.Clamp(_value - subtract, 0, maxValue);

            waitTimer.Reset();

            OnSubtractValue?.Invoke(subtract);
        }

        public void RestoreValue()
        {
            float diff = maxValue - _value;

            if (diff == 0)
                return;

            _value = maxValue;

            OnAddValue?.Invoke(diff);
        }

        /// <summary>
        /// Returns true if successfully recharged the value
        /// </summary>
        /// <returns></returns>
        public void UpdateRecharge()
        {
            if (_value == maxValue || !waitTimer.IsDone)
                return;

            AddValue(rechargeValue * Time.deltaTime);
        }
    }

    //public class RechargingValue
    //{
    //    public float Value => _value;
    //    private float _value;

    //    readonly float maxValue;
    //    readonly float rechargeValue;

    //    public event Action<float> OnAddValue;
    //    public event Action<float> OnSubtractValue;

    //    Timer waitTimer;
    //    Timer tickTimer;

    //    public RechargingValue(float maxValue, float rechargeValue, float waitTime, float tickTime)
    //    {
    //        this.maxValue = maxValue;
    //        this.rechargeValue = rechargeValue;
    //        _value = maxValue;

    //        waitTimer = new Timer(waitTime);
    //        tickTimer = new Timer(tickTime).Stop();
    //    }

    //    public void AddValue(float add)
    //    {
    //        _value = Mathf.Clamp(_value + add, 0, maxValue);
    //        OnAddValue?.Invoke(add);
    //    }

    //    public void SubtractValue(float subtract)
    //    {
    //        _value = Mathf.Clamp(_value - subtract, 0, maxValue);

    //        waitTimer.Reset();
    //        tickTimer.Stop();

    //        OnSubtractValue?.Invoke(subtract);
    //    }

    //    /// <summary>
    //    /// Returns true if successfully recharged the value
    //    /// </summary>
    //    /// <returns></returns>
    //    public void UpdateRecharge()
    //    {
    //        if (_value == maxValue)
    //            return;

    //        if (!waitTimer.IsDone)
    //        {
    //            if (tickTimer.IsStopped) // should only happen once when the waitTimer is first done
    //                tickTimer.Reset();

    //            return;
    //        }

    //        if (tickTimer.IsDone)
    //        {
    //            AddValue(rechargeValue);
    //            tickTimer.Reset();
    //            return;
    //        }
    //    }
    //}
}
