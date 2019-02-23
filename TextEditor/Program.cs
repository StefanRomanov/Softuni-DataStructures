using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        ITextEditor textEditor = new TextEditor();
        Regex pattern = new Regex("\"(.*)\"");

        string command = string.Empty;
        while((command = Console.ReadLine()) != "end")
        {
            Match match = pattern.Match(command);
            string matchedString = match.Groups[1].ToString();

            string[] commandArgs = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string firstArgument = commandArgs[0];
    
            switch (firstArgument)
            {
                case "users":
                    string prefix = "";
                    if(commandArgs.Length > 1)
                    {
                        prefix = commandArgs[1];
                    }
                    foreach (string user in textEditor.Users(prefix))
                    {
                        Console.WriteLine(user);
                    }
                    continue;
                case "login":
                    textEditor.Login(commandArgs[1]);
                    continue;
                case "logout":
                    textEditor.Logout(commandArgs[1]);
                    continue;
            }

            try
            {
                switch (commandArgs[1])
                {
                    case "insert":
                        textEditor.Insert(firstArgument, int.Parse(commandArgs[2]), matchedString);
                        break;
                    case "prepend":
                        textEditor.Prepend(firstArgument, matchedString);
                        break;
                    case "substring":
                        textEditor.Substring(firstArgument, int.Parse(commandArgs[2]), int.Parse(commandArgs[3]));
                        break;
                    case "delete":
                        textEditor.Delete(firstArgument, int.Parse(commandArgs[2]), int.Parse(commandArgs[3]));
                        break;
                    case "clear":
                        textEditor.Clear(firstArgument);
                        break;
                    case "length":
                        Console.WriteLine(textEditor.Length(firstArgument));
                        break;
                    case "print":
                        Console.WriteLine(textEditor.Print(firstArgument));
                        break;
                    case "undo":
                        textEditor.Undo(firstArgument);
                        break;
                }
            } catch(Exception ignored)
            {
                //ignored
            }
        }
    }
}
