using SpriteAnimator;
using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    public class PlayerMoveState : State
    {
        Data data;
        ActorComponents components;

        public PlayerMoveState(Data data, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
        }

        [System.Serializable]
        public class Data
        {
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

            float speed = components.stats.GetStat(StatType.moveSpeed);
            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * speed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            data.SFXLoop?.Pause();
        }
    }
}
