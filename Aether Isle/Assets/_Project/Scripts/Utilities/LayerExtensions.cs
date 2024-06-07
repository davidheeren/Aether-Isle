using UnityEngine;

namespace Utilities
{
    public static class LayerExtensions
    {
        public static bool Compare(this LayerMask layerMask, int layer)
        {
            return (layerMask & (1 << layer)) != 0;
        }
    }
}
