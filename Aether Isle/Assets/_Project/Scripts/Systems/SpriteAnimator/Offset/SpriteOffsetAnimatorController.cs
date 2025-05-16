using UnityEngine;

namespace SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteOffsetAnimatorController : MonoBehaviour
    {
        [SerializeField] SpriteAnimatorController controller;
        [SerializeField] SpriteOffsetAnimation[] offsets;

        SpriteOffsetAnimation currentOffset;
        SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (controller == null) return;
            controller.OnSpriteChanged += OnSpriteChanged;
            controller.OnAnimationChanged += OnAnimationChanged;
        }

        private void OnDisable()
        {
            if (controller == null) return;
            controller.OnSpriteChanged -= OnSpriteChanged;
            controller.OnAnimationChanged -= OnAnimationChanged;
        }

        private void OnSpriteChanged()
        {
            if (currentOffset == null) return;

            transform.localPosition = currentOffset.offsets[controller.currentSpriteIndex];
        }

        private void OnAnimationChanged()
        {
            TryGetMatchingOffset(controller.currentAnimation, out SpriteOffsetAnimation offset);
            currentOffset = offset;
            // Offset will be null if no matches
        }

        [ContextMenu("Set First Offset")]
        private void SetFirstOffset()
        {
            if (controller == null) return;

            if (!controller.TryGetAnimation(0, out SpriteAnimation anim)) return;

            if (!TryGetMatchingOffset(anim, out SpriteOffsetAnimation offset)) return;

            if (offset.offsets.Length == 0) return;

            transform.localPosition = offset.offsets[0];
        }

        private bool TryGetMatchingOffset(SpriteAnimation anim, out SpriteOffsetAnimation offset)
        {
            foreach (SpriteOffsetAnimation o in offsets)
            {
                if (o.animation == anim)
                {
                    offset = o;
                    return true;
                }
            }

            offset = null;
            return false;
        }
    }
}
