using SpriteAnimator;
using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ZombieChaseState : State
    {
        [SerializeField] float chaseSpeed = 3;
        [SerializeField] SpriteAnimation animation;

        AIMovement aiMovement;
        SpriteAnimatorController animator;

        private ZombieChaseState() : base(null) { }
        public ZombieChaseState Init(AIMovement aiMovement, SpriteAnimatorController animator, Node child = null)
        {
            InitializeState(child);

            this.aiMovement = aiMovement;
            this.animator = animator;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play(animation);

            aiMovement.Enter();
        }

        protected override void UpdateState()
        {
            aiMovement.Update(chaseSpeed);
        }
    }
}
