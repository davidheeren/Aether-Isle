using SpriteAnimator;
using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerAttackState : State
    {
        [SerializeField] float moveAttackDirSpeed = 1f;
        [SerializeField] DamageProjectile projectile;
        [SerializeField] LayerMask damageMask;
        [SerializeField] AudioClip attackSFX;
        [SerializeField] SpriteAnimation animation;

        CharacterComponents components;
        PlayerAimDirection aim;

        Vector2 initialAimDir;

        private PlayerAttackState() : base(null) { }
        public PlayerAttackState Init(CharacterComponents components, PlayerAimDirection aim, Node child = null)
        {
            InitializeState(child);
            this.components = components;
            this.aim = aim;

            return this;
        }

        protected override bool CanEnterState()
        {
            return InputManager.Instance.input.Game.Attack.WasPressedThisFrame();
        }

        protected override void EnterState()
        {
            base.EnterState();

            initialAimDir = aim.aimDir;

            projectile.Spawn(components.col, damageMask, components.transform.position + (Vector3)aim.aimDir * 0.75f, Mathf.Atan2(initialAimDir.y, initialAimDir.x) * Mathf.Rad2Deg - 90);
            SFXManager.Instance.PlaySFXClip(attackSFX, components.transform.position);
            components.animator.Play(animation);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(initialAimDir * moveAttackDirSpeed);
        }
    }
}
