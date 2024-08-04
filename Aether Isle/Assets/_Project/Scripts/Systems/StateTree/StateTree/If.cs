using UnityEngine;

namespace StateTree
{
    public class If : Node
    {
        Condition condition;
        Node child;

        public If Create(Condition condition, Node child)
        {
            CreateNode();

            this.condition = condition;
            this.child = child;

            if (child == null)
                Debug.LogError("If's child cannot be null");

            return this;
        }

        public override State Evaluate()
        {
            State state = null;

            if (condition.Calculate())
                state = child.Evaluate();

            return state;
        }

        protected override void SetChildrenParentRelationships()
        {
            AddChild(child);
        }
    }
}
