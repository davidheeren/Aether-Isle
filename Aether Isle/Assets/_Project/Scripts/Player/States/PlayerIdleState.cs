using SpriteAnimator;
using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerIdleState : State
    {
        Data data;
        CharacterComponents components;

        public PlayerIdleState(Data data, CharacterComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;
        }

        [System.Serializable]
        public class Data
        {
            public SpriteAnimation animation;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play(data.animation);
        }
    }
}
