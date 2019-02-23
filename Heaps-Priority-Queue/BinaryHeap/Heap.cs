using System;
using System.Collections.Generic;

public static class Heap<T> where T : IComparable<T>
{
    public static void Sort(T[] arr)
    {
        BinaryHeap<T> heap = new BinaryHeap<T>(arr);

        for(int i = arr.Length - 1; i >= 0; i--)
        {
            arr[i] = heap.Pull();
        }
    }
}
