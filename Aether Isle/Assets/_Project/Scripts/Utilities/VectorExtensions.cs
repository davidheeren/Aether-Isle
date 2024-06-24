using UnityEngine;

namespace Game
{
    public static class VectorExtensions
    {
        public static Vector2 Reciprocal(this Vector2 vector)
        {
            float sqrMag = vector.sqrMagnitude;

            if (sqrMag == 0)
                return Vector3.zero;

            float invSqrMag = 1.0f / sqrMag;

            return vector * invSqrMag;
        }
    }
}
