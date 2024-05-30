
using UnityEngine;

namespace StateTree
{
    public class NullBeforeModifier : Modifier
    {
        Timer timer;

        public NullBeforeModifier(float delay, Node child) : base(null, child)
        {
            timer = new Timer(delay);
            timer.ForceDone();
        }

        protected override void EnterChildState()
        {
            timer.ForceDone();
        }

        protected override void ExitChildState()
        {
            timer.Reset();
        }

        public override State Evaluate()
        {
            //Debug.Log(timer.isDone);
            if (!timer.isDone)
                return null;
            else
                return child.Evaluate();
        }
    }
}
