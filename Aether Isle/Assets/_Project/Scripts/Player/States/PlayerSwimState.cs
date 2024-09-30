using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    public class PlayerSwimState : State
    {
        Data data;
        ObjectStats stats;
        ActorComponents components;

        public PlayerSwimState(Data data , ObjectStats stats, ActorComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.stats = stats;
            this.components = components;
        }

        [System.Serializable]
        public class Data
        {
            public StatModifier speedModifier;
            public GameObject aimGraphic;
            public AudioClip splashEnterSFX;
            public AudioClip splashExitSFX;
        }

        protected override void EnterState()
        {
            base.EnterState();

            stats.AddModifier(data.speedModifier);
            data.aimGraphic.SetActive(false);

            SFXManager.Instance.PlaySFXClip(data.splashEnterSFX, components.movement.transform.position);
        }

        protected override void ExitState()
        {
            base.ExitState();

            stats.RemoveModifier(data.speedModifier);
            SFXManager.Instance.PlaySFXClip(data.splashExitSFX, components.movement.transform.position);

            data.aimGraphic.SetActive(true);
        }
    }
}
