using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class CharacterIdleState : State
    {
        [SerializeField] SpriteAnimation animation;
        SpriteAnimatorController animator;

        private CharacterIdleState() : base(null) { }
        public CharacterIdleState Init(SpriteAnimatorController animator, Node child = null)
        {
            InitializeState(child);

            this.animator = animator;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play(animation);

            //if (animation != null)
            //    animator.Play(animation);
            //else
            //    animator.Play("Idle");
        }
    }
}
