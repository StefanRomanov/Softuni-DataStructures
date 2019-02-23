using System;

public class AVL<T> where T : IComparable<T>
{
    private Node<T> root;

    public Node<T> Root
    {
        get
        {
            return this.root;
        }
    }

    public bool Contains(T item)
    {
        var node = this.Search(this.root, item);
        return node != null;
    }

    public void Insert(T item)
    {
        this.root = this.Insert(this.root, item);
    }

    public void Delete(int item)
    {
        this.root = this.Delete(this.root, item);
    }

    private Node<T> Delete(Node<T> node, int item)
    {
        if (node == null)
        {
            return null;
        }

        var cmp = item.CompareTo(node.Value);

        if (cmp < 0)
        {
            node.Left = Delete(node.Left, item);
        }
        else if (cmp > 0)
        {
            node.Right = Delete(node.Right, item);
        }
        else
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }
            else
            {
                var min = GetMin(node.Right);
                min.Right = DeleteMin(node.Right);
                min.Left = node.Left;
                node = min;
            }
        }

        node = Balance(node);
        return node;
    }

    public void DeleteMin()
    {
        this.root = this.DeleteMin(this.root);
    }

    private Node<T> DeleteMin(Node<T> node)
    {
        if (node == null)
        {
            return null;
        }

        if (node.Left == null)
        {
            return node.Right;
        }

        node.Left = DeleteMin(node.Left);

        node = Balance(node);

        return node;
    }

    public void EachInOrder(Action<T> action)
    {
        this.EachInOrder(this.root, action);
    }

    private Node<T> Insert(Node<T> node, T item)
    {
        if (node == null)
        {
            return new Node<T>(item);
        }

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            node.Left = this.Insert(node.Left, item);
        }
        else if (cmp > 0)
        {
            node.Right = this.Insert(node.Right, item);
        }

        node = Balance(node);
        UpdateHeight(node);

        return node;
    }

    private Node<T> Search(Node<T> node, T item)
    {
        if (node == null)
        {
            return null;
        }

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            return Search(node.Left, item);
        }
        else if (cmp > 0)
        {
            return Search(node.Right, item);
        }

        return node;
    }

    private void EachInOrder(Node<T> node, Action<T> action)
    {
        if (node == null)
        {
            return;
        }

        this.EachInOrder(node.Left, action);
        action(node.Value);
        this.EachInOrder(node.Right, action);
    }

    private static Node<T> Balance(Node<T> node)
    {
        int balance = Height(node.Left) - Height(node.Right);
        if(balance < -1)
        {
            balance = Height(node.Right.Left) - Height(node.Right.Right);

            if(balance > 0)
            {
                node.Right = RotateRight(node.Right);
            }

            return RotateLeft(node);
        }
        else if (balance > 1)
        {
            balance = Height(node.Left.Left) - Height(node.Left.Right);
            if (balance < 0)
            {
                node.Left = RotateLeft(node.Left);
            }

            return RotateRight(node);
        }

        return node;
    }

    private static int Height(Node<T> node)
    {
        if(node == null)
        {
            return 0;
        }

        return node.Height;
    }

    private static void UpdateHeight(Node<T> node)
    {
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    private static Node<T> RotateLeft(Node<T> oldRoot)
    {
        Node<T> newRoot = oldRoot.Right;
        oldRoot.Right = oldRoot.Right.Left;
        newRoot.Left = oldRoot;

        UpdateHeight(oldRoot);

        return newRoot;
     }

    private static Node<T> RotateRight(Node<T> oldRoot)
    {
        Node<T> newRoot = oldRoot.Left;
        oldRoot.Left = oldRoot.Left.Right;
        newRoot.Right = oldRoot;

        UpdateHeight(oldRoot);

        return newRoot;
    }

    private Node<T> GetMin(Node<T> node)
    {
        if (node == null)
        {
            return null;
        }

        if (node.Left == null)
        {
            return node;
        }

        return GetMin(node.Left);
    }
}
