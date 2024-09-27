
namespace StateTree
{
    public class LockIfModifier : Modifier
    {
        Condition condition;
        int? depth;

        bool isLocked = false;
        public LockIfModifier(Condition condition, int? depth, Node child) : base(child)
        {
            this.condition = condition;
            this.depth = depth;
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void UpdateSubState()
        {
            if (condition.Calculate() && !isLocked)
            {
                subState.LockSuperStates(depth, true);
            }

            if (!condition.Calculate() && isLocked)
            {
                subState.LockSuperStates(depth, false);
            }
        }
    }
}
