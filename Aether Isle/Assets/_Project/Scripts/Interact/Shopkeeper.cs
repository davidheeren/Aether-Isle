using UnityEngine;
using Utilities;

namespace Game
{
    public class Shopkeeper : InteractableMB
    {
        [SerializeField] float interactTime;

        Timer t;

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
