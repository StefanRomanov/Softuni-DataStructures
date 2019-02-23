using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    public static void Main(string[] args)
    {
        ProductManager productManager = new ProductManager();
        int lines = int.Parse(Console.ReadLine());

        for (int i = 0; i < lines; i++)
        {

            Regex rx = new Regex("([a-zA-Z]+) (.*)");
            Match match = rx.Match(Console.ReadLine().Trim());

            string command = match.Groups[1].Value;
            string[] arguments = match.Groups[2].Value.Split(';');

            switch (command)
            {
                case "AddProduct":
                    Console.WriteLine(productManager.AddProduct(arguments[0], arguments[2], decimal.Parse(arguments[1])));
                    break;
                case "FindProductsByName":
                    var productsByName = productManager.FindProductsByName(arguments[0]);

                    if (productsByName.Count > 0)
                    {
                        foreach (var product in productsByName)
                        {
                            Console.WriteLine(product.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found");
                    }
                    break;
                case "FindProductsByProducer":
                    var productsByProducer = productManager.FindProductsByProducer(arguments[0]);
                    if (productsByProducer.Count > 0)
                    {
                        foreach (var product in productsByProducer)
                        {
                            Console.WriteLine(product.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found");
                    }

                    break;
                case "DeleteProducts":
                    string result = "";
                    if (arguments.Length > 1)
                    {
                        result = productManager.DeleteProducts(arguments[0], arguments[1]);
                    }
                    else
                    {
                        result = productManager.DeleteProducts(arguments[0]);
                    }

                    Console.WriteLine(result);
                    break;
                case "FindProductsByPriceRange":
                    var productsByPrice = productManager
                                    .FindProductsByPriceRange(decimal.Parse(arguments[0]), decimal.Parse(arguments[1]));
                    if (productsByPrice.Count > 0)
                    {
                        foreach (var product in productsByPrice)
                        {
                            Console.WriteLine(product.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found");
                    }
                    break;
            }
        }
    }
}
