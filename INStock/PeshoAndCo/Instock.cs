using System;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using System.Linq;

public class Instock : IProductStock
{
    private Dictionary<string, LinkedListNode<Product>> _labelMap;
    private List<Product> _productsList;
    private Dictionary<int, LinkedList<Product>> _quantityMap;
    private OrderedBag<Product> _priceBag;
    private SortedSet<Product> _sortedLabels;

    public Instock()
    {
        _productsList = new List<Product>();
        _labelMap = new Dictionary<string, LinkedListNode<Product>>();
        _quantityMap = new Dictionary<int, LinkedList<Product>>();
        _priceBag = new OrderedBag<Product>((a,b) => b.Price.CompareTo(a.Price));
        _sortedLabels = new SortedSet<Product>();
    }

    public int Count => _productsList.Count;

    public void Add(Product product)
    {
        if (_labelMap.ContainsKey(product.Label))
        {
            throw new ArgumentException();
        }

        LinkedListNode<Product> node = new LinkedListNode<Product>(product);
        
        _productsList.Add(product);
        
        _labelMap.Add(product.Label,node);
        
        if (!_quantityMap.ContainsKey(product.Quantity))
        {
            _quantityMap[product.Quantity] = new LinkedList<Product>();
        }
        
        _quantityMap[product.Quantity].AddLast(node);
        _priceBag.Add(product);
        _sortedLabels.Add(product);
    }

    public void ChangeQuantity(string product, int quantity)
    {
        if (!_labelMap.ContainsKey(product))
        {
            throw new ArgumentException();
        }

        var node = _labelMap[product];

        if (node.Value.Quantity == quantity)
        {
            return;
        }

        _quantityMap[node.Value.Quantity].Remove(node);
        node.Value.Quantity = quantity;

        if (!_quantityMap.ContainsKey(quantity))
        {
            _quantityMap[quantity] = new LinkedList<Product>();
        }
        
        _quantityMap[quantity].AddLast(node);
    }

    public bool Contains(Product product)
    {
        return _labelMap.ContainsKey(product.Label);
    }

    public Product Find(int index)
    {
        if (Count <= index || index < 0)
        {
            throw new IndexOutOfRangeException();
        }
        
        return _productsList[index];
    }

    public IEnumerable<Product> FindAllByPrice(double price)
    {
        var product = new Product("", price,0);
        return _priceBag.Range(product,true,product,true);
    }

    public IEnumerable<Product> FindAllByQuantity(int quantity)
    {
        if (_quantityMap.ContainsKey(quantity))
        {
            return  _quantityMap[quantity];
        }
        
        return new List<Product>();
    }

    public IEnumerable<Product> FindAllInRange(double lo, double hi)
    {
        var low = new Product("", lo,0);
        var high = new Product("", hi,0);
        return _priceBag.Range(high,true,low, false);
    }

    public Product FindByLabel(string label)
    {
        if (!_labelMap.ContainsKey(label))
        {
            throw new ArgumentException();
        }

        return _labelMap[label].Value;
    }

    public IEnumerable<Product> FindFirstByAlphabeticalOrder(int count)
    {
        if (count > Count)
        {
            throw new ArgumentException();
        }
        
        return _sortedLabels.Take(count);
    }

    public IEnumerable<Product> FindFirstMostExpensiveProducts(int count)
    {
        if (count > Count)
        {
            throw new ArgumentException();
        }
        
        return _priceBag.Take(count);
    }

    public IEnumerator<Product> GetEnumerator()
    {
        return _productsList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
