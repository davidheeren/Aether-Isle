using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class EnemyChaseState : State
    {
        [SerializeField] float chaseSpeed = 3;

        AIMovement aiMovement;
        Animator animator;

<<<<<<< Updated upstream:Aether Isle/Assets/_Project/Scripts/AI/States/ZombieChaseState.cs
        private ZombieChaseState() : base(null, null) { }
        public ZombieChaseState(string copyJson, AIMovement aiMovement, Animator animator, Node child = null) : base(copyJson, child)
=======
        public EnemyChaseState Create(AIMovement aiMovement, Animator animator, Node child = null)
>>>>>>> Stashed changes:Aether Isle/Assets/_Project/Scripts/AI/States/EnemyChaseState.cs
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
