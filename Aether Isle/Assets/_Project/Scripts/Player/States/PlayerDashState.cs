using UnityEngine;
using StateTree;
using System;
using Utilities;

namespace Game
{
    [Serializable]
    public class PlayerDashState : State
    {
        [SerializeField] float dashSpeed = 10;
        [SerializeField] AudioClip dashSFX;

        Movement movement;
        Collider2D collider;
        Health health;

        Vector2 dashDir;
        LayerMask enemyMask;

        private PlayerDashState() : base(null, null) { }
        public PlayerDashState(string copyJson, Movement movement, Collider2D collider, Health health, Node child = null) : base(copyJson, child)
        {
            this.movement= movement;
            this.collider= collider;
            this.health= health;

            enemyMask = enemyMask.GetLayerMaskByName("Enemy");
        }

        protected override void EnterState()
        {
            base.EnterState();

            collider.excludeLayers = enemyMask;
            health.canTakeDamage = false;

            dashDir = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
            SFXManager.Instance.PlaySFXClip(dashSFX, movement.transform.position);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            movement.Move(dashDir * dashSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            health.canTakeDamage = true;
            collider.excludeLayers =new LayerMask();
        }
    }
}
