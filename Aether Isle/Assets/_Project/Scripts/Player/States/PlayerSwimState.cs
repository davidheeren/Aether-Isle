using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PlayerSwimState : State
    {
        [SerializeField] GameObject aimGraphic;
        [SerializeField] AudioClip splashEnterSFX;
        [SerializeField] AudioClip splashExitSFX;

        CharacterComponents components;

        private PlayerSwimState() : base(null) { }
        public PlayerSwimState Init(CharacterComponents components, Node child = null)
        {
            InitializeState(child);
            this.components = components;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            aimGraphic.SetActive(false);

            SFXManager.Instance.PlaySFXClip(splashEnterSFX, components.movement.transform.position);
        }

        protected override void ExitState()
        {
            base.ExitState();

            SFXManager.Instance.PlaySFXClip(splashExitSFX, components.movement.transform.position);

            aimGraphic.SetActive(true);
        }
    }
}
