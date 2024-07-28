using UnityEngine;

namespace StateTree
{
    public class Sequence : Node
    {
        // Note: LockCooldownModifier does not work well with sequence

        // How to use: inside the sequence have the first child have an if and a lock, then the rest only have locks

        Node[] _children; // "_" Because we already have a children var in Node
        State superState;

        int currentChildIndex = 0;

        public Sequence Create(Node[] _children)
        {
            CreateNode();

            this._children = _children;

            return this;
        }

        protected override void SetChildrenParentRelationships()
        {
            foreach (Node child in _children)
            {
                AddChild(child);
            }
        }

        protected override void Setup()
        {
            base.Setup();

            superState = GetFirstSuperState(this);
            superState.OnEnterState += OnSuperStateEnter;
        }

        public override State Evaluate()
        {
            State output = children[currentChildIndex].Evaluate();

            if (!superState.isLocked && output != null)
                currentChildIndex = (currentChildIndex + 1) % children.Count;

            return output;
        }

        void OnSuperStateEnter()
        {
            currentChildIndex = 0;
        }
    }
}
