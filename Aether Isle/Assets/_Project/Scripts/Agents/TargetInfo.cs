using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TargetInfo
    {
        public bool isActive
        {
            get
            {
                if (target == null) return false;
                return target.isActiveAndEnabled;
            }
        }

        public Target target { get; private set; }

        public bool hasLOS { get; private set; } = true;

        public Vector2? lastKnownPosition {  get; private set; }
        Queue<Vector2> lastKnownPath = new Queue<Vector2>();


        const float nextDist = 3;
        public Vector2 GetKnownPosition(Vector2 position)
        {
            if (!isActive) Debug.LogError("Trying to access smart position while target not active");

            if (!hasLOS && lastKnownPath.Count > 0)
            {
                while (lastKnownPath.Count > 1)
                {
                    if ((lastKnownPath.Peek() - position).sqrMagnitude <= nextDist * nextDist)
                        lastKnownPath.Dequeue();
                    else
                        break;
                }

                return lastKnownPath.Peek();
            }

            return target.Position;
        }

        public TargetInfo DisableTarget()
        {
            target = null;
            hasLOS = false;

            return this;
        }

        public TargetInfo SetNewTarget(Target tar, bool los)
        {
            if (tar == null) { Debug.LogError("Target is null"); return this; }
            if (!tar.isActiveAndEnabled) { Debug.LogError("Target is not active"); return this; }

            target = tar;
            UpdateLOS(los);

            return this;
        }

        public TargetInfo UpdateLOS(bool los)
        {
            if (!isActive)
            {
                Debug.LogError("Trying to update LOS while target is not active");
                return this;
            }

            hasLOS = los;

            if (los)
            {
                lastKnownPosition = null;
                lastKnownPath.Clear();
            }
            else
            {
                lastKnownPath.Enqueue(target.Position);
                lastKnownPosition = target.Position;
            }

            return this;
        }
    }
}
