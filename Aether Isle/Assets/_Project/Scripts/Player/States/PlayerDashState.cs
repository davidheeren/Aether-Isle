using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerDashState : State
    {
        Data data;
        CharacterComponents components;
        Target target;

        Vector2 dashDir;
        LayerMask initialExcludeLayers;

        public PlayerDashState(Data data, CharacterComponents components, Target target, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
            this.target = target;

            initialExcludeLayers = components.col.excludeLayers;
        }

        [System.Serializable]
        public class Data
        {
            public float dashSpeed = 10;
            public AudioClip dashSFX;
            public SpriteAnimation animation;
            public LayerMask dashMask;
            public float duration = 0.25f;
            public float cooldown = 0.25f;
        }

        protected override bool CanEnterState()
        {
            return InputManager.Instance.input.Game.Dash.WasPressedThisFrame() && InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.col.excludeLayers = initialExcludeLayers | data.dashMask;
            components.health.canTakeDamage = false;
            target.DisablePosition();

            components.animator.Play(data.animation);

            dashDir = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
            SFXManager.Instance.PlaySFXClip(data.dashSFX, components.movement.transform.position);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(dashDir * data.dashSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            components.health.canTakeDamage = true;
            components.col.excludeLayers = initialExcludeLayers;
            target.EnablePosition();
        }
    }
}
