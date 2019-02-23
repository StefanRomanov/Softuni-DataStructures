using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class ProductManager
{
    private Dictionary<string, OrderedBag<Product>> nameMap;

    private Dictionary<string, OrderedBag<Product>> producerMap;

    private OrderedMultiDictionary<decimal, Product> priceMap;

    private Dictionary<string, OrderedBag<Product>> NameAndProducerMap;

    public ProductManager()
    {
        this.nameMap = new Dictionary<string, OrderedBag<Product>>();
        this.producerMap = new Dictionary<string, OrderedBag<Product>>();
        this.NameAndProducerMap = new Dictionary<string, OrderedBag<Product>>();
        this.priceMap = new OrderedMultiDictionary<decimal, Product>(true);
    }

    public string AddProduct(string name, string producer, decimal price)
    {
        Product product = new Product(name, producer, price);

        if (!nameMap.ContainsKey(name))
        {
            nameMap[name] = new OrderedBag<Product>();
        }
        if (!producerMap.ContainsKey(producer))
        {
            producerMap[producer] = new OrderedBag<Product>();
        }
        if (!priceMap.ContainsKey(price))
        {
            priceMap[price] = new OrderedBag<Product>();
        }

        nameMap[name].Add(product);
        priceMap[price].Add(product);
        producerMap[producer].Add(product);

        return "Product added";
    }

    public string DeleteProducts(string producer)
    {
        if (!producerMap.ContainsKey(producer))
        {
            return "No products found";
        }
        OrderedBag<Product> results = producerMap[producer];

        foreach (Product product in results)
        {
            nameMap[product.Name].Remove(product);
            priceMap[product.Price].Remove(product);
        }

        producerMap.Remove(producer);

        return $"{results.Count} products deleted";
    }

    public string DeleteProducts(string name, string producer)
    {
        if (!producerMap.ContainsKey(producer))
        {
            return "No products found";
        }

        OrderedBag<Product> results = new OrderedBag<Product>();
        OrderedBag<Product> resultsByProducer = producerMap[producer];

        foreach (Product product in resultsByProducer)
        {
            if (product.Name == name)
            {
                results.Add(product);
            }
        }

        if(results.Count == 0)
        {
            return "No products found";
        }

        foreach (Product product in results)
        {
            producerMap[product.Producer].Remove(product);
            nameMap[product.Name].Remove(product);
            priceMap[product.Price].Remove(product);
        }

        return $"{results.Count} products deleted";
    }

    public OrderedBag<Product> FindProductsByName(string name)
    {
        if (!nameMap.ContainsKey(name))
        {
            return new OrderedBag<Product>();
        }
        return nameMap[name];
    }

    public OrderedBag<Product> FindProductsByProducer(string producer)
    {
        if (!producerMap.ContainsKey(producer))
        {
            return new OrderedBag<Product>();
        }

        return producerMap[producer];
    }

    public ICollection<Product> FindProductsByPriceRange(decimal low, decimal high)
    {
        var result = new OrderedBag<Product>();
        var resultMap = priceMap.Range(low, true, high, true).SelectMany(x => x.Value).ToList();

        return resultMap;
    }

    private static string GetNameAndProducerKey(string name, string producer)
    {
        return $"{name}_{producer}";
    }
}