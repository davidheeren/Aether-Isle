using System;

namespace StateTree
{
    [Serializable]
    public abstract class Condition
    {
        // There is not a CreateCondition Method

        public abstract bool Calculate();
    }
}
