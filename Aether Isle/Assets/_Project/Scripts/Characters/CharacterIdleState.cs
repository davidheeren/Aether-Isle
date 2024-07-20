using StateTree;
using UnityEngine;

namespace Game
{
    public class CharacterIdleState : State
    {
        Animator animator;

        public CharacterIdleState Create(Animator animator, Node child = null)
        {
            CreateState(child);

            this.animator = animator;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play("Idle");
        }
    }
}
