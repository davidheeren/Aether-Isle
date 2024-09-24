
namespace StateTree
{
    public abstract class Task : Node
    {
        Node child;

        public Task(Node child)
        {
            this.child = child;
        }

        public override State Evaluate()
        {
            DoTask();
            return child.Evaluate();
        }

        protected override void SetChildrenParentRelationships() => AddChildren(child);

        protected abstract void DoTask();
    }
}
