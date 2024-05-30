using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class Modifier : Node
    {
        protected Node child;

        protected State firstChildState;

        protected List<State> parentStates = new List<State>(); // All the states "above" this modifier

        public Modifier(string copyJson, Node child) : base(copyJson)
        {
            this.child = child;
        }

        protected override void Setup()
        {
            base.Setup(); // Just to print names 

            firstChildState = GetFirstChildState(this);

            // I don't think we have to unsubscribe
            firstChildState.OnEnterState += EnterChildState;
            firstChildState.OnUpdateState += UpdateChildState;
            firstChildState.OnExitState += ExitChildState;

            SetAllParentStates(this);
        }

        protected virtual void EnterChildState() { }
        protected virtual void UpdateChildState() { }
        protected virtual void ExitChildState() { }


        public abstract override State Evaluate();

        protected override void SetChildrenParentRelationships()
        {
            SetupChild(child);
        }

        State GetFirstChildState(Node startNode) // Excludes the start node
        {
            if (startNode.children == null)
            {
                Debug.LogError("Modifier does not have a child firstState");
                return null;
            }
            if (startNode.children.Count != 1)
            {
                Debug.LogError("Found more than one child below this modifier");
                return null;
            }

            Node firstChild = startNode.children[0];

            if (firstChild is State)
                return (State)firstChild;

            return GetFirstChildState(firstChild);
        }

        State GetFirstParentState(Node startNode) // Excludes the start node
        {
            if (startNode.parent == null) 
                return null;

            if (startNode.parent is State)
                return (State)startNode.parent;

            return GetFirstParentState(startNode.parent);
        }

        void SetAllParentStates(Node startNode) // Sets the parent nodes
        {
            State firstState = GetFirstParentState(startNode);

            if (firstState != null)
            {
                parentStates.Add(firstState);
                SetAllParentStates(firstState);
            }
        }

        /// <summary>
        /// Locks or unlocks all states "above" it
        /// </summary>
        /// <param name="canEvaluate"></param>
        protected void LockAllParentStates(bool isLock)
        {
            foreach (State state in parentStates)
            {
                state.isLocked = isLock;
            }
        }
    }
}
