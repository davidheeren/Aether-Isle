using SpriteAnimator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class SpriteAnimatorTest : MonoBehaviour
    {
        [SerializeField] SpriteAnimation anim1;
        [SerializeField] string anim2;
        [SerializeField] int anim3;

        SpriteAnimatorController animator;

        void Awake()
        {
            animator = GetComponent<SpriteAnimatorController>();
        }

        void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (animator.isPaused)
                    animator.Resume();
                else
                    animator.Pause();
            }

            if (Keyboard.current.rKey.wasPressedThisFrame)
                animator.Restart();

            if (Keyboard.current.sKey.wasPressedThisFrame)
                animator.Stop();

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                animator.Play(anim1);

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
                animator.Play(anim2);

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
                animator.Play(anim3);
        }
    }
}
