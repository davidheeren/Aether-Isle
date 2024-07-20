using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerAttackState : State
    {
        [SerializeField] float moveAttackDirSpeed = 1f;
        [SerializeField] CollisionDamage attackPrefab;
        [SerializeField] AudioClip attackSFX;

        CharacterComponents components;
        PlayerAimDirection aim;

        Vector2 initialAimDir;

        public PlayerAttackState Create(CharacterComponents components, PlayerAimDirection aim, Node child = null)
        {
            CreateState(child);

            this.components = components;
            this.aim = aim;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            initialAimDir = aim.aimDir;

            CollisionDamage obj = GameObject.Instantiate(attackPrefab, components.transform.position + (Vector3)aim.aimDir * 0.75f, Quaternion.Euler(0, 0, Mathf.Atan2(initialAimDir.y, initialAimDir.x) * Mathf.Rad2Deg - 90));
            obj.SetCollider(components.col);
            SFXManager.Instance.PlaySFXClip(attackSFX, components.transform.position);
            components.animator.Play("Attack", -1, 0); // Resets anim even if already playing
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(initialAimDir * moveAttackDirSpeed);
        }
    }
}
