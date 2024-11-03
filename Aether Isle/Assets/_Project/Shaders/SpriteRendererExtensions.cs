using UnityEngine;

namespace Game
{
    public static class SpriteRendererExtensions
    {
        private static readonly MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        public static void SetPropertyFloat(this SpriteRenderer sr, string property, float value)
        {
            //MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            //sr.GetPropertyBlock(propertyBlock, 0);
            //propertyBlock.SetFloat(property, value);
            //sr.SetPropertyBlock(propertyBlock, 0);

            // Weird things were happening to sprite sheet sampling when setting property block
            sr.material.SetFloat(property, value);
        }

        public static void SetPropertyColor(this SpriteRenderer sr, string property, Color value)
        {
            //MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            sr.GetPropertyBlock(propertyBlock, 0);
            propertyBlock.SetColor(property, value);
            sr.SetPropertyBlock(propertyBlock, 0);
        }

        public static void SetPropertyInt(this SpriteRenderer sr, string property, int value)
        {
            //MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            sr.GetPropertyBlock(propertyBlock, 0);
            propertyBlock.SetInt(property, value);
            sr.SetPropertyBlock(propertyBlock, 0);
        }
    }
}
