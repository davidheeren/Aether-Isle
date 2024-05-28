using System;

namespace StateTree
{
    [Serializable]
    public class And : Condition
    {
        Condition condition1;
        Condition condition2;

        public And(Condition condition1, Condition condition2) : base(null)
        {
            this.condition1 = condition1;
            this.condition2 = condition2;
        }

        public override bool Calculate()
        {
            return condition1.Calculate() && condition2.Calculate();
        }
    }
}
