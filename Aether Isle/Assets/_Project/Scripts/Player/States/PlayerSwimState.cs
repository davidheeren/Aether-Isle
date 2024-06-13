using UnityEngine;
using StateTree;
using System;

namespace Game
{
    [Serializable]
    public class PlayerSwimState : State
    {
        [SerializeField] float swimSpeed = 2;
        [SerializeField] GameObject aimGraphic;
        [SerializeField] AudioClip spashSFX;

        Movement movement;
        Animator animator;

        private PlayerSwimState() : base(null, null) { }
        public PlayerSwimState(string copyJson, Movement movement, Animator animator, Node child) : base(copyJson, child)
        {
            this.movement = movement;
            this.animator = animator;
        }

        protected override void EnterState()
        {
            base.EnterState();

            aimGraphic.SetActive(false);

            SFXManager.Instance.PlaySFXClip(spashSFX, movement.transform.position);

            animator.Play("Swim");
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * swimSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            aimGraphic?.SetActive(true);
        }
    }
}
