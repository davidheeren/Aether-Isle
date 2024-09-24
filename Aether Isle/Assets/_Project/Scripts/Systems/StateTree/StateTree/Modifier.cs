using UnityEngine;

namespace StateTree
{
    public abstract class Modifier : Node
    {
        protected Node child;

        protected State subState;

        public Modifier(Node child)
        {
            this.child = child;
        }

        protected override void Setup()
        {
            base.Setup(); // Just to print names 

            subState = GetFirstSubNode<State>();

            // I don't think we have to unsubscribe
            subState.OnPreExitState += PreExitSubState;

            subState.OnEnterState += EnterSubState;
            subState.OnUpdateState += UpdateSubState;
            subState.OnExitState += ExitSubState;
        }

        protected virtual void PreExitSubState() { }

        protected virtual void EnterSubState() { }
        protected virtual void UpdateSubState() { }
        protected virtual void ExitSubState() { }


        public abstract override State Evaluate();

        protected override void SetChildrenParentRelationships() => AddChildren(child);
    }
}
