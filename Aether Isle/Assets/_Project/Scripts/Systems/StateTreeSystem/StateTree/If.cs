
namespace StateTree
{
    public class If : Node
    {
        Condition condition;
        Node child;

        public If(Condition condition, Node child) : base(null)
        {
            this.condition = condition;
            this.child = child;
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
            SetupChild(child);
        }
    }
}
