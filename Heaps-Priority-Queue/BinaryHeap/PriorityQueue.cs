using System;
using System.Collections.Generic;

public class PriorityQueue<T> : BinaryHeap<T> where T : IComparable<T>
{
    public void DecreaseKey(T item)
    {
        if (base.heap.Count == 0)
        {
            throw new InvalidOperationException();
        }

        for (int i = 0; i < this.heap.Count; i++)
        {
            if (base.heap[i].Equals(item))
            {
                base.HeapifyDown(i);
            }
        }
    }
}
