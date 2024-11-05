using UnityEngine;

namespace Utilities
{
    public class RechargingValue
    {
        public int Value => _value;
        private int _value;

        readonly int startValue;

        Timer waitTimer;
        Timer tickTimer;

        public RechargingValue(int startValue, float waitTime, float tickTime)
        {
            this.startValue = startValue;
            _value = startValue;

            waitTimer = new Timer(waitTime);
            tickTimer = new Timer(tickTime).Stop();
        }

        public void AddValue(int add)
        {
            _value = Mathf.Clamp(_value + add, 0, startValue);
        } 

        public void SubtractValue(int subtract)
        {
            _value = Mathf.Clamp(_value - subtract, 0, startValue);

            waitTimer.Reset();
            tickTimer.Stop();
        }

        /// <summary>
        /// Returns true if successfully recharged the value
        /// </summary>
        /// <returns></returns>
        public bool UpdateRecharge()
        {
            if (!waitTimer.IsDone)
            {
                if (tickTimer.IsStopped) // should only happen once when the waitTimer is first done
                    tickTimer.Reset();

                return false;
            }

            if (tickTimer.IsDone)
            {
                AddValue(1);
                tickTimer.Reset();
                return true;
            }

            return false;
        }
    }
}
