using StateTree;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class PlayerDashState : State
    {
        [SerializeField] float dashSpeed = 10;
        [SerializeField] AudioClip dashSFX;
        [SerializeField] LayerMask dashMask;

        CharacterComponents components;

        Vector2 dashDir;

        private PlayerDashState() : base(null, null) { }
        public PlayerDashState(string copyJson, CharacterComponents components, Node child = null) : base(copyJson, child)
        {
            this.components = components;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.collider.excludeLayers = dashMask;
            components.health.canTakeDamage = false;

            components.animator.Play("Dash");

            dashDir = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
            SFXManager.Instance.PlaySFXClip(dashSFX, components.movement.transform.position);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(dashDir * dashSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            components.health.canTakeDamage = true;
            components.collider.excludeLayers = new LayerMask();
        }
    }
}
