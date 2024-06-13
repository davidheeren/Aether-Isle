
namespace StateTree
{
    public class LockIfModifier : Modifier
    {
        Condition condition;
        int? depth;
        bool canReenter;

        bool isLocked = false;

        public LockIfModifier(Condition condition, int? depth, Node child) : this(condition, depth, false, child) { }
        public LockIfModifier(Condition condition, int? depth, bool canReenter, Node child) : base(null, child)
        {
            this.condition = condition;
            this.depth = depth;
            this.canReenter = canReenter;
        }

        protected override void Setup()
        {
            base.Setup();
            subState.canReenter = canReenter;
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
