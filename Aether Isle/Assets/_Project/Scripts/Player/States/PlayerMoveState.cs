using SpriteAnimator;
using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    public class PlayerMoveState : State
    {
        Data data;
        ObjectStats stats;
        ActorComponents components;

        public PlayerMoveState(Data data, ObjectStats stats, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.stats = stats;
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

            float speed = stats.GetStat(StatType.moveSpeed);
            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * speed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            data.SFXLoop?.Pause();
        }
    }
}
