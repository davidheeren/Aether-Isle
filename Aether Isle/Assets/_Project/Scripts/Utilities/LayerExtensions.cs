using UnityEngine;

namespace Utilities
{
    public static class LayerExtensions
    {
        public static bool Compare(this LayerMask layerMask, int layer)
        {
            return (layerMask & (1 << layer)) != 0; // Double check that -1 returns all layers
        }

        // Only in Awake of after; not in constructor
        public static LayerMask GetLayerMaskByName(this LayerMask mask, string layerName)
        {
            int layerIndex = LayerMask.NameToLayer(layerName);

            if (layerIndex == -1)
                Debug.LogWarning("Layer Not Found");

            return 1 << layerIndex;
        }

        public static LayerMask GetLayerMask(this int layer)
        {
            return (LayerMask)(1 << layer);
        }
    }
}
