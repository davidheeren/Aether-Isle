namespace StateTree
{
    public class NullCondition<T> : Condition
    {
        Ref<T> obj;

        public NullCondition<T> Create(Ref<T> obj)
        {
            this.obj = obj;

            return this;
        }

        public override bool Calculate()
        {
            return obj.value == null;
        }
    }
}
