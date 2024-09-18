using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerAttackState : State
    {
        Data data;
        CharacterComponents components;
        PlayerAimDirection aim;

        Vector2 initialAimDir;

        public PlayerAttackState(Data data, CharacterComponents components, PlayerAimDirection aim, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
            this.aim = aim;
        }

        [System.Serializable]
        public class Data
        {
            public float moveAttackDirSpeed = 1f;
            public DamageProjectile projectile;
            public LayerMask damageMask;
            public AudioClip attackSFX;
            public SpriteAnimation animation;

            public float duration = 0.25f;
            public float cooldown = 0.5f;
        }

        protected override bool CanEnterState()
        {
            return InputManager.Instance.input.Game.Attack.WasPressedThisFrame();
        }

        protected override void EnterState()
        {
            base.EnterState();

            initialAimDir = aim.aimDir;

            data.projectile.Spawn(components.col, data.damageMask, components.transform.position + (Vector3)aim.aimDir * 0.75f, Mathf.Atan2(initialAimDir.y, initialAimDir.x) * Mathf.Rad2Deg - 90, components.transform);
            SFXManager.Instance.PlaySFXClip(data.attackSFX, components.transform.position);
            components.animator.Play(data.animation);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(initialAimDir * data.moveAttackDirSpeed);
        }
    }
}
