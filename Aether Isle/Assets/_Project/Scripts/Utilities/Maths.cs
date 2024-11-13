using UnityEngine;

namespace Utilities
{
    public static class Maths
    {
        // https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
        // The % in c# is the remainder not modulus so will not return correct values or wrapping if x is 0 or negative
        public static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static Vector2 DirectionFromAngle(float radians)
        {
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public static float InverseClamp(float value, float min, float max, float threshold)
        {
            if (value < min + threshold)
                value = min;
            if (value > max - threshold)
                value = max;
            return value;
        }

        public static Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;

            Vector2 output = Vector2.zero;
            output.x = vector.x * Mathf.Cos(rad) - vector.y * Mathf.Sin(rad);
            output.y = vector.x * Mathf.Sin(rad) + vector.y * Mathf.Cos(rad);

            return output;
        }

        public static Vector2 Quadratic(float a, float b, float c)
        {
            Vector2 answer;
            answer.x = (-b + Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
            answer.y = (-b - Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
            return answer;
        }

        // https://www.gamedev.net/forums/topic/401165-target-prediction-system--target-leading/
        public static Vector2 TargetPrediction(Vector2 targetPos, Vector2 targetVel, Vector2 startPos, float projectileSpeed)
        {
            float a = (projectileSpeed * projectileSpeed) - (Vector2.Dot(targetVel, targetVel));
            float b = -2 * Vector2.Dot(targetPos - startPos, targetVel);
            float c = -Vector2.Dot(targetPos - startPos, targetPos - startPos);
            Vector2 quad = Quadratic(a, b, c);
            float t = Mathf.Max(quad.x, quad.y);

            if (!float.IsNormal(t) || t < 0)
            {
                // If 0, either of infinities or NaN
                // If t < 0

                Debug.LogWarning("Projectile slower than target, t: " + t);
                return targetPos;
            }

            Vector2 targetPred = targetPos + (targetVel * t);
            return targetPred;
        }

        public static Vector2 Reciprocal(Vector2 vector)
        {
            float sqrMag = vector.sqrMagnitude;

            if (sqrMag == 0)
                return Vector2.zero;

            float invSqrMag = 1.0f / sqrMag;

            return vector * invSqrMag;
        }

        //public static float DistanceAngleFactor(Vector2 vector1, Vector2 vector2)
        //{
        //    return Vector2.Dot(Reciprocal(vector1), Reciprocal(vector2));
        //}

        /// <summary>
        /// Returns normalized factor based on distance and the angle. The smaller the distance and the smaller the angle, the larger the value will be
        /// </summary>
        /// <returns></returns>
        public static float DistanceAngleFactor(Vector2 pos, Vector2 dir, Vector2 targetPos, float radius, float angleFactor01 = 0.5f)
        {
            Vector2 distTo = targetPos - pos;
            float dist = Mathf.Clamp(distTo.magnitude, 0, radius);
            float deltaAngle = Vector3.Angle(distTo, dir); // degrees

            float distFac = Mathf.InverseLerp(radius, 0, dist) * 0.5f;
            float angleFac = Mathf.InverseLerp(180, 0, deltaAngle) * 0.5f;

            return (distFac * (1 - angleFactor01) * 2) + (angleFac * angleFactor01 * 2);
        }

        public static bool InRadius(Vector2 pos, Vector2 targetPos, float radius)
        {
            float sqrDist = (targetPos - pos).sqrMagnitude;
            return sqrDist < radius * radius;
        }

        #region Random
        public static float RandomSign()
        {
            return Random.Range(0, 2) == 0 ? 1 : -1;
        }

        // TODO: Refactor
        public static float RandomDistributed(float maxValue, float distributionPo, bool inverse, bool AddNegative)
        {
            // Greater Distribution Co results in more close to zero
            // Due to some rounding errors close to 0, I think, it seems to work better to 1 / Po and inverse
            float randInitial = UnityEngine.Random.Range(0f, 1f);
            float randTransformed = Mathf.Pow(randInitial, 1 / distributionPo);
            float randLerp = Mathf.InverseLerp(0, 1, randTransformed);
            if (!inverse)
                randLerp = 1 - randLerp;
            float randFinal = Mathf.Lerp(0, maxValue, randLerp);
            if (AddNegative)
                randFinal = randFinal * Maths.RandomSign();
            return randFinal;
        }
        #endregion
    }
}
