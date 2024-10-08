﻿
namespace StateTree
{
    public class LockDurationModifier : Modifier
    {
        int? depth;
        bool? canReenter;

        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)
        bool isLocked = false;

        public LockDurationModifier(float? delay, int? depth, Node child) : base(child)
        {
            this.depth = depth;

            if (delay != null)
            {
                timer = new SimpleTimer(delay.Value);
                timer.Stop();
            }
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
