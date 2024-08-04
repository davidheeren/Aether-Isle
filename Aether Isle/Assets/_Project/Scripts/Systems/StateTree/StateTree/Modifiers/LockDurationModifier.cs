
namespace StateTree
{
    public class LockDurationModifier : Modifier
    {
        int? depth;
        bool? canReenter;

        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)
        bool isLocked = false;

        public LockDurationModifier Create(float? delay, int? depth, Node child)
        {
            return Create(delay, depth, null, child);
        }

        public LockDurationModifier Create(float? delay, int? depth, bool? canReenter, Node child)
        {
            CreateModifier(child);

            this.depth = depth;
            this.canReenter = canReenter;

            if (delay != null)
            {
                timer = new SimpleTimer(delay.Value);
                timer.Stop();
            }

            return this;
        }

        protected override void Setup()
        {
            base.Setup();

            if (canReenter != null)
                subState.canReenter = canReenter.Value;
        }

        protected override void EnterSubState()
        {
            if (timer != null) timer.Reset();

            subState.LockSuperStates(depth, true);
            isLocked = true;
        }

        protected override void ExitSubState()
        {
            if (timer != null)
            {
                timer.Stop();

                // In case a higher state than the depth was changed
                if (isLocked)
                {
                    subState.LockSuperStates(depth, false);
                    isLocked = false;
                }
            }
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void UpdateSubState()
        {
            if (timer == null) return;

            if (timer.isDone && isLocked)
            {
                subState.LockSuperStates(depth, false);
                isLocked = false;
            }
        }
    }
}
