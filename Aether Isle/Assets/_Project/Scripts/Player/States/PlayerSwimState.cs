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

        CharacterComponents components;

        public PlayerSwimState Create(CharacterComponents components, Node child = null)
        {
            CreateState(child);

            this.components = components;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            aimGraphic.SetActive(false);

            SFXManager.Instance.PlaySFXClip(splashEnterSFX, components.movement.transform.position);

            components.animator.Play("Swim");
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            components.movement.Move(InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * swimSpeed);
        }

        protected override void ExitState()
        {
            base.ExitState();

            SFXManager.Instance.PlaySFXClip(splashExitSFX, components.movement.transform.position);

            aimGraphic?.SetActive(true);
        }
    }
}
