﻿namespace StateTree
{
    public class NullCooldownModifier : Modifier
    {
        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)
        bool hasEnteredSateOnce; // If the delay is infinite, we can still enter the state once

        public NullCooldownModifier(float? delay, Node child) : base(child)
        {
            if (delay != null)
            {
                timer = new SimpleTimer(delay.Value);
                timer.ForceDone();
            }
        }

        protected override void EnterSubState()
        {
            if (timer != null) timer.ForceDone();
        }

        protected override void ExitSubState()
        {
            if (timer != null) timer.Reset();
        }

        public override State Evaluate()
        {
            if (timer != null)
            {
                if (!timer.isDone)
                    return null;
            }
            else if (hasEnteredSateOnce)
                return null;

            hasEnteredSateOnce = true;
            return child.Evaluate();
        }
    }
}
