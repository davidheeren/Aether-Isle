
namespace StateTree
{
    public class LockDurationModifier : Modifier
    {
        int? depth;
        bool canReenter;

        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)
        bool isLocked = false;

        public LockDurationModifier(float? delay, int? depth, Node child) : this(delay, depth, false, child) { }
        public LockDurationModifier(float? delay, int? depth, bool canReenter, Node child) : base(null, child)
        {
            this.depth = depth;
            this.canReenter = canReenter;

            if (delay != null)
            {
                timer = new SimpleTimer(delay.Value);
                timer.Stop();
            }
        }

        protected override void Setup()
        {
            base.Setup();
            firstChildState.canReenter = canReenter;
        }

        protected override void EnterChildState()
        {
            if (timer != null) timer.Reset();

            LockParentStates(depth, true);
            isLocked = true;
        }

        protected override void ExitChildState()
        {
            if (timer != null)
            {
                timer.Stop();

                // In case a higher state than the depth was changed
                if (isLocked)
                {
                    LockParentStates(depth, false);
                }
            }
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void UpdateChildState()
        {
            if (timer == null) return;

            if (timer.isDone && isLocked)
            {
                LockParentStates(depth, false);
            }
        }
    }
}
