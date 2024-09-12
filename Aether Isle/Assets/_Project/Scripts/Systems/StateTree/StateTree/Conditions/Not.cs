using System;

namespace StateTree
{
    [Serializable]
    public class Not : Condition
    {
        Condition condition;

        public Not(Condition condition)
        {
            this.condition = condition;
        }

        public override bool Calculate()
        {
            return !condition.Calculate();
        }
    }
}
