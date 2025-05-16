using UnityEngine;

namespace SpriteAnimator
{
    [CreateAssetMenu(menuName = "SpriteAnimator/OffsetAnimation")]
    public class SpriteOffsetAnimation : ScriptableObject
    {
        public SpriteAnimation animation;
        public Vector2[] offsets;
    }
}
