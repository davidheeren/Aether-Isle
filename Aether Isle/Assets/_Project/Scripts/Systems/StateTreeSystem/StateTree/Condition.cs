using System;

namespace StateTree
{
    [Serializable]
    public abstract class Condition : Copyable
    {
        protected Condition(string copyJson) : base(copyJson)
        {

        }

        public abstract bool Calculate();
    }
}
