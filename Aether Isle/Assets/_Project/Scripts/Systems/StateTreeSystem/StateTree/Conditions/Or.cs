using System;

namespace StateTree
{
    [Serializable]
    public class Or : Condition
    {
        Condition condition1;
        Condition condition2;

        public Or Create(Condition condition1, Condition condition2)
        {
            this.condition1 = condition1;
            this.condition2 = condition2;

            return this;
        }

        public override bool Calculate()
        {
            return condition1.Calculate() || condition2.Calculate();
        }
    }
}
