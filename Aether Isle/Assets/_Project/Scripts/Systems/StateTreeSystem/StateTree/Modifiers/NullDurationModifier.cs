
namespace StateTree
{
    public class NullDurationModifier : Modifier
    {
        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)

        public NullDurationModifier(float delay, Node child) : base(null, child)
        {
            timer = new SimpleTimer(delay);
            timer.Stop();
        }

        protected override void EnterChildState()
        {
            timer.Reset();
        }

        protected override void ExitChildState()
        {
            timer.Stop();
        }

        public override State Evaluate()
        {
            if (timer.isDone)
                return null;

            return child.Evaluate();
        }
    }
}
