using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class ThreadExecutor : IScheduler
{
    private int TotalCycles { get; set; }
    private readonly Dictionary<int, Task> _idMap;
    private readonly LinkedList<Task> _taskList;
    private readonly Dictionary<Priority, OrderedBag<Task>> _priorityMap;
    private readonly OrderedBag<LinkedListNode<Task>> _consumptionBag;

    public ThreadExecutor()
    {
        TotalCycles = 0;
        _idMap = new Dictionary<int, Task>();
        _taskList = new LinkedList<Task>();
        _priorityMap = new Dictionary<Priority, OrderedBag<Task>>();
        _consumptionBag = new OrderedBag<LinkedListNode<Task>>(
            (x, y) => x.Value.CompareTo(y.Value)
            );
        
        _priorityMap.Add(Priority.LOW, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        _priorityMap.Add(Priority.MEDIUM, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        _priorityMap.Add(Priority.HIGH, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        _priorityMap.Add(Priority.EXTREME, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
    }

    public int Count => _taskList.Count;


    public void ChangePriority(int id, Priority newPriority)
    {
        if (!_idMap.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        var task = _idMap[id];
        
        if (newPriority == task.TaskPriority)
        {
            return;
        }

        task.TaskPriority = newPriority;
        _priorityMap[task.TaskPriority].Remove(task);
        _priorityMap[newPriority].Add(task);
    }

    public bool Contains(Task task)
    {
        return _idMap.ContainsKey(task.Id);
    }

    public int Cycle(int cycles)
    {        
        if(Count == 0)
        {
            throw new InvalidOperationException();
        }

        TotalCycles += cycles; 
        
        var firstNode = new LinkedListNode<Task>(new Task(5,TotalCycles - cycles , Priority.EXTREME));
        var secondNode = new LinkedListNode<Task>(new Task(7,TotalCycles, Priority.LOW));
        
        

        var dead = _consumptionBag.Range(firstNode, false, secondNode , true);
        

        var result = dead.Count;

        foreach(var node in dead)
        {
            var task = node.Value;
            
            _taskList.Remove(task);
            _priorityMap[task.TaskPriority].Remove(task);
            _idMap.Remove(task.Id);
        }
        
        dead.Clear();

        return result;
    }

    public void Execute(Task task)
    {
        if (_idMap.ContainsKey(task.Id))
        {
            throw new ArgumentException();
        }
        
        var node = new LinkedListNode<Task>(task);

        _priorityMap[task.TaskPriority].Add(task);
        _consumptionBag.Add(node);
        _idMap.Add(task.Id, task);
        _taskList.AddLast(task);
    }

    public IEnumerable<Task> GetByConsumptionRange(int lo, int hi, bool inclusive)
    {
        LinkedListNode<Task> low = new LinkedListNode<Task>(new Task(5, lo + TotalCycles, inclusive ? Priority.EXTREME : Priority.LOW));
        LinkedListNode<Task> high = new LinkedListNode<Task>(new Task(7, hi + TotalCycles, inclusive ? Priority.LOW : Priority.EXTREME));
        
        return _consumptionBag.Range(low,inclusive, high, inclusive )
            .Select(x => x.Value);
    }

    public Task GetById(int id)
    {
        if (!_idMap.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        return _idMap[id];
    }

    public Task GetByIndex(int index)
    {
        if (_taskList.Count <= index)
        {
            throw new ArgumentOutOfRangeException();
        }
        return _taskList.ElementAt(index);
    }

    public IEnumerable<Task> GetByPriority(Priority type)
    {
        return _taskList.Where(x => x.TaskPriority == type).OrderByDescending(x => x.Id);
    }

    public IEnumerable<Task> GetByPriorityAndMinimumConsumption(Priority priority, int lo) { 

        return _taskList.Where(x => x.TaskPriority == priority && x.Consumption >= lo).OrderByDescending(x => x.Id);
    }


    public IEnumerator<Task> GetEnumerator()
    {
        foreach (var task in _taskList)
        {
            yield return task;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}