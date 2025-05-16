using SpatialPartition;
using UnityEngine;

namespace Game
{
    public interface IInteractable : ISpatialGridEntry
    {
        public Material Material { get; }

        public bool CanInteract();
        public bool CanContinue();
        public void EnterInteract(ActorComponents components);
        public void UpdateInteract(ActorComponents components);
        public void ExitInteract(ActorComponents components);
    }
}
