using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TargetInfo
    {
        // GameObject, transform, and collider will be null if !isActive
        public bool isActive {  get; private set; }
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
        public Collider2D collider { get; private set; }

        // Both Vector2 will be null if !hasLOS
        public bool hasLOS { get; private set; } = true;
        public Vector2? lastKnownPosition {  get; private set; }
        public Vector2? lastKnownPredictionPosition {  get; private set; }
        Queue<Vector2> lastKnownPath = new Queue<Vector2>();

        const float nextDist = 3;
        public Vector2 GetSmartPosition(Vector2 position)
        {
            if (!isActive) Debug.LogError("Trying to access smart position while target not active");

            if (!hasLOS && lastKnownPath.Count > 0)
            {
                //if ((lastKnownPath.Peek() - position).sqrMagnitude <= nextDist * nextDist && lastKnownPath.Count > 1)
                //    lastKnownPath.Dequeue();

                while (lastKnownPath.Count > 1)
                {
                    if ((lastKnownPath.Peek() - position).sqrMagnitude <= nextDist * nextDist)
                        lastKnownPath.Dequeue();
                    else
                        break;
                }

                return lastKnownPath.Peek();
            }

            return transform.position;
        }

        public TargetInfo SetTarget(Collider2D col, bool los)
        {
            if (col != null)
            {
                isActive = true;

                collider = col;
                gameObject = col.gameObject;
                transform = col.transform;

                // Even if los is false, we still want to set the last known positions
                hasLOS = los;
                SetLastKnownPositions();
            }
            else
            {
                UpdateLOS(los);

                isActive = false;

                gameObject = null;
                transform = null;
                collider = null;

                if (los)
                {
                    Debug.LogError("Collider is null but LOS is true");
                    los = false;
                }
            }

            return this;
        }

        public TargetInfo UpdateLOS(bool los)
        {
            if (!isActive)
                Debug.LogError("Trying to update LOS while target is not active");

            if (los)
            {
                hasLOS = true;

                lastKnownPosition = null;
                lastKnownPredictionPosition = null;

                lastKnownPath.Clear();
            }
            else
            {
                lastKnownPath.Enqueue(transform.position);

                if (!hasLOS) // So that is doesn't keep updating last known position
                    return this;

                hasLOS = false;

                SetLastKnownPositions();
            }

            return this;
        }

        void SetLastKnownPositions()
        {
            lastKnownPath.Clear();

            lastKnownPosition = transform.position;

            lastKnownPredictionPosition = transform.position;

            float predictionAmount = 2;
            if (gameObject.TryGetComponent<Movement>(out Movement movement))
                lastKnownPredictionPosition = (Vector2)transform.position + movement.targetVelocity.normalized * predictionAmount;
        }
    }
}
