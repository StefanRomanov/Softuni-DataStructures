using System;
using System.Collections.Generic;

public class BinaryHeap<T> where T : IComparable<T>
{
    protected List<T> heap { get; }

    public BinaryHeap()
    {
        this.heap = new List<T>();
    }

    public BinaryHeap(T[] array)
    {
        this.heap = new List<T>();
        foreach(T item in array)
        {
            this.Insert(item);
        }
    }

    public int Count
    {
        get
        {
            return this.heap.Count;
        }
    }

    public void Insert(T item)
    {
        this.heap.Add(item);
        this.HeapifyUp(this.heap.Count - 1);
    }

    public T Peek()
    {
        return this.heap[0];
    }

    public T Pull()
    {
        if (this.Count <= 0)
        {
            throw new InvalidOperationException();
        }

        T item = this.heap[0];

        this.Swap(0, this.heap.Count - 1);
        this.heap.RemoveAt(this.heap.Count - 1);
        this.HeapifyDown(0);

        return item;
    }

    protected void HeapifyDown(int index)
    {
        while (index < this.heap.Count / 2)
        {
            int child = this.LeftChildIndex(index);
            if(this.HasChild(child + 1) && this.IsGreater(child + 1, child))
            {
                child = child + 1;
            }

            if(this.IsGreater(index, child))
            {
                break;
            }

            this.Swap(index,child);
            index = child;
        }
    }

    private bool HasChild(int index)
    {
        return this.heap.Count - 1 >= index;
    }

    private int LeftChildIndex(int index)
    {
        return index * 2 + 1;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0 && this.IsGreater(index, this.Parent(index)))
        {
            this.Swap(index, this.Parent(index));
            index = this.Parent(index);
        }
    }

    private void Swap(int index, int parentIndex)
    {
        T temp = this.heap[index];
        this.heap[index] = this.heap[parentIndex];
        this.heap[parentIndex] = temp;
    }

    private int Parent(int index)
    {
        return (index - 1)/2;
    }

    private bool IsGreater(int firstIndex, int secondIndex)
    {
        return this.heap[firstIndex].CompareTo(this.heap[secondIndex]) > 0;
    }
}
