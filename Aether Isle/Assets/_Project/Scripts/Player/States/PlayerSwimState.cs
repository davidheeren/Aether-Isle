using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerSwimState : State
    {
        [SerializeField] float swimSpeed = 2;
        [SerializeField] GameObject aimGraphic;
        [SerializeField] AudioClip splashEnterSFX;
        [SerializeField] AudioClip splashExitSFX;

        CharacterComponents compoenents;

        private PlayerSwimState() : base(null, null) { }
        public PlayerSwimState(string copyJson, CharacterComponents components, Node child = null) : base(copyJson, child)
        {
            this.compoenents = components;
        }

        protected override void EnterState()
        {
            base.EnterState();

            aimGraphic.SetActive(false);

            SFXManager.Instance.PlaySFXClip(splashEnterSFX, compoenents.movement.transform.position);

            compoenents.animator.Play("Swim");
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            compoenents.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * swimSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            SFXManager.Instance.PlaySFXClip(splashExitSFX, compoenents.movement.transform.position);

            aimGraphic?.SetActive(true);
        }
    }
}
