using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerIdleState : State
    {
        CharacterComponents components;

        public PlayerIdleState Create(CharacterComponents components, Node child = null)
        {
            CreateState(child);

            this.components = components;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play("Idle");
        }
    }
}
