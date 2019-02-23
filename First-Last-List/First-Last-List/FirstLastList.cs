using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using System.Linq;

public class FirstLastList<T> : IFirstLastList<T> where T : IComparable<T>
{
    private LinkedList<T> byInsertion;
    private OrderedBag<LinkedListNode<T>> ascending;
    private OrderedBag<LinkedListNode<T>> descending;

    public FirstLastList()
    {
        this.byInsertion = new LinkedList<T>();
        this.ascending = new OrderedBag<LinkedListNode<T>>((x, y) => x.Value.CompareTo(y.Value));
        this.descending = new OrderedBag<LinkedListNode<T>>((x, y) => y.Value.CompareTo(x.Value));
    }

    public int Count
    {
        get
        {
            return this.byInsertion.Count;
        }
    }

    public void Add(T element)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(element);
        this.byInsertion.AddLast(node);
        this.ascending.Add(node);
        this.descending.Add(node);
    }

    public void Clear()
    {
        this.byInsertion.Clear();
        this.ascending.Clear();
        this.descending.Clear();
    }

    public IEnumerable<T> First(int count)
    {
        if (!isInBounds(count))
        {
            throw new ArgumentOutOfRangeException();
        }

        LinkedListNode<T> current = this.byInsertion.First;
        for(int i = 0; i < count; i++)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    public IEnumerable<T> Last(int count)
    {
        if (!isInBounds(count))
        {
            throw new ArgumentOutOfRangeException();
        }

        LinkedListNode<T> current = this.byInsertion.Last;
        for (int i = 0; i < count; i++)
        {
            yield return current.Value;
            current = current.Previous;
        }
    }

    public IEnumerable<T> Max(int count)
    {
        if (!isInBounds(count))
        {
            throw new ArgumentOutOfRangeException();
        }
        foreach (var item in this.descending.Take(count))
        {
            yield return item.Value;
        }
        
    }

    public IEnumerable<T> Min(int count)
    {
        if (!isInBounds(count))
        {
            throw new ArgumentOutOfRangeException();
        }

        foreach (var item in this.ascending.Take(count))
        {
            yield return item.Value;
        }
    }

    public int RemoveAll(T element)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(element);
        foreach(var item in this.ascending.Range(node, true, node, true))
        {
            this.byInsertion.Remove(item);
        }
        int result = this.ascending.RemoveAllCopies(node);
        this.descending.RemoveAllCopies(node);

        return result;
    }

    private bool isInBounds(int count)
    {
        return count >= 0 && count <= this.Count;
    }
}
