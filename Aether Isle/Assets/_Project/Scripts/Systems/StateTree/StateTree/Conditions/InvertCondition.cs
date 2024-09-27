using System;

namespace StateTree
{
    public class InvertCondition : Condition
    {
        Condition condition;

        public InvertCondition(Condition condition, Node child) : base(child)
        {
            this.condition = condition;
        }

        public override bool Calculate()
        {
            return !condition.Calculate();
        }
    }
}
