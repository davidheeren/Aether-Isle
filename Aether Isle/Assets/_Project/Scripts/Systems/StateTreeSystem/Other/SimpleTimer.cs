using System;
using UnityEngine;

namespace StateTree
{
    public class SimpleTimer
    {
        public float delay { get; private set; }

        float timeAtStart;

        public SimpleTimer(float delay)
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
        /// SimpleTimer will never be done
        /// </summary>
        public void Stop()
        {
            timeAtStart = Mathf.Infinity;
        }

        /// <summary>
        /// SimpleTimer will always be done
        /// </summary>
        public void ForceDone()
        {
            timeAtStart = Mathf.NegativeInfinity;
        }
    }
}
