using System;

namespace StateTree
{
    [Serializable]
    public abstract class Task : Node
    {
        Node child;

        protected void CreateTask(Node child)
        {
            CreateNode();
            this.child = child;
        }

        public override State Evaluate()
        {
            DoTask();
            return child.Evaluate();
        }

        protected override void SetChildrenParentRelationships()
        {
            AddChild(child);
        }

        protected abstract void DoTask();
    }
}
