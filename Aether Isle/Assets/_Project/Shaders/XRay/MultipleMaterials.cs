using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MultipleMaterials : MonoBehaviour
    {
        [SerializeField] Material[] materials = new Material[0];
        SpriteRenderer spriteRenderer;

        private void OnValidate()
        {
            UpdateMaterials();
        }

        void UpdateMaterials()
        {
            if (materials.Length == 0)
                return;

            foreach (Material mat in materials)
            {
                if (mat == null)
                    return;
            }

            GetSpriteRenderer().materials = materials;
        }

        SpriteRenderer GetSpriteRenderer()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            return spriteRenderer;
        }
    }
}
