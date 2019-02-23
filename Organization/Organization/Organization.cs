using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Organization : IOrganization
{
    private List<Person> personList;
    private Dictionary<string, HashSet<Person>> nameMap;

    public Organization()
    {
        personList = new List<Person>();
        nameMap = new Dictionary<string, HashSet<Person>>();
    }

    public int Count => personList.Count;

    public bool Contains(Person person)
    {
        return ContainsByName(person.Name);
    }

    public bool ContainsByName(string name)
    {
        return nameMap.ContainsKey(name);
    }

    public void Add(Person person)
    {
        if (!nameMap.ContainsKey(person.Name))
        {
            nameMap[person.Name] = new HashSet<Person>();
        }

        personList.Add(person);
        nameMap[person.Name].Add(person);
    }

    public Person GetAtIndex(int index)
    {
        if (index >= Count)
        {
            throw new IndexOutOfRangeException();
        }

        return personList[index];
    }

    public IEnumerable<Person> GetByName(string name)
    {
        if (!nameMap.ContainsKey(name))
        {
            return new List<Person>();
        }

        return nameMap[name];
    }

    public IEnumerable<Person> FirstByInsertOrder(int count = 1)
    {
        return personList.Take(count);
    }

    public IEnumerable<Person> SearchWithNameSize(int minLength, int maxLength)
    {
        return nameMap
            .Where(x => x.Key.Length <= maxLength && x.Key.Length >= minLength)
            .Select(x => x.Value)
            .SelectMany(personSet => personSet);
    }

    public IEnumerable<Person> GetWithNameSize(int length)
    {
        var result = nameMap
            .Where(x => x.Key.Length == length)
            .Select(x => x.Value)
            .SelectMany(personSet => personSet);

        if (!result.Any())
        {
            throw new ArgumentException();
        }

        return result;
    }

    public IEnumerable<Person> PeopleByInsertOrder()
    {
        return personList;
    }

    public IEnumerator<Person> GetEnumerator()
    {
        return personList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}