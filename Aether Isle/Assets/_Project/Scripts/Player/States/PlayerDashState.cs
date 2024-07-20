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

        public PlayerDashState Create(CharacterComponents components, Node child = null)
        {
            CreateState(child);

            this.components = components;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.col.excludeLayers = dashMask;
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
            components.col.excludeLayers = new LayerMask();
        }
    }
}
