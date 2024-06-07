using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class State : Node
    {
        Node child;

        State possibleSubState; // The new possible sub state
        State currentSubState; // The current sub state that is updated

        [NonSerialized] public bool isLocked = false; // Locks current sub state from changing
        [NonSerialized] public bool canReenter = false; // Exits then enters itself if locked and evaluated

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
                possibleSubState = child.Evaluate();

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

            if (!isLocked)
            {
                if (possibleSubState != currentSubState)
                {
                    currentSubState?.ExitStateWrapper();
                    currentSubState = possibleSubState;
                    currentSubState?.EnterStateWrapper();
                }
            }
            else if (currentSubState.canReenter && (possibleSubState == currentSubState))
            {
                currentSubState?.ExitStateWrapper();
                currentSubState?.EnterStateWrapper();
            }

            currentSubState?.UpdateStateWrapper();
        }

        public void FixedUpdateStateWrapper()
        {
            FixedUpdateState();
            currentSubState?.FixedUpdateStateWrapper();
        }

        public void ExitStateWrapper()
        {
            ExitState();
            OnExitState?.Invoke();

            currentSubState?.ExitStateWrapper();
            currentSubState = null;
            possibleSubState = null;
        }

        protected virtual void EnterState() { if (rootState.debugState) Debug.Log("Enter: " + name); }
        protected virtual void UpdateState() { }
        protected virtual void FixedUpdateState() { }
        protected virtual void ExitState() { if (rootState.debugState) Debug.Log("Exit: " + name); }


        protected override void SetChildrenParentRelationships()
        {
            SetupChild(child);
        }
    }
}
