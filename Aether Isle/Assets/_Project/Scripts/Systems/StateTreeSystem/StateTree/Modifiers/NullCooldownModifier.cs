using UnityEngine;

namespace StateTree
{
    public class NullCooldownModifier : Modifier
    {
        Timer timer; // Timer is null if the delay is null (infinite)
        bool hasEnteredSateOnce;

        public NullCooldownModifier(float? delay, Node child) : base(null, child)
        {
            if (delay != null)
            {
                timer = new Timer(delay.Value);
                timer.ForceDone();
            }
        }

        protected override void EnterChildState()
        {
            if (timer != null) timer.ForceDone();
        }

        protected override void ExitChildState()
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
