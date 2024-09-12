using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerIdleState : State
    {
        [SerializeField] SpriteAnimation animation;
        CharacterComponents components;

        private PlayerIdleState() : base(null) { }
        public PlayerIdleState Init(CharacterComponents components, Node child = null)
        {
            InitializeState(child);
            this.components = components;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play(animation);
        }
    }
}
