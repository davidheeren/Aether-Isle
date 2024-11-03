using System;
using System.Collections;
using System.Collections.Generic;

namespace Utilities
{
    public class RollingBuffer<T> : IEnumerable<T>
    {
        public readonly int length;

        T[] buffer;
        //public T[] Buffer => buffer;

        public int Count { get; private set; }
        int currentIndex;

        public RollingBuffer(int length)
        {
            this.length = length;
            buffer = new T[length];
        }

        public RollingBuffer<T> Add(T obj)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero.");

            buffer[currentIndex] = obj;
            currentIndex = (currentIndex + 1) % buffer.Length;

            if (Count < length)
                Count++;

            return this;
        }

        public T Get(int index)
        {
            if (index < 0 || index >= length)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the filled range.");

            return buffer[index];
        }

        public RollingBuffer<T> Clear()
        {
            buffer = new T[length];
            Count = 0;
            currentIndex = 0;

            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < length; i++)
            {
                yield return Get(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
