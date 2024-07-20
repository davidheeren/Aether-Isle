using System;

namespace StateTree
{
    [Serializable]
    public class Not : Condition
    {
        Condition condition;

        public Not Create(Condition condition)
        {
            this.condition = condition;

            return this;
        }

        public override bool Calculate()
        {
            return !condition.Calculate();
        }
    }
}
