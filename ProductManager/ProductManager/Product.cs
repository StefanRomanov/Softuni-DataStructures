using System;

public class Product : IComparable<Product>
{
    public string Name { get; private set; }
    public string Producer { get; private set; }
    public decimal Price { get; private set; }

    public Product(string name, string producer, decimal price)
    {
        this.Name = name;
        this.Producer = producer;
        this.Price = price;
    }

    public override string ToString()
    {
        return $"{{{this.Name};{this.Producer};{this.Price.ToString("F")}}}"; ;
    }

    public int CompareTo(Product other)
        {
            if (this == other)
            {
                return 0;
            }

            var cmp = string.Compare(Name, other.Name, StringComparison.InvariantCulture);

            if (cmp == 0)
            {
                cmp = string.Compare(Producer, other.Producer, StringComparison.InvariantCulture);
            }

            if (cmp == 0)
            {
                cmp = Price.CompareTo(other.Price);
            }

            return cmp;
        }
 
}