namespace StateTree
{
    public class NullCondition<T> : Condition
    {
        Ref<T> obj;

        public NullCondition(Ref<T> obj) : base(null)
        {
            this.obj = obj;
        }

        public override bool Calculate()
        {
            return obj.value == null;
        }
    }
}
