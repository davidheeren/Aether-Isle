
namespace StateTree
{
    public class LockIfModifier : Modifier
    {
        Condition condition;
        int? depth;
        bool? canReenter;

        bool isLocked = false;

        public LockIfModifier Create(Condition condition, int? depth, Node child)
        {
            return Create(condition, depth, null, child);
        }

        public LockIfModifier Create(Condition condition, int? depth, bool? canReenter, Node child)
        {
            CreateModifier(child);

            this.condition = condition;
            this.depth = depth;
            this.canReenter = canReenter;

            return this;
        }

        protected override void Setup()
        {
            base.Setup();

            if (canReenter != null)
                subState.canReenter = canReenter.Value;
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
