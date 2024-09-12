using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class State : Node
    {
        // Edit Vars
        [NonSerialized] public bool isLocked = false; // Locks current sub state from changing
        [NonSerialized] public bool canReenter = false; // Exits then enters itself if locked and evaluated
        [NonSerialized] public bool enabled = true;
        [NonSerialized] public int priority;
        public State SetPriority(int priority) { this.priority = priority; return this; }
        public State SetEnabled(bool enabled) { this.enabled = enabled; return this; }
        public State SetCanReenter(bool canReenter) { this.canReenter = canReenter; return this; }

        // Readonly vars
        private State possibleSubState; // The new possible sub state that is set during Evaluate()
        private State currentSubState; // The current sub state that is updated
        private List<State> superStates; // All the states "above" this modifier
        private Node child;
        public int stateDepth { get; private set; }

        // Events
        public event Action OnPreExitState;
        public event Action OnEnterState;
        public event Action OnUpdateState;
        public event Action OnExitState;

        // Constructor
        public State(Node child) => InitializeState(child);

        protected void InitializeState(Node child) => this.child = child;

        protected override void SetChildrenParentRelationships() => AddChild(child);

        protected override void Setup()
        {
            base.Setup();
            superStates = GetSuperNodes<State>();
            stateDepth = superStates.Count;
        }

        public override State Evaluate() // Evaluate is called first, then UpdateStateWrapper
        {
            if (!CanEnterState())
                return null;

            if (child != null && enabled)
                possibleSubState = child.Evaluate();

            return this;
        }

        protected virtual bool CanEnterState() => true; // Override this in your custom State to have your own If Node in the state itself

        #region EnterUpdateExit Wrappers
        public void EnterStateWrapper()
        {
            EnterState();
            OnEnterState?.Invoke();
            // We do not need to call the current sub states enter method because we set it to null on the exit so it will be called on the evaluate method
        }

        public virtual void UpdateStateWrapper()
        {
            if (!enabled) return;

            UpdateState();
            OnUpdateState?.Invoke();

            SetCurrentSubState(possibleSubState, ref currentSubState);

            currentSubState?.UpdateStateWrapper();
        }

        public void FixedUpdateStateWrapper()
        {
            if (!enabled) return;

            FixedUpdateState();
            currentSubState?.FixedUpdateStateWrapper();
        }

        public virtual void ExitStateWrapper()
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
        #endregion

        protected void SetCurrentSubState(State _possibleSubState, ref State _currentSubState)
        {
            if (!isLocked && _possibleSubState != _currentSubState)
                _currentSubState?.OnPreExitState?.Invoke();

            if (!isLocked)
            {
                if (_possibleSubState != _currentSubState)
                {
                    _currentSubState?.ExitStateWrapper();
                    _currentSubState = _possibleSubState;
                    _currentSubState?.EnterStateWrapper();
                }
            }
            else if (_currentSubState.canReenter && (_possibleSubState == _currentSubState))
            {
                _currentSubState?.ExitStateWrapper();
                _currentSubState?.EnterStateWrapper();
            }
        }

        /// <summary>
        /// Locks or unlocks all states "above" it
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="isLock"></param>
        public void LockSuperStates(int? depth, bool isLock)
        {
            int lockDepth = superStates.Count;
            if (depth != null) lockDepth = depth.Value;

            for (int i = 0; i < lockDepth; ++i)
            {
                if (i > superStates.Count)
                {
                    Debug.LogWarning("Lock depth greater than parent states count");
                    break;
                }

                superStates[i].isLocked = isLock;
            }
        }
    }
}
