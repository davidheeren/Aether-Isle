
namespace StateTree
{
    public class NullTimerModifier : Modifier
    {
        Timer timer;

        public NullTimerModifier(float delay, Node child) : base(null, child)
        {
            timer = new Timer(delay);
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
            else
                return child.Evaluate();
        }
    }
}
