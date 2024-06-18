using StateTree;
using UnityEngine;

namespace Game
{
    public class CharacterIdleState : State
    {
        Animator animator;

        public CharacterIdleState(Animator animator, Node child = null) : base(null, child)
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
