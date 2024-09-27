using System;

namespace StateTree
{
    public class VirtualCondition : Condition
    {
        Func<bool> condition;

        public VirtualCondition(Func<bool> condition, Node child = null) : base(child)
        {
            this.condition = condition;
        }

        public override bool Calculate()
        {
            return condition();
        }
    }
}
