namespace StateTree
{
    public class VirtualCondition : Condition
    {
        ConditionMethod condition;

        public VirtualCondition Create(ConditionMethod condition)
        {
            this.condition = condition;

            return this;
        }

        public override bool Calculate()
        {
            return condition();
        }
    }

    public delegate bool ConditionMethod();
}
