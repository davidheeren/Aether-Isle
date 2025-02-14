using SpatialPartition;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class InteractableMB : MonoBehaviour, IInteractable
    {
        protected SpriteRenderer spriteRenderer;

        public Material Material => spriteRenderer.material;
        public Vector2 Position => transform.position;
        public virtual bool Moveable => false;
        public int Layer => gameObject.layer;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            InteractableSpatialManager.Instance.Add(this);
        }

        private void OnDisable()
        {
            if (InteractableSpatialManager.TryGetInstance(out InteractableSpatialManager instance))
                instance.Remove(this);
        }

        public virtual bool CanInteract() => true;

        public virtual bool CanContinue() => false;

        public virtual void EnterInteract(ActorComponents components) { }

        public virtual void UpdateInteract(ActorComponents components) { }

        public virtual void ExitInteract(ActorComponents components) { }
    }
}
