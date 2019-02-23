using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Chainblock : IChainblock
{
    private Dictionary<TransactionStatus, LinkedList<Transaction>> _statusMap;
    private LinkedList<Transaction> _transactionsList;
    private Dictionary<int, LinkedListNode<Transaction>> _idMap;
    private Dictionary<string, OrderedBag<Transaction>> _receiverMap;

    public Chainblock()
    {
        _statusMap = new Dictionary<TransactionStatus, LinkedList<Transaction>>();
        _transactionsList = new LinkedList<Transaction>();
        _idMap = new Dictionary<int, LinkedListNode<Transaction>>();
        _receiverMap = new Dictionary<string, OrderedBag<Transaction>>();


        _statusMap.Add(TransactionStatus.Successfull, new LinkedList<Transaction>());
        _statusMap.Add(TransactionStatus.Failed, new LinkedList<Transaction>());
        _statusMap.Add(TransactionStatus.Aborted, new LinkedList<Transaction>());
        _statusMap.Add(TransactionStatus.Unauthorized, new LinkedList<Transaction>());
    }

    public int Count => _transactionsList.Count;

    public void Add(Transaction tx)
    {
        if (!_receiverMap.ContainsKey(tx.To))
        {
            _receiverMap[tx.To] = new OrderedBag<Transaction>();
        }

        var node = new LinkedListNode<Transaction>(tx);

        _statusMap[node.Value.Status].AddLast(tx);
        _transactionsList.AddLast(node);
        _idMap.Add(tx.Id, node);
        _receiverMap[tx.To].Add(tx);
    }

    public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
    {
        if (!Contains(id))
        {
            throw new ArgumentException();
        }

        var transaction = _idMap[id].Value;
        if (transaction.Status == newStatus)
        {
            return;
        }

        transaction.Status = newStatus;

    }

    public bool Contains(Transaction tx)
    {
        return Contains(tx.Id);
    }

    public bool Contains(int id)
    {
        return _idMap.ContainsKey(id);
    }

    public IEnumerable<Transaction> GetAllInAmountRange(double lo, double hi)
    {
        return _transactionsList.Where(x => x.Amount >= lo && x.Amount <= hi);
    }

    public IEnumerable<Transaction> GetAllOrderedByAmountDescendingThenById()
    {
        return _transactionsList.OrderByDescending(x => x.Amount).ThenBy(x => x.Id);
    }

    public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
    {
        var result = _statusMap[status];
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result.OrderByDescending(x => x.Amount).Select(x => x.To);
    }

    public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
    {
        var result = _statusMap[status];
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result.OrderByDescending(x => x.Amount).Select(x => x.From);
    }

    public Transaction GetById(int id)
    {
        if (!Contains(id))
        {
            throw new InvalidOperationException();
        }

        return _idMap[id].Value;
    }

    public IEnumerable<Transaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
    {
        var low = new Transaction(int.MaxValue, TransactionStatus.Aborted, "", "", lo);
        var high = new Transaction(0, TransactionStatus.Aborted, "", "", hi - 1);

        if (!_receiverMap.ContainsKey(receiver))
        {
            throw new InvalidOperationException();
        }

        var result = _receiverMap[receiver].Range(high, true, low, true);

        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<Transaction> GetByReceiverOrderedByAmountThenById(string receiver)
    {
        if (!_receiverMap.ContainsKey(receiver))
        {
            throw new InvalidOperationException();
        }

        var result = _receiverMap[receiver];

        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<Transaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
    {
        var result = _transactionsList
            .Where(x => x.From == sender && x.Amount > amount)
            .OrderByDescending(x => x.Amount)
            .ToList();

        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<Transaction> GetBySenderOrderedByAmountDescending(string sender)
    {
        

        var result = _transactionsList
            .Where(x => x.From == sender)
            .OrderByDescending(x => x.Amount)
            .ToList();
        
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<Transaction> GetByTransactionStatus(TransactionStatus status)
    {
        var result = _statusMap[status];
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result.OrderByDescending(x => x.Amount);
    }

    public IEnumerable<Transaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
    {
        return _statusMap[status].Where(x => x.Amount <= amount).OrderByDescending(x => x.Amount);
    }

    public IEnumerator<Transaction> GetEnumerator()
    {
        return _transactionsList.GetEnumerator();
    }

    public void RemoveTransactionById(int id)
    {
        if (!Contains(id))
        {
            throw new InvalidOperationException();
        }

        var node = _idMap[id];
        var transaction = node.Value;

        _transactionsList.Remove(transaction);
        _idMap.Remove(id);
        _statusMap[transaction.Status].Remove(transaction);
        _receiverMap[transaction.To].Remove(transaction);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}