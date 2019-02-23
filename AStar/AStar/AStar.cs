using System;
using System.Collections.Generic;

public class AStar
{
    private char[,] Map;
    private int MapRows;
    private int MapColumns;

    private PriorityQueue<Node> Queue { get; set; }
    private Dictionary<Node, Node> Parent { get; set; }
    private Dictionary<Node, int> Cost { get; set; }

    public AStar(char[,] map)
    {
        this.Cost = new Dictionary<Node, int>();
        this.Parent = new Dictionary<Node, Node>();
        this.Queue = new PriorityQueue<Node>();
        this.Map = map;
        this.MapRows = this.Map.GetLength(0);
        this.MapColumns = this.Map.GetLength(1);
    }

    public static int GetH(Node current, Node goal)
    {
        int deltaX = Math.Abs(current.Col - goal.Col);
        int deltaY = Math.Abs(current.Row - goal.Row);

        return deltaX + deltaY;
    }


    public IEnumerable<Node> GetPath(Node start, Node goal)
    {
        this.Queue.Enqueue(start);
        this.Parent[start] = null;
        this.Cost[start] = 0;

        int[] steps = new int[4] { 1, -1, 0, 0 };

        while (this.Queue.Count > 0)
        {
            Node current = this.Queue.Dequeue();
            if(current.Equals(goal))
            {
                break;
            }

            int currentRow = current.Row;
            int currentCol = current.Col;


            for (int i = 0, j = 3; i < 4; i++, j--)
            {
                int row = currentRow + steps[i];
                int col = currentCol + steps[j];

                if (IsValid(row, col))
                {
                    Node neighbour = new Node(row, col);

                    int newCost = Cost[current] + 1;

                    if(!this.Cost.ContainsKey(neighbour) || newCost < Cost[neighbour])
                    {
                        this.Cost[neighbour] = newCost;
                        neighbour.F = newCost + GetH(neighbour, goal);
                        this.Queue.Enqueue(neighbour);
                        this.Parent[neighbour] = current;
                    }
                }
            }
        }

        return this.reconstructPath(start, goal);
    }

    private bool IsValid(int row, int col)
    {
        return row >= 0 && row < this.MapRows
               && col >= 0 && col < this.MapColumns
               && this.Map[row, col] != 'W';
    }

    private IEnumerable<Node> reconstructPath(Node start, Node goal)
    {
        Stack<Node> result = new Stack<Node>();

        if (!this.Parent.ContainsKey(goal))
        {
            result.Push(start);
            return result;
        }

        Node current = goal;

        while (current != null)
        {
            result.Push(current);
            current = this.Parent[current];
        }

        return result;
    }
}

