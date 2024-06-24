using UnityEngine;

namespace Utilities
{
    public static class LayerExtensions
    {
        public static bool Compare(this LayerMask layerMask, int layer)
        {
            return (layerMask & (1 << layer)) != 0;
        }

        // Only in Awake of after; not in constructor
        public static LayerMask GetLayerMaskByName(this LayerMask mask, string layerName)
        {
            int layerIndex = LayerMask.NameToLayer(layerName);

            if (layerIndex == -1)
                Debug.LogWarning("Layer Not Found");

            return 1 << layerIndex;
        }

    }
}
