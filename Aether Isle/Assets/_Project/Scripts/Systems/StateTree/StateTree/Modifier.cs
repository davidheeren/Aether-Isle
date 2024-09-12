using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class Modifier : Node
    {
        protected Node child;

        protected State subState;

        public Modifier(Node child) => InitializeModifier(child);

        protected void InitializeModifier(Node child) => this.child = child;

        protected override void Setup()
        {
            base.Setup(); // Just to print names 

            subState = GetFirstSubState(this);

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

        protected override void SetChildrenParentRelationships()
        {
            AddChild(child);
        }

        State GetFirstSubState(Node startNode) // Excludes the start node
        {
            if (startNode.children.Count == 0)
            {
                Debug.LogError("Modifier does not have a subState");
                return null;
            }
            else if (startNode.children.Count > 1)
            {
                Debug.LogError("Found more than one subState below this modifier");
                return null;
            }

            Node firstChild = startNode.children[0];

            if (firstChild is State)
                return (State)firstChild;

            return GetFirstSubState(firstChild);
        }
    }
}
