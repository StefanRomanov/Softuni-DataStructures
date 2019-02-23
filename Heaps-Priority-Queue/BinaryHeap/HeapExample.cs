using System;

public class HeapExample
{
    static void Main()
    {
        Console.WriteLine("Created an empty heap.");
        var heap = new BinaryHeap<int>();
        heap.Insert(5);
        heap.Insert(8);
        heap.Insert(1);
        heap.Insert(3);
        heap.Insert(12);
        heap.Insert(-4);

        heap.heap[1] = 3;
        
        //Console.WriteLine("Heap elements (max to min):");
        //while (heap.Count > 0)
        //{
        //    var max = heap.Pull();
        //    Console.WriteLine(max);
        //}

        //int[] array = new int[5];
        //array[0] = 6;
        //array[1] = 3;
        //array[2] = -1;
        //array[3] = 14;
        //array[4] = 20;
        //
        //Heap<int>.Sort(array);

        
    }
}
