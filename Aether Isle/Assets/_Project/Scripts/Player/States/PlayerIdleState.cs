using UnityEngine;
using StateTree;

namespace Game
{
    public class PlayerIdleState : State
    {
        Animator animator;

        public PlayerIdleState(Animator animator, Node child) : base(null, child)
        {
            this.animator = animator;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play("Idle");
        }
    }
}
