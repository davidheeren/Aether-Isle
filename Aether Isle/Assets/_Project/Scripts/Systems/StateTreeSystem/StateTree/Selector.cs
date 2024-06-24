namespace StateTree
{
    public class Selector : Node
    {
        Node[] _children; // "_" Because we already have a children var in Node

        public Selector(Node[] _children) : base(null)
        {
            this._children = _children;
        }

        public override State Evaluate() // Goes through each child and returns the first one that is not null
        {
            State state = null;
            foreach (Node child in children)
            {
                state = child.Evaluate();

                if (state != null)
                    break;
            }
            return state;
        }

        protected override void SetChildrenParentRelationships()
        {
            foreach (Node child in _children)
            {
                AddChild(child);
            }
        }
    }
}
