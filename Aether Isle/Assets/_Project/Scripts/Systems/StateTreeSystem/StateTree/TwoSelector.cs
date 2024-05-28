using System;

namespace StateTree
{
    [Serializable]
    public class TwoSelector : Node
    {
        Node child1;
        Node child2;

        public TwoSelector(Node child1, Node child2) : this(null, child1, child2) { }
        public TwoSelector(string copyJson, Node child1, Node child2) : base(copyJson)
        {
            this.child1 = child1;
            this.child2 = child2;
        }

        public override State Evaluate() // If state1 is null then return state2
        {
            State state = child1.Evaluate();

            if (state != null)
                return state;

            return child2.Evaluate();
        }

        protected override void SetChildrenParentRelationships()
        {
            SetupChild(child1);
            SetupChild(child2);
        }
    }
}
