
namespace StateTree
{
    public class LockIfModifier : Modifier
    {
        Condition condition;
        bool isLocked = false;

        public LockIfModifier(Condition condition, Node child) : base(null, child)
        {
            this.condition = condition;
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void UpdateChildState()
        {
            if (condition.Calculate() && !isLocked)
            {
                LockAllParentStates(true);
            }

            if (!condition.Calculate() && isLocked)
            {
                LockAllParentStates(false);
            }
        }
    }
}
