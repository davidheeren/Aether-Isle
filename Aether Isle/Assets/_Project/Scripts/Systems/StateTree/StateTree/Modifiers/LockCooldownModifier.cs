
using UnityEngine;

namespace StateTree
{
    public class LockCooldownModifier : Modifier
    {
        int? depth;

        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)
        bool isLocked = false;
        bool alreadyLockedOnce = false;


        public LockCooldownModifier(float? delay, int? depth, Node child) : base(child)
        {
            this.depth = depth;

            if (delay != null)
            {
                timer = new SimpleTimer(delay.Value);
                timer.Stop();
            }
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void EnterSubState()
        {
            alreadyLockedOnce = false;
        }

        protected override void PreExitSubState()
        {
            timer?.Reset();

            if (!alreadyLockedOnce)
            {
                subState.LockSuperStates(depth, true);
                isLocked = true;
                alreadyLockedOnce = true;
            }
        }

        protected override void UpdateSubState()
        {
            if (timer == null)
                return;

            if (timer.isDone && isLocked)
            {
                subState.LockSuperStates(depth, false);
                isLocked = false;
            }
        }

        protected override void ExitSubState()
        {
            if (isLocked)
            {
                subState.LockSuperStates(depth, false);
                isLocked = false;
            }
        }
    }
}
