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
    }
}
