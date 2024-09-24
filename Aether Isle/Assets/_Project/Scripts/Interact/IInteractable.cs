using UnityEngine;

namespace Game
{
    public interface IInteractable
    {
        public Material Material { get; }
        public Vector2 Position { get; }

        public bool CanInteract();
        public void Interact(CharacterComponents playerComponents);
        public bool CanContinue();
        public void UpdateInteract();
    }
}
