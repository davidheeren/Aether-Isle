using UnityEngine;

namespace StateTree
{
    /// <summary>
    /// Overrides input conditions children
    /// </summary>
    public class CompositeCondition : Condition
    {
        Condition a;
        Condition b;
        CompositeType type;


        /// <summary>
        /// Input Conditions should not have children because they won't matter
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        /// <param name="child"></param>
        public CompositeCondition(Condition a, Condition b, CompositeType type, Node child) : base(child)
        {
            this.a = a;
            this.b = b;
            this.type = type;
        } 


        public override bool Calculate()
        {
            bool a = this.a.Calculate();
            bool b = this.b.Calculate();

            switch (type)
            {
                case CompositeType.And: // Logical AND
                    return a && b;

                case CompositeType.Or:  // Logical OR
                    return a || b;

                case CompositeType.Xor:  // Exclusive OR (true if one is true, but not both)
                    return a ^ b;        // XOR is implemented with the ^ operator in C#

                case CompositeType.Nand: // Negation of AND (true unless both are true)
                    return !(a && b);

                case CompositeType.Nor:  // Negation of OR (true if both are false)
                    return !(a || b);

                case CompositeType.Xnor: // Negation of XOR (true if both are the same)
                    return !(a ^ b);
            }

            Debug.LogError("Not a supported type");
            return false;
        }
    }

    public enum CompositeType
    {
        And,
        Or,
        Xor,  
        Nand,
        Nor,
        Xnor
    }
}
