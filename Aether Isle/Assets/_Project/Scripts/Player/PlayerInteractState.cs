using SpatialPartition;
using StateTree;
using System;
using UnityEngine;

namespace Game
{
    public class PlayerInteractState : State
    {
        Data data;
        ActorComponents components;
        int? lockDetph;

        InteractableSpatialManager interactableSpatialManager;
        IInteractable currentInteractable;

        public PlayerInteractState(Data data, ActorComponents components, int? lockDepth, Node child = null) : base(child)
        {
           this.data = data;
            this.components = components;
        }

        [Serializable]
        public class Data
        {
            public float radius = 2;
        }

        protected override void Setup()
        {
            base.Setup();

            interactableSpatialManager = InteractableSpatialManager.Instance;

            rootState.OnUpdateState += AlwaysUpdate;
        }

        protected override bool CanEnterState()
        {
            return currentInteractable != null && InputManager.Instance.input.Game.Interact.WasPerformedThisFrame();
        }

        private void AlwaysUpdate()
        {
            if (isActive) // We don't want to update the interactable when interacting
                return;

            IInteractable newInteractable = interactableSpatialManager.FindClosestEntryInRadius(components.transform.position, data.radius, InteractableFilter);

            if (newInteractable != currentInteractable)
            {
                currentInteractable?.Material.SetFloat("_OutlineFactor", 0);
                currentInteractable = newInteractable;
                currentInteractable?.Material.SetFloat("_OutlineFactor", 1);
            }
        }

        private bool InteractableFilter(IInteractable i)
        {
            return i.CanInteract();
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(lockDetph, true);

            currentInteractable.EnterInteract(components);
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            currentInteractable.UpdateInteract(components);

            if (!currentInteractable.CanContinue())
                LockSuperStates(lockDetph, false);
        }

        protected override void ExitState()
        {
            base.ExitState();

            currentInteractable.ExitInteract(components);
        }
    }
}
