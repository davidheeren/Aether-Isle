using UnityEngine;

namespace Utilities
{
    public class Timer
    {
        public float delay { get; private set; }

        float timeAtStart;

        public bool isPaused { get; private set; }
        bool wasPaused;

        float timeAtPause;
        float timeAtResume;

        float totalPauseTime = 0; // added up pause time of previous pauses but not the current one
    
        public Timer(float delay)
        {
            timeAtStart = Time.time;
            this.delay = delay;

            if (delay <= 0)
                Debug.LogError("Delay cannot be less than or equal to 0");
        }

        float currentPauseDeltaTime
        {
            get
            {
                if (isPaused)
                    return Time.time - timeAtPause;
                else if (wasPaused)
                    return timeAtResume - timeAtPause;
                else
                    return 0;
            }
        }

        public float currentDeltaTime
        {
            get
            {
                // only clamp this value
                float deltaTime = timeAtStart + delay - Time.time;
                return Mathf.Max(deltaTime + currentPauseDeltaTime + totalPauseTime, 0);
            }
        }
        public float currentPercent // starts at 1 and goes to 0
        {
            get
            {
                return currentDeltaTime / delay;
            }
        }

        public bool isDone
        {
            get
            {
                return currentDeltaTime == 0;
            }
        }
        
        public void Reset()
        {
            timeAtStart = Time.time;
            isPaused = false;
            wasPaused = false;
            totalPauseTime = 0;
        }

        public void Stop()
        {
            timeAtStart = Mathf.Infinity;
        }
        public void Pause()
        {
            if (isPaused)
            {
                Debug.LogWarning("Paused SimpleTimer while already paused");
                return;
            }

            if (wasPaused) // incudes previous pause times
                totalPauseTime += timeAtResume - timeAtPause;

            isPaused = true;
            wasPaused = true;
            timeAtPause = Time.time;
        }
        public void Resume()
        {
            if (!isPaused)
            {
                Debug.LogWarning("Resumed SimpleTimer while already resumed");
                return;
            }

            isPaused = false;
            timeAtResume = Time.time;
        }
    }
}
