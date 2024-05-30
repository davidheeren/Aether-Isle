
namespace StateTree
{
    public class LockModifier : Modifier
    {
        Timer timer;
        bool isLocked = false;

        public LockModifier(float delay, Node child) : base(null, child)
        {
            timer = new Timer(delay);
            timer.Stop();
        }

        protected override void EnterChildState()
        {
            timer.Reset();
            LockAllParentStates(true);
            isLocked = true;

            /*
            foreach (State par in parentStates)
            {
                Debug.Log("Locked parent: " + par.name);
            }
            */
        }

        protected override void ExitChildState()
        {
            timer.Stop();
        }

        public override State Evaluate()
        {
            return child.Evaluate();
        }

        protected override void UpdateChildState()
        {
            if (timer.isDone && isLocked)
            {
                LockAllParentStates(false);
                //Debug.Log("Unlocked");
            }
        }
    }
}
