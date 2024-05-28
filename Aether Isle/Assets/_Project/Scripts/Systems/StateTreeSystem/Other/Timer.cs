using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public class Timer
    {
        public float delay { get; private set; }

        float timeAtStart;

        public Timer(float delay)
        {
            timeAtStart = Time.time;
            this.delay = delay;

            if (delay <= 0)
                Debug.LogError("Delay cannot be less than or equal to 0");
        }


        public bool isDone
        {
            get
            {
                return Time.time - timeAtStart >= delay;
            }
        }

        public void Reset()
        {
            timeAtStart = Time.time;
        }

        /// <summary>
        /// Timer will never be done
        /// </summary>
        public void Stop()
        {
            timeAtStart = Mathf.Infinity;
        }
    }
}
