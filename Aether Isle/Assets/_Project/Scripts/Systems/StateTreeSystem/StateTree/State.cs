using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class State : Node
    {
        Node child;

        State newSubState;
        State currentSubState;

        [NonSerialized] public bool isLocked = false;

        public event Action OnEnterState;
        public event Action OnUpdateState;
        public event Action OnExitState;

        public State(string copyJson, Node child) : base(copyJson)
        {
            this.child = child;
        }

        
        public override State Evaluate() // Evaluate is called first, then UpdateStateWrapper
        {
            if (child != null)
            {
                State potentialSubState = child.Evaluate();

                if (!isLocked)
                {
                    newSubState = potentialSubState;
                }
            }

            return this;
        }

        public void EnterStateWrapper()
        {
            EnterState();
            OnEnterState?.Invoke();
            // We do not need to call the current sub states enter method because we set it to null on the exit so it will be called on the evaluate method
        }

        public void UpdateStateWrapper()
        {
            UpdateState();
            OnUpdateState?.Invoke();

            if (newSubState != currentSubState)
            {
                currentSubState?.ExitStateWrapper();
                currentSubState = newSubState;
                currentSubState?.EnterStateWrapper();
            }

            currentSubState?.UpdateStateWrapper();
        }

        public void ExitStateWrapper()
        {
            ExitState();
            OnExitState?.Invoke();

            currentSubState?.ExitStateWrapper();
            currentSubState = null;
            newSubState = null;
        }

        protected virtual void EnterState() { if (rootState.debugState) Debug.Log("Enter: " + name); }
        protected virtual void UpdateState() { /*if (rootState.debugState) Debug.Log("Update: " + name);*/ }
        protected virtual void ExitState() { if (rootState.debugState) Debug.Log("Exit: " + name); }


        protected override void SetChildrenParentRelationships()
        {
            SetupChild(child);
        }
    }
}
