
namespace StateTree
{
    public class LockIfModifier : Modifier
    {
        Condition condition;
        int? depth;

        bool isLocked = false;

        public LockIfModifier(Condition condition, int? depth, Node child) : base(null, child)
        {
            this.condition = condition;
            this.depth = depth;
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void UpdateChildState()
        {
            if (condition.Calculate() && !isLocked)
            {
                LockParentStates(depth, true);
            }

            if (!condition.Calculate() && isLocked)
            {
                LockParentStates(depth, false);
            }
        }
    }
}
