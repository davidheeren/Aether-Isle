using UnityEngine;

namespace Game
{
    public interface IAimDirection
    {
        /// <summary>
        /// Should be normalized
        /// </summary>
        public Vector2 AimDirection { get; }
    }
}
