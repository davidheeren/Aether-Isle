using UnityEngine;
using StateTree;
using System;

namespace Game
{
    [Serializable]
    public class PlayerDashState : State
    {
        [SerializeField] float dashSpeed = 10;
        [SerializeField] AudioClip dashSFX;

        Movement movement;

        Vector2 dashDir;

        private PlayerDashState() : base(null, null) { }
        public PlayerDashState(string copyJson, Movement movement, Node child) : base(copyJson, child)
        {
            this.movement= movement;
        }

        protected override void EnterState()
        {
            base.EnterState();

            dashDir = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
            SFXManager.Instance.PlaySFXClip(dashSFX, movement.transform.position);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            movement.Move(dashDir * dashSpeed);
        }
    }
}
