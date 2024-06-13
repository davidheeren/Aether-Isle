using UnityEngine;
using StateTree;
using System;

namespace Game
{
    [Serializable]
    public class PlayerRunState : State
    {
        [SerializeField] float runSpeed = 5;

        Movement movement;
        Animator animator;

        private PlayerRunState() : base(null, null) { }
        public PlayerRunState(string copyJson, Movement movement, Animator animator, Node child) : base(copyJson, child)
        {
            this.movement = movement;
            this.animator = animator;
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play("Run");
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * runSpeed);
        }
    }
}
