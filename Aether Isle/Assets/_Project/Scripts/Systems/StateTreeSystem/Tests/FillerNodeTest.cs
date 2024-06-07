using UnityEngine;

namespace StateTree.Test
{
    public class FillerNodeTest : Node
    {
        Node child;

        public FillerNodeTest(Node child) : base(null)
        {
            this.child = child;
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void SetChildrenParentRelationships()
        {
            SetupChild(child);
        }
    }
}
