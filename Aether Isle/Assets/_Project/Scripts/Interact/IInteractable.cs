using UnityEngine;

namespace Game
{
    public interface IInteractable
    {
        public Material Material { get; }
        public Vector2 Position { get; }

        public bool CanInteract();
        public void Interact(ActorComponents playerComponents);
        public bool CanContinue();
        public void UpdateInteract();
    }
}
