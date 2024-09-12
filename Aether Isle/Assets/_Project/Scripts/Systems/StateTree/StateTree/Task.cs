using System;

namespace StateTree
{
    [Serializable]
    public abstract class Task : Node
    {
        Node child;

        public Task(Node child) => InitializeTask(child);

        protected void InitializeTask(Node child) => this.child = child;

        public override State Evaluate()
        {
            DoTask();
            return child.Evaluate();
        }

        protected override void SetChildrenParentRelationships() => AddChild(child);

        protected abstract void DoTask();
    }
}
