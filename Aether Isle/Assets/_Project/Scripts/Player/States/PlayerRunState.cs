using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerRunState : State
    {
        [SerializeField] float runSpeed = 5;

        CharacterComponents components;
        SFXLoop runSFX;

        private PlayerRunState() : base(null, null) { }
        public PlayerRunState(string copyJson, CharacterComponents components, SFXLoop runSFX, Node child = null) : base(copyJson, child)
        {
            this.components = components;
            this.runSFX = runSFX;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play("Run");

            runSFX.Play();
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * runSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            runSFX.Stop();
        }
    }
}
