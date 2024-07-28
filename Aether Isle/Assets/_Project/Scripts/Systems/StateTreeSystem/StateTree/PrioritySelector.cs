
namespace StateTree
{
    public class PrioritySelector : Node
    {
        // TODODODODOODODODD
        Node[] _children; // "_" Because we already have a children var in Node

        public PrioritySelector Create(Node[] _children)
        {
            CreateNode();

            this._children = _children;

            return this;
        }

        protected override void SetChildrenParentRelationships()
        {
            foreach (Node child in _children)
            {
                AddChild(child);
            }
        }

        public override State Evaluate() // Returns the highest priority state that is not null
        {
            if (children.Count == 0)
                return null;

            State highestState = children[0].Evaluate();

            for (int i = 1; i < children.Count; i++)
            {
                State currentState = children[i].Evaluate();

                if (currentState != null)
                {
                    if (highestState == null)
                    {
                        highestState = currentState;
                    }
                    else
                    {
                        if (highestState.Priority() < currentState.Priority())
                        {
                            highestState = currentState;
                        }
                    }
                }
            }

            return highestState;
        }
    }
}
