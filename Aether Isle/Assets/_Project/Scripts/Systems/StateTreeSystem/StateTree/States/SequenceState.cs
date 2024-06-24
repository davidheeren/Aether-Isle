using UnityEngine;

namespace StateTree
{
    public class SequenceState : State
    {
        //TODODDODODODODODODODOOODODOODD

        Node[] _children; // "_" Because we already have a children var in Node
        State[] currentSubStates;
        State[] possibleSubStates;

        public SequenceState(Node[] _children) : base(null, null)
        {
            this._children = _children;

            currentSubStates = new State[this._children.Length];
            possibleSubStates = new State[this._children.Length];
        }

        public override State Evaluate() // Goes through each child gets state
        {
            for (int i = 0; i < possibleSubStates.Length; i++)
            {
                possibleSubStates[i] = children[i].Evaluate();

                if (possibleSubStates[i] == null)
                {
                    return null;
                }
            }

            return this;
        }

        protected override void SetChildrenParentRelationships()
        {
            foreach (Node child in _children)
            {
                AddChild(child);
            }
        }

        public override void UpdateStateWrapper()
        {
            UpdateState();
            OnUpdateState?.Invoke();

            for (int i = 0; i < currentSubStates.Length;i++)
            {
                SetCurrentSubState(possibleSubStates[i], ref currentSubStates[i]);

                currentSubStates[i]?.UpdateStateWrapper();
            }
        }

        public override void ExitStateWrapper()
        {
            ExitState();
            OnExitState?.Invoke();

            for (int i = 0;i < currentSubStates.Length;i++)
            {
                currentSubStates[i]?.ExitStateWrapper();
                currentSubStates[i] = null;
                possibleSubStates[i] = null;
            }
        }
    }
}
