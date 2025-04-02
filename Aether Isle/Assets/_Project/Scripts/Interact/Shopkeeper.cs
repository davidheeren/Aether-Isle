using UnityEngine;
using Utilities;

namespace Game
{
    public class Shopkeeper : InteractableMB
    {
        [SerializeField] float interactTime;
        [SerializeField] float yOffset = -0.5f;

        Timer t;

        public override Vector2 Position => transform.position + Vector3.one * yOffset;

        protected override void Awake()
        {
            base.Awake();

            t = new Timer(interactTime).Stop();
        }

        public override bool CanContinue()
        {
            return !t.IsDone;
        }

        public override void EnterInteract(ActorComponents components)
        {
            base.EnterInteract(components);

            t.Reset();
        }
    }
}
