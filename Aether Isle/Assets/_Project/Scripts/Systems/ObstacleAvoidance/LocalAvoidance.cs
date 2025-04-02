using UnityEngine;
using System;

namespace Game
{
    public class LocalAvoidance
    {
        // Use spatial hash grid instead of raycasting

        public Vector2 GetAvoidance(Vector2 position)
        {
            throw new NotImplementedException();
        }

        private Vector2 Reciprocal(Vector2 vector)
        {
            float sqrMag = vector.sqrMagnitude;

            if (sqrMag == 0)
                return Vector3.zero;

            float invSqrMag = 1.0f / sqrMag;

            return vector * invSqrMag;
        }
    }
}
