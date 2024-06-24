using StateTree;
using UnityEngine;

namespace Game
{
    public class PlayerIdleState : State
    {
        CharacterComponents components;

        public PlayerIdleState(CharacterComponents components, Node child = null) : base(null, child)
        {
            this.components = components;
        }

        protected override void EnterState()
        {
            base.EnterState();

            components.animator.Play("Idle");
        }
    }
}
