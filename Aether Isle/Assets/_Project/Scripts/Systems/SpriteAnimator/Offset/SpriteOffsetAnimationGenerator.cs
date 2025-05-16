using CustomInspector;
using UnityEngine;

namespace SpriteAnimator
{
    public class SpriteOffsetAnimationGenerator : MonoBehaviour
    {
        [Button(nameof(Increment))]
        [Button(nameof(SetOffset))]

        [SerializeField, Hook(nameof(OnAnimationChanged))] SpriteOffsetAnimation offsetAnim;
        [SerializeField, Hook(nameof(OnAnimationChanged))] SpriteAnimation anim;

        [SerializeField] Transform spriteTransform;
        [SerializeField] SpriteRenderer spriteRenderer;


        [SerializeField] int currentIndex = -1;

        void OnAnimationChanged()
        {
            if (anim == null || offsetAnim == null)
                return;

            currentIndex = -1;
            if (offsetAnim.offsets.Length != anim.Length)
                offsetAnim.offsets = new Vector2[anim.Length];
            offsetAnim.animation = anim;
        }

        void Increment()
        {
            if (offsetAnim == null || anim == null || spriteTransform == null || spriteRenderer == null)
                return;

            currentIndex = (currentIndex + 1) % anim.Length;

            spriteRenderer.sprite = anim.Sprites[currentIndex];
            spriteTransform.localPosition = offsetAnim.offsets[currentIndex];

        }

        void SetOffset()
        {
            if (offsetAnim == null || anim == null || spriteTransform == null || spriteRenderer == null)
                return;

            offsetAnim.offsets[currentIndex] = spriteTransform.localPosition;
        }
    }
}
