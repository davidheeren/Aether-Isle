using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerAttackState : State
    {
        Data data;
        ActorComponents components;

        Vector2 dir;

        public PlayerAttackState(Data data, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;

            throw new System.Exception("Should not be using this State");
        }

        [System.Serializable]
        public class Data
        {
            public float moveAttackDirSpeed = 1f;
            public ProjectileDamage projectile;
            public LayerMask damageMask;
            public AudioClip attackSFX;
            public SpriteAnimation animation;

            public float duration = 0.25f;
            public float cooldown = 0.5f;
        }

        protected override bool CanEnterState()
        {
            return InputManager.Instance.input.Game.Use.WasPressedThisFrame();
        }

        protected override void EnterState()
        {
            base.EnterState();

            //dir = aimDirection.AimDirection;

            //data.projectile.Spawn(components, data.damageMask, components.transform.position + (Vector3)dir * 0.75f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            SFXManager.Instance.PlaySFXClip(data.attackSFX, components.transform.position);
            components.animator.Play(data.animation);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(dir * data.moveAttackDirSpeed);
        }
    }
}
