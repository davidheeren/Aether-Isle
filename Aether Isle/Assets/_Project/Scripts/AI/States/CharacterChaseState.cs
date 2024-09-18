using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class CharacterChaseState : State
    {
        Data data;
        CharacterMovement aiMovement;
        SpriteAnimatorController animator;

        public CharacterChaseState(Data data, CharacterMovement aiMovement, SpriteAnimatorController animator, Node child = null) : base(child)
        {
            this.data = data;
            this.aiMovement = aiMovement;
            this.animator = animator;
        }

        [System.Serializable]
        public class Data
        {
            public float chaseSpeed = 3;
            public SpriteAnimation animation;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play(data.animation);

            aiMovement.Enter();
        }

        protected override void UpdateState()
        {
            aiMovement.Update(data.chaseSpeed);
        }
    }
}
