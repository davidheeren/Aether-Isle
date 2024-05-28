
namespace StateTree
{
    public class If : Node
    {
        Node child;

        public If(Node child) : base(null)
        {
            this.child = child;
        }

        public override State Evaluate()
        {
            State state = child.Evaluate();

            if (state != null)
                return state;

            return null;
        }

        protected override void SetChildrenParentRelationships()
        {
            SetupChild(child);
        }
    }
}
