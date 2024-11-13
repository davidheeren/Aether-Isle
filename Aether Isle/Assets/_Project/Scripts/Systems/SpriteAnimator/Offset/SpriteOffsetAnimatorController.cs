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
            currentOffset = null;

            foreach (SpriteOffsetAnimation offset in offsets)
            {
                if (offset.animation == controller.currentAnimation)
                {
                    currentOffset = offset;
                    break;
                }
            }
        }
    }
}
