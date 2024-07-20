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
        SFXLoop runSFXLoop;

        public PlayerRunState Create(CharacterComponents components, SFXLoop runSFXLoop, Node child = null)
        {
            CreateState(child);

            this.components = components;
            this.runSFXLoop = runSFXLoop;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play("Run");

            runSFXLoop.Play();
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * runSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            runSFXLoop.Stop();
        }
    }
}
