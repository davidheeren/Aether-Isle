namespace StateTree
{
    public class NullCondition<T> : Condition
    {
        Ref<T> obj;

        public NullCondition(Ref<T> obj, Node child = null) : base(child)
        {
            this.obj = obj;
        }

        public override bool Calculate()
        {
            return obj.value == null;
        }
    }
}
