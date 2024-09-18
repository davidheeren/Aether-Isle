using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerSwimState : State
    {
        Data data;
        CharacterComponents components;

        public PlayerSwimState(Data data, CharacterComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
        }

        [System.Serializable]
        public class Data
        {
            public GameObject aimGraphic;
            public AudioClip splashEnterSFX;
            public AudioClip splashExitSFX;
        }

        protected override void EnterState()
        {
            base.EnterState();

            data.aimGraphic.SetActive(false);

            SFXManager.Instance.PlaySFXClip(data.splashEnterSFX, components.movement.transform.position);
        }

        protected override void ExitState()
        {
            base.ExitState();

            SFXManager.Instance.PlaySFXClip(data.splashExitSFX, components.movement.transform.position);

            data.aimGraphic.SetActive(true);
        }
    }
}
