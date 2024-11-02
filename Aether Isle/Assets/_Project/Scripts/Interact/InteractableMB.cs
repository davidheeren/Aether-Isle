using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class InteractableMB : MonoBehaviour, IInteractable
    {
        protected SpriteRenderer spriteRenderer;
        protected ActorComponents playerComponents;

        public Material Material => spriteRenderer.material;
        public Vector2 Position => transform.position;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual bool CanContinue() => false;

        public virtual bool CanInteract() => true;

        public virtual void Interact(ActorComponents playerComponents)
        {
            this.playerComponents = playerComponents;
        }

        public virtual void UpdateInteract() { }

        public virtual void ExitInteract() { }
    }
}
