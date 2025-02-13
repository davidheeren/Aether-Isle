using System;

namespace Pathfinding
{
    public class MinHeapLegacy1<TValue, TPriority> where TPriority : IComparable<TPriority>
    {
        // Notes about a Heap:
        // - Parent priority <= children priorities
        // - Child L or R does not matter to priority
        // - Each parent can have 0-2 children

        HeapItem[] items;
        int currentItemCount;
        public int Count => currentItemCount;

        public MinHeapLegacy1(int maxHeapSize)
        {
            items = new HeapItem[maxHeapSize];
        }

        public void Add(TValue value, TPriority priority)
        {
            if (currentItemCount >= items.Length)
                throw new InvalidOperationException("Heap is full");

            items[currentItemCount] = new HeapItem(value, priority);
            SortUp(currentItemCount);
            currentItemCount++;
        }

        public TValue Peek()
        {
            if (currentItemCount == 0)
                throw new InvalidOperationException("Heap is empty");

            return items[0].value;
        }

        public TValue Pop()
        {
            if (currentItemCount == 0)
                throw new InvalidOperationException("Heap is empty");

            HeapItem firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            SortDown(0);
            return firstItem.value;
        }

        public void Clear()
        {
            currentItemCount = 0;
        }

        private void SortDown(int index)
        {
            while (true)
            {
                int childIndexLeft = ChildLeftIndex(index);
                int childIndexRight = ChildRightIndex(index);
                int swapIndex = index;

                // Check if left child exists and has lower priority
                if (childIndexLeft < currentItemCount && items[childIndexLeft].priority.CompareTo(items[swapIndex].priority) < 0)
                {
                    swapIndex = childIndexLeft;
                }

                // Check if right child exists and has lower priority than current lowest
                if (childIndexRight < currentItemCount && items[childIndexRight].priority.CompareTo(items[swapIndex].priority) < 0)
                {
                    swapIndex = childIndexRight;
                }

                // If no swap is needed, we're done
                if (swapIndex == index)
                    break;

                Swap(index, swapIndex);
                index = swapIndex;
            }
        }

        private void SortUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = ParentIndex(index);

                if (items[index].priority.CompareTo(items[parentIndex].priority) < 0)
                {
                    Swap(index, parentIndex);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        void Swap(int a, int b)
        {
            HeapItem temp = items[a];
            items[a] = items[b];
            items[b] = temp;
        }

        private int ParentIndex(int childIndex) => (childIndex - 1) / 2;

        private int ChildLeftIndex(int parentIndex) => parentIndex * 2 + 1;

        private int ChildRightIndex(int parentIndex) => parentIndex * 2 + 2;

        private struct HeapItem
        {
            public TValue value;
            public TPriority priority;

            public HeapItem(TValue value, TPriority priority)
            {
                this.value = value;
                this.priority = priority;
            }
        }
    }
}
