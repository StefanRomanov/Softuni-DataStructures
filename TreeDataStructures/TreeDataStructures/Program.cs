using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Dictionary<int, Tree<int>> nodeByValue = new Dictionary<int, Tree<int>>();

    public static void Main(string[] args)
    {
        ReadTree();
        List<int> result = LongestPath();

        Console.WriteLine("Longest path: " + string.Join(" ", result));
    }

    static Tree<int> GetTreeNodeByValue(int value)
    {
        if (!nodeByValue.ContainsKey(value))
        {
            nodeByValue[value] = new Tree<int>(value);
        }

        return nodeByValue[value];
    }

    static void AddEdge(int parent, int child)
    {
        Tree<int> parentNode = GetTreeNodeByValue(parent);
        Tree<int> childNode = GetTreeNodeByValue(child);

        parentNode.Children.Add(childNode);
        childNode.Parent = parentNode;
    }

    static void ReadTree()
    {
        int nodeCount = int.Parse(Console.ReadLine());
        for (int i = 1; i < nodeCount; i++)
        {
            string[] edge = Console.ReadLine().Split(' ');
            AddEdge(int.Parse(edge[0]), int.Parse(edge[1]));
        }
    }

    static Tree<int> FindRootNode()
    {
        return nodeByValue.Values.FirstOrDefault(x => x.Parent == null);
    }

    static List<int> FindLeafNodes()
    {
        return nodeByValue.Values.Where(x => x.Children.Count == 0).ToList().Select(x => x.Value).ToList();
    }

    static List<int> FindMiddleNodes()
    {
        return nodeByValue.Values.Where(x => x.Children.Count != 0 && x.Parent != null).ToList().Select(x => x.Value).ToList();
    }

    static void PrintTree()
    {
        Tree<int> root = nodeByValue.Values.FirstOrDefault(x => x.Parent == null);
        PrintNode(root,0);
    }

    static void PrintNode(Tree<int> node, int indentation)
    {
        Console.WriteLine(new string(' ', indentation) + node.Value);

        if (node.Children.Count == 0)
        {
            return;
        }

        foreach (Tree<int> child in node.Children)
        {
            PrintNode(child, indentation + 2);
        }
    }

    static Tree<int> DeepestNode()
    {
        List<Tree<int>> leafs = nodeByValue.Values.Where(x => x.Children.Count == 0).ToList();

        int maxPath = 0;
        int currentPath = 0;

        Tree<int> maxNode = null;

        foreach (Tree<int> node in leafs)
        {
            var currentNode = node;

            while(currentNode.Parent != null)
            {
                currentPath++;
                currentNode = currentNode.Parent;
            }

            if (currentPath > maxPath)
            {
                maxNode = node;
                maxPath = currentPath;
            }

            currentPath = 0;
        }

        return maxNode;
    }

    static List<int> LongestPath()
    {
        Tree<int> node = DeepestNode();

        List<int> result = new List<int>();

        while(true)
        {
            result.Add(node.Value);

            if(node.Parent == null)
            {
                break;
            }
            node = node.Parent;
        }

        result.Reverse();

        return result;
    }
}
