using System;

namespace StateTree
{
    [Serializable]
    public abstract class Task : Node
    {
        Node child;

        public Task(string copyJson, Node child) : base(copyJson)
        {
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
