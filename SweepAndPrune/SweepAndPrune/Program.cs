using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{ 
    static void Main(string[] args)
    {
        IList<Figure> figures = new List<Figure>();
        IDictionary<string, Figure> figuresByName = new Dictionary<string, Figure>();

        string[] input = Console.ReadLine().Split(' ');

        while (input[0] != "start")
        {
            Figure current = new Figure(input[1], int.Parse(input[2]), int.Parse(input[3]));
            figures.Add(current);
            figuresByName[input[1]] = current;

            input = Console.ReadLine().Split(' ');
        }

        int ticks = -1;
        IList<string> collisionStrings = new List<string>();

        while(input[0] != "end")
        {
            ticks++;

            if (input[0].Equals("move"))
            {
                Figure current = figuresByName[input[1]];
                current.X1 = int.Parse(input[2]);
                current.Y1 = int.Parse(input[3]);
            }

            if (!input[0].Equals("start"))
            {
                collisionStrings = RecalculateCollisuions(figures);

                PrintCollisions(ticks, collisionStrings);
            }

            input = Console.ReadLine().Split(' ');
        }
    }

    private static void PrintCollisions(int ticks, IList<string> collisionStrings)
    {
        foreach(string output in collisionStrings)
        {
            Console.WriteLine($"({ticks}) {output}");
        }
    }

    private static IList<String> RecalculateCollisuions(IList<Figure> figures)
    {
        IList<String> collisionsList = new List<string>();

        InsertionSort(figures);

        for (var i = 0; i < figures.Count; i++)
        {
            var current = figures[i];

            for (var j = i + 1; j < figures.Count; j++)
            {
                var other = figures[j];

                if (current.X2 < other.X1)
                {
                    break;
                }

                if (current.Intersects(other))
                {
                    collisionsList.Add($"{current.Name} collides with {other.Name}");
                }
            }
        }

        return collisionsList;
    }

    private static void InsertionSort(IList<Figure> figures)
    {
        for (var i = 0; i < figures.Count; i++)
        {
            var j = i;

            while (j > 0 && figures[j - 1].X1 > figures[j].X1)
            {
                var temp = figures[j - 1];
                figures[j - 1] = figures[j];
                figures[j] = temp;

                j--;
            }
        }
    }
}