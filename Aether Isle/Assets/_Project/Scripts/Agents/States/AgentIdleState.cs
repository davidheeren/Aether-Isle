using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class AgentIdleState : State
    {
        Data data;
        SpriteAnimatorController animator;

        public AgentIdleState(Data data, SpriteAnimatorController animator, Node child = null) : base(child)
        {
            this.data = data;
            this.animator = animator;
        }

        [System.Serializable]
        public class Data
        {
            public SpriteAnimation animation;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play(data.animation);

            //if (animation != null)
            //    animator.Play(animation);
            //else
            //    animator.Play("Idle");
        }
    }
}
