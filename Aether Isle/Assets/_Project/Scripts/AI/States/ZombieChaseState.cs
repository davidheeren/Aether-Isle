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

        private ZombieChaseState() : base(null, null) { }
        public ZombieChaseState(string copyJson, AIMovement aiMovement, Animator animator, Node child = null) : base(copyJson, child)
        {
            this.aiMovement = aiMovement;
            this.animator = animator;
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
