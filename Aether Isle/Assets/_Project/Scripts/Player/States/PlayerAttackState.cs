using UnityEngine;
using StateTree;
using System;

namespace Game
{
    [Serializable]
    public class PlayerAttackState : State
    {
        [SerializeField] float moveAttackDirSpeed = 1f;
        [SerializeField] GameObject attackPrefab;
        [SerializeField] AudioClip attackSFX;

        Transform playerT;
        PlayerAimDirection aim;
        Animator animator;
        Movement movement;

        Vector2 initialAimDir;

        private PlayerAttackState() : base(null, null) { }
        public PlayerAttackState(string copyJson, Transform playerT, PlayerAimDirection aim, Animator animator, Movement movement, Node child) : base(copyJson, child)
        {
            this.playerT = playerT;
            this.aim = aim;
            this.animator = animator;
            this.movement = movement;
        }

        protected override void EnterState()
        {
            base.EnterState();

            initialAimDir = aim.aimDir;

            GameObject.Instantiate(attackPrefab, playerT.position + (Vector3)aim.aimDir * 0.75f, Quaternion.Euler(0, 0, Mathf.Atan2(initialAimDir.y, initialAimDir.x) * Mathf.Rad2Deg));
            SFXManager.Instance.PlaySFXClip(attackSFX, playerT.position);
            animator.Play("Attack", -1, 0); // Resets anim even if already playing
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            movement.MoveVelocity(initialAimDir * moveAttackDirSpeed);
        }
    }
}
