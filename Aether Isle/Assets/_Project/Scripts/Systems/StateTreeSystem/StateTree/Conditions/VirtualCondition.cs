using UnityEngine;

namespace StateTree
{
    public class VirtualCondition : Condition
    {
        ConditionMethod condition;

        public VirtualCondition(ConditionMethod condition) : base(null)
        {
            if (condition == null)
                Debug.LogError("Condition Method cannot be null");

            this.condition = condition;
        }

        public override bool Calculate()
        {
            return condition();
        }
    }

    public delegate bool ConditionMethod();
}
