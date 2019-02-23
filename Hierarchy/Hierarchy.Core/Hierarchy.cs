using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Hierarchy<T> : IHierarchy<T>
{
    private Node<T> Root;
    private Dictionary<T, Node<T>> Nodes;

    public Hierarchy(T root)
    {
        this.Root = new Node<T>(root);
        this.Nodes = new Dictionary<T, Node<T>>();
        this.Nodes.Add(root, this.Root);
    }

    public int Count
    {
        get
        {
            return this.Nodes.Count;
        }
    }

    public void Add(T element, T child)
    {
        if (!this.Contains(element))
        {
            throw new ArgumentException("Element does not exist");
        }

        if (this.Contains(child))
        {
            throw new ArgumentException("Child already exists");
        }

        Node<T> childNode = new Node<T>(child);

        this.Nodes[element].Children.Add(childNode);
        this.Nodes.Add(child, childNode);
        this.Nodes[child].Parent = this.Nodes[element];
    }

    public void Remove(T element)
    {
        if (!this.Contains(element))
        {
            throw new ArgumentException("Element doesn't exist");
        }

        Node<T> current = this.Nodes[element];

        if (current.Equals(this.Root))
        {
            throw new InvalidOperationException("Cannot delete root");
        }
            
        List<Node<T>> children = current.Children;
        Node<T> parent = current.Parent;

        foreach(Node<T> child in children)
        {
            child.Parent = parent;
        }

        parent.Children.Remove(current);

        if (children.Count > 0)
        {
            parent.Children.AddRange(children);
        }


        this.Nodes.Remove(element);
            
    }

    public IEnumerable<T> GetChildren(T item)
    {
        if (!this.Contains(item))
        {
            throw new ArgumentException("Element doesn't exist");
        }

        return this.Nodes[item].Children.Select(x => x.Value);
    }

    public T GetParent(T item)
    {
        if (!this.Contains(item))
        {
            throw new ArgumentException("Element doesn't exist");
        }

        Node<T> parent = this.Nodes[item].Parent;

        if (parent == null)
        {
            return default(T);
        }
        else
        {
            return parent.Value;
        }
    }

    public bool Contains(T value)
    {
        return this.Nodes.ContainsKey(value);
    }

    public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
    {
        List<T> result = new List<T>();

        foreach(T element in this.Nodes.Keys)
        {
            if (other.Contains(element))
            {
                result.Add(element);
            }
        }

        return result;
    } 

    public IEnumerator<T> GetEnumerator()
    {
        if(this.Count == 0)
        {
            yield break;
        }

        Queue<Node<T>> queue = new Queue<Node<T>>();
        queue.Enqueue(this.Root);

        while(queue.Count > 0)
        {
            Node<T>  current = queue.Dequeue();

            yield return current.Value;

            foreach (Node<T> child in current.Children)
            {
                queue.Enqueue(child);
            }
        }   
        
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public class Node<T>
    {
        public Node(T value, Node<T> parent = null)
        {
            this.Value = value;
            this.Parent = parent;
            this.Children = new List<Node<T>>();
        }

        public T Value { get; set; }
        public Node<T> Parent { get; set; }
        public List<Node<T>> Children { get; set; }
    }
}