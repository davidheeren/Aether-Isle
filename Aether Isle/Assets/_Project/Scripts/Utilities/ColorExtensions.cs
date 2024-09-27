using UnityEngine;

namespace Game
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;  // Modify the alpha value
            return color;     // Return the modified color
        }
    }
}
