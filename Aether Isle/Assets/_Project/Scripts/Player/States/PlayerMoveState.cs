using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerMoveState : State
    {
        Data data;
        CharacterComponents components;

        public PlayerMoveState(Data data, CharacterComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
        }

        [System.Serializable]
        public class Data
        {
            public float speed = 5;
            public SpriteAnimation animation;
            public SFXLoop SFXLoop;
        }

        protected override bool CanEnterState()
        {
            return InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play(data.animation);

            data.SFXLoop?.Play();
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * data.speed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            data.SFXLoop?.Stop();
        }
    }
}
