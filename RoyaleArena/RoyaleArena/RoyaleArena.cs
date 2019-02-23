using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class RoyaleArena : IArena
{
    private readonly LinkedList<Battlecard> _cardList;
    private readonly Dictionary<int, LinkedListNode<Battlecard>> _idMap;
    private readonly Dictionary<CardType, OrderedBag<Battlecard>> _typeMap;
    private readonly Dictionary<string, OrderedBag<Battlecard>> _nameMap;
    private readonly OrderedBag<Battlecard> _swagBag;

    public RoyaleArena()
    {
        _cardList = new LinkedList<Battlecard>();
        _idMap = new Dictionary<int, LinkedListNode<Battlecard>>();
        _typeMap = new Dictionary<CardType, OrderedBag<Battlecard>>();
        _nameMap = new Dictionary<string, OrderedBag<Battlecard>>();
        _swagBag = new OrderedBag<Battlecard>(swagAsc);

        _typeMap.Add(CardType.MELEE, new OrderedBag<Battlecard>());
        _typeMap.Add(CardType.RANGED, new OrderedBag<Battlecard>());
        _typeMap.Add(CardType.BUILDING, new OrderedBag<Battlecard>());
        _typeMap.Add(CardType.SPELL, new OrderedBag<Battlecard>());
    }

    public int Count => _cardList.Count;

    public void Add(Battlecard card)
    {
        if (_idMap.ContainsKey(card.Id))
        {
            throw new ArgumentException();
        }

        if (!_nameMap.ContainsKey(card.Name))
        {
            _nameMap.Add(card.Name, new OrderedBag<Battlecard>(swagDesc));
        }

        LinkedListNode<Battlecard> node = CreateNode(card);
        
        
        _typeMap[card.Type].Add(card);
        _swagBag.Add(card);
        _nameMap[card.Name].Add(card);
        _idMap.Add(card.Id, node);
        _cardList.AddLast(node);
    }

    public void ChangeCardType(int id, CardType type)
    {
        if (!_idMap.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        var card = _idMap[id].Value;

        if (card.Type == type)
        {
            return;
        }

        card.Type = type;
    }

    public bool Contains(Battlecard card)
    {
        return _idMap.ContainsKey(card.Id);
    }

    public IEnumerable<Battlecard> FindFirstLeastSwag(int n)
    {
        if (n > Count)
        {
            throw new InvalidOperationException();
        }

        return _swagBag.Take(n);
    }

    public IEnumerable<Battlecard> GetAllByNameAndSwag()
    {
        var list = new List<Battlecard>();

        foreach (var key in _nameMap.Keys)
        {
            list.Add(_nameMap[key].GetFirst());
        }

        return list;
    }

    public IEnumerable<Battlecard> GetAllInSwagRange(double lo, double hi)
    {
        var low = new Battlecard(0, CardType.BUILDING, "lo", 0, lo);
        var high = new Battlecard(0, CardType.BUILDING, "hig", 0, hi);

        return _swagBag.Range(low, true, high, true);
    }

    public IEnumerable<Battlecard> GetByCardType(CardType type)
    {
        if (_typeMap[type].Count == 0)
        {
            throw new InvalidOperationException();
        }

        return _typeMap[type];
    }

    public IEnumerable<Battlecard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
    {
        if (Count == 0)
        {
            throw new InvalidOperationException();
        }

        var cards = _typeMap[type].RangeFrom(new Battlecard(0, CardType.RANGED, "aha", damage, 15), true);

        if (cards.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return cards;
    }

    public Battlecard GetById(int id)
    {
        if (Count == 0)
        {
            throw new InvalidOperationException();
        }

        if (!_idMap.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        return _idMap[id].Value;
    }

    public IEnumerable<Battlecard> GetByNameAndSwagRange(string name, double lo, double hi)
    {
        var low = new Battlecard(0, CardType.BUILDING, "lo", 0, lo);
        var high = new Battlecard(0, CardType.BUILDING, "high", 0, hi);

        if (_nameMap[name].Count == 0)
        {
            throw new InvalidOperationException();
        }

        return _nameMap[name].Range(high, true, low, false);
    }

    public IEnumerable<Battlecard> GetByNameOrderedBySwagDescending(string name)
    {
        if (Count == 0)
        {
            throw new InvalidOperationException();
        }

        if (!_nameMap.ContainsKey(name))
        {
            throw new InvalidOperationException();
        }

        return _nameMap[name];
    }

    public IEnumerable<Battlecard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
    {
        if (Count == 0)
        {
            throw new InvalidOperationException();
        }

        if (_typeMap[type].Count == 0)
        {
            throw new InvalidOperationException();
        }

        var low = new Battlecard(0, CardType.BUILDING, "lo", lo, 0);
        var high = new Battlecard(0, CardType.BUILDING, "high", hi, 0);

        return _typeMap[type].Range(high, true, low, false);
    }

    public IEnumerator<Battlecard> GetEnumerator()
    {
        return this._cardList.GetEnumerator();
    }

    public void RemoveById(int id)
    {
        LinkedListNode<Battlecard> node = _idMap[id];

        _idMap.Remove(id);
        _cardList.Remove(node);
        _nameMap[node.Value.Name].Remove(node.Value);
        _swagBag.Remove(node.Value);
        _typeMap[node.Value.Type].Remove(node.Value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private LinkedListNode<Battlecard> CreateNode(Battlecard card)
    {
        return new LinkedListNode<Battlecard>(card);
    }

    private Comparison<Battlecard> swagDesc =
        (a, b) =>
        {
            var compare = b.Swag.CompareTo(a.Swag);
            if (compare == 0)
            {
                return a.Id.CompareTo(b.Id);
            }

            return compare;
        };
    
    private Comparison<Battlecard> swagAsc =
        (x, y) =>
        {
            var compare = x.Swag.CompareTo(y.Swag);

            if(compare == 0)
            {
                return x.Id.CompareTo(y.Id);
            }

            return compare;
        };
}