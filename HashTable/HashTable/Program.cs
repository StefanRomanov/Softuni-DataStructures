using System;
using System.Linq;

class Program
{
    static void Main()
    {
        //First task from the homework. Uncomment to execute.
        //CountSymbols();

        //Second task from the homework. Uncomment to execute.
        Phonebook();
    }

    public static void Phonebook()
    {
        HashMap<string, string> map = new HashMap<string, string>();

        string input = Console.ReadLine();

        while (input  != "search")
        {
            string[] parts = input.Split('-');

            map[parts[0]] = parts[1];

            input = Console.ReadLine();
        }

        input = Console.ReadLine();

        while (input != "end")
        {
            if (map.ContainsKey(input))
            {
                Console.WriteLine($"{input} -> {map[input]}");
            }
            else
            {
                Console.WriteLine($"Contact {input} does not exist.");
            }

            input = Console.ReadLine();
        }
    }

    public static void CountSymbols()
    {
        HashMap<char, int> map = new HashMap<char, int>();

        char[] input = Console.ReadLine().ToCharArray();

        foreach (char symbol in input)
        {
            if (!map.ContainsKey(symbol))
            {
                map[symbol] = 0;
            }
            map[symbol]++;
        }

        foreach (char key in map.Keys.OrderBy(k => k))
        {
            Console.WriteLine($"{key}: {map[key]} time/s");
        }
    }
}
