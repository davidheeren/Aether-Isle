
namespace StateTree
{
    public class LockDurationModifier : Modifier
    {
        int? depth;

        Timer timer; // Timer is null if the delay is null (infinite)
        bool isLocked = false;

        public LockDurationModifier(float? delay, int? depth, Node child) : base(null, child)
        {
            this.depth = depth;

            if (delay != null)
            {
                timer = new Timer(delay.Value);
                timer.Stop();
            }
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
