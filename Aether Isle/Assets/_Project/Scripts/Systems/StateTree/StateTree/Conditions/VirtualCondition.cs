namespace StateTree
{
    public class VirtualCondition : Condition
    {
        ConditionMethod condition;

        public VirtualCondition(ConditionMethod condition)
        {
            this.condition = condition;
        }

        public override bool Calculate()
        {
            return condition();
        }
    }

    public delegate bool ConditionMethod();
}
