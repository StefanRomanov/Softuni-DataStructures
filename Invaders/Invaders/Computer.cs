using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using Wintellect.PowerCollections;

public class Computer : IComputer
{
    public int Energy { get; set; }
    private int TotalTurns { get; set; }

    private OrderedBag<LinkedListNode<Invader>> _invaders;
    private LinkedList<Invader> _byInsertion;

    public Computer(int energy)
    {
        if (energy < 0)
        {
            throw new ArgumentException();
        }

        TotalTurns = 0;
        Energy = energy;
        _invaders = new OrderedBag<LinkedListNode<Invader>>((a, b) => a.Value.CompareTo(b.Value));
        _byInsertion = new LinkedList<Invader>();
    }


    public void Skip(int turns)
    {
        TotalTurns += turns;
        var invader = new Invader(0, TotalTurns);
        var node = new LinkedListNode<Invader>(invader);
        var badInvaders = _invaders.RangeTo(node, true);

        foreach (var inv in badInvaders)
        {
            Energy -= inv.Value.Damage;
            _byInsertion.Remove(inv);
        }

        if (Energy < 0)
        {
            Energy = 0;
        }

        badInvaders.Clear();
    }

    public void AddInvader(Invader invader)
    {
        invader.Distance = TotalTurns + invader.Distance;

        var node = new LinkedListNode<Invader>(invader);

        _invaders.Add(node);
        _byInsertion.AddLast(node);
    }

    public void DestroyHighestPriorityTargets(int count)
    {
        if (_invaders.Count == 0)
        {
            return;
        }

        for (var i = 0; i < count; i++)
        {
            var invader = _invaders.RemoveFirst();
            _byInsertion.Remove(invader);
        }
    }

    public void DestroyTargetsInRadius(int radius)
    {
        var invader = new Invader(0, radius + TotalTurns);
        var node = new LinkedListNode<Invader>(invader);
        var badInvaders = _invaders.RangeTo(node, true);

        foreach (var inv in badInvaders)
        {
            _byInsertion.Remove(inv);
        }

        badInvaders.Clear();
    }

    public IEnumerable<Invader> Invaders()
    {
        return _byInsertion;
    }
}