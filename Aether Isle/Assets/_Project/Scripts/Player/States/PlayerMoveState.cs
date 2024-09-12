using SpriteAnimator;
using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerMoveState : State
    {
        [SerializeField] float speed = 5;
        [SerializeField] SpriteAnimation animation;
        [SerializeField] SFXLoop SFXLoop;

        CharacterComponents components;

        private PlayerMoveState() : base(null) { }
        public PlayerMoveState Init(CharacterComponents components, Node child = null)
        {
            InitializeState(child);
            this.components = components;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play(animation);

            SFXLoop?.Play();
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * speed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            SFXLoop?.Stop();
        }
    }
}
