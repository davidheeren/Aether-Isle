
namespace StateTree
{
    public abstract class Condition : Node
    {
        Node child;

        public Condition(Node child)
        {
            this.child = child;
        }

        protected override void SetChildrenParentRelationships()
        {
            AddChildren(child);
        }

        public override State Evaluate()
        {
            if (Calculate())
                return child.Evaluate();

            return null;
        }

        public abstract bool Calculate();
    }
}
