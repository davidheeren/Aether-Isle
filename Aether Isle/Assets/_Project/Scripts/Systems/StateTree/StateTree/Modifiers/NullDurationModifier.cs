
namespace StateTree
{
    public class NullDurationModifier : Modifier
    {
        SimpleTimer timer; // SimpleTimer is null if the delay is null (infinite)

        public NullDurationModifier Create(float delay, Node child)
        {
            CreateModifier(child);

            timer = new SimpleTimer(delay);
            timer.Stop();

            return this;
        }

        protected override void EnterSubState()
        {
            timer.Reset();
        }

        protected override void ExitSubState()
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
