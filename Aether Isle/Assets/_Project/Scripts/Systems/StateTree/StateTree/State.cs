using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class State : Node
    {
        State possibleSubState; // The new possible sub state
        State currentSubState; // The current sub state that is updated

        protected List<State> superStates = new List<State>(); // All the states "above" this modifier

        Node child;

        [NonSerialized] public bool isLocked = false; // Locks current sub state from changing
        [NonSerialized] public bool canReenter = false; // Exits then enters itself if locked and evaluated

        public Action OnPreExitState;

        public Action OnEnterState;
        public Action OnUpdateState;
        public Action OnExitState;

        protected void CreateState(Node child)
        {
            CreateNode();
            this.child = child;
        }

        protected override void SetChildrenParentRelationships() => AddChild(child);

        protected override void Setup()
        {
            base.Setup();
            SetAllSuperStates(this);
        }

        public override State Evaluate() // Evaluate is called first, then UpdateStateWrapper
        {
            if (!CanEnterState())
                return null;

            if (child != null)
                possibleSubState = child.Evaluate();

            return this;
        }

        protected virtual bool CanEnterState() => true; // Override this in your custom State to have your own If Node in the state itself

        #region EnterUpdateExit
        public void EnterStateWrapper()
        {
            EnterState();
            OnEnterState?.Invoke();
            // We do not need to call the current sub states enter method because we set it to null on the exit so it will be called on the evaluate method
        }

        public virtual void UpdateStateWrapper()
        {
            UpdateState();
            OnUpdateState?.Invoke();

            SetCurrentSubState(possibleSubState, ref currentSubState);

            currentSubState?.UpdateStateWrapper();
        }

        public void FixedUpdateStateWrapper()
        {
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

        #region SuperStates
        State GetFirstSuperState(Node startNode) 
        {
            // Excludes the start node

            if (startNode.parent == null)
                return null;

            if (startNode.parent is State)
                return (State)startNode.parent;

            return GetFirstSuperState(startNode.parent);
        }

        void SetAllSuperStates(Node startNode)
        {
            State firstState = GetFirstSuperState(startNode);

            if (firstState != null)
            {
                superStates.Add(firstState);
                SetAllSuperStates(firstState);
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
        #endregion
    }
}
