using System;

namespace StateTree
{
    [Serializable]
    public class Ref<T>
    {
        public T value;

        public Ref(T value = default)
        {
            this.value = value;
        }
    }
}
