namespace StateTree
{
    public abstract class PriorityModifier : Modifier
    {
        protected void CreatePriorityModifier(Node child)
        {
            CreateModifier(child);

            subState.Priority = Priority;
        }

        public abstract int Priority();
    }
}
