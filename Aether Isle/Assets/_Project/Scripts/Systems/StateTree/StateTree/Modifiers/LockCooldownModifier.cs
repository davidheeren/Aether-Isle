
using UnityEngine;

namespace StateTree
{
    public class LockCooldownModifier : Modifier
    {
        int? depth;
        bool? canReenter;

        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)
        bool isLocked = false;
        bool alreadyLockedOnce = false;


        public LockCooldownModifier Create(float? delay, int? depth, Node child)
        {
            return Create(delay, depth, null, child);
        }

        public LockCooldownModifier Create(float? delay, int? depth, bool? canReenter, Node child)
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
