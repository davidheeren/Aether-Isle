using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ZombieChaseState : State
    {
        [SerializeField] float chaseSpeed = 3;

        AIMovement aiMovement;
        Animator animator;

        public ZombieChaseState Create(AIMovement aiMovement, Animator animator, Node child = null)
        {
            CreateState(child);

            this.aiMovement = aiMovement;
            this.animator = animator;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play("Run");

            aiMovement.Enter();
        }

        protected override void UpdateState()
        {
            aiMovement.Update(chaseSpeed);
        }
    }
}
