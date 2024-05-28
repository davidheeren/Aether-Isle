
using UnityEngine;

namespace StateTree
{
    public class LockTimerModifier : Modifier
    {
        Timer timer;
        bool isLocked = false;

        public LockTimerModifier(float delay, Node child) : base(null, child)
        {
            timer = new Timer(delay);
            timer.Stop();
        }

        protected override void EnterChildState()
        {
            Debug.LogError("ENTER");
            timer.Reset();
            LockAllParentStates(true);
            isLocked = true;


            foreach (State par in parentStates)
            {
                Debug.Log("Locked parent: " + par.name);
            }
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
                Debug.Log("Unlocked");
            }
        }
    }
}
