using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enterprise : IEnterprise
{
    
    private Dictionary<Guid, Employee> idMap;

    public Enterprise()
    {
        idMap = new Dictionary<Guid, Employee>();
    }

    public int Count => idMap.Count;
    

    public void Add(Employee employee)
    {
        if (idMap.ContainsKey(employee.Id))
        {
            throw new ArgumentException();
        }
        idMap.Add(employee.Id,employee);
    }

    public IEnumerable<Employee> AllWithPositionAndMinSalary(Position position, double minSalary)
    {
        var result = idMap.Values.Where(x => x.Position == position && x.Salary >= minSalary);
        
        if (result.Any())
        {
            return result;
        }
        
        return new List<Employee>();
    }

    public bool Change(Guid guid, Employee employee)
    {
        if (Contains(guid))
        {
            idMap[guid] = employee;
            
            return true;
        }

        return false;
    }

    public bool Contains(Guid guid)
    {
        return idMap.ContainsKey(guid);
    }

    public bool Contains(Employee employee)
    {
        return idMap.ContainsKey(employee.Id);
    }

    public bool Fire(Guid guid)
    {
        if (idMap.ContainsKey(guid))
        {
            idMap.Remove(guid);
        }

        return true;
    }

    public Employee GetByGuid(Guid guid)
    {
        if (Contains(guid))
        {
            return idMap[guid];
        }
        
        throw new ArgumentException();
    }

    public IEnumerable<Employee> GetByPosition(Position position)
    {
        return idMap.Values.Where(x => x.Position == position);
    }

    public IEnumerable<Employee> GetBySalary(double minSalary)
    {
        var result = idMap.Values.Where(x => x.Salary >= minSalary);
        
        if (result.Any())
        {
            return result;
        }
        
        throw new InvalidOperationException();
    }

    public IEnumerable<Employee> GetBySalaryAndPosition(double salary, Position position)
    {
        var result = idMap.Values.Where(x => x.Salary.Equals(salary) && x.Position == position);
        
        if (result.Any())
        {
            return result;
        }
        
        throw new InvalidOperationException();
    }

    public Position PositionByGuid(Guid guid)
    {
        if (Contains(guid))
        {
            return idMap[guid].Position;
        }
        
        throw new InvalidOperationException();
    }

    public bool RaiseSalary(int months, int percent)
    {
        var result = false;
        foreach (var keyValuePair in idMap)
        {
            if (keyValuePair.Value.HireDate.AddMonths(months) <= DateTime.Now)
            {
                keyValuePair.Value.Salary += keyValuePair.Value.Salary * (percent / 100.0);
                result = true;
            }
        }

        return result;
    }

    public IEnumerable<Employee> SearchByFirstName(string firstName)
    {
        var result = idMap.Values.Where(x => x.FirstName == firstName);
        
        if (result.Any())
        {
            return result;
        }
        
        return new List<Employee>();
    }

    public IEnumerable<Employee> SearchByNameAndPosition(string firstName, string lastName, Position position)
    {
        var result = idMap.Values.Where(x => x.FirstName == firstName && x.LastName == lastName && x.Position == position);
        
        if (result.Any())
        {
            return result;
        }
        
        return new List<Employee>();
    }

    public IEnumerable<Employee> SearchByPosition(IEnumerable<Position> positions)
    {
        var result = idMap.Values.Where(x => positions.Contains(x.Position));
        
        if (result.Any())
        {
            return result;
        }
        
        return new List<Employee>();
    }

    public IEnumerable<Employee> SearchBySalary(double minSalary, double maxSalary)
    {
        var result = idMap.Values.Where(x => x.Salary >= minSalary && x.Salary <= maxSalary);
        
        if (result.Any())
        {
            return result;
        }
        
        return new List<Employee>();
    }  
    
    public IEnumerator<Employee> GetEnumerator()
    {
        foreach (var kvp in idMap)
        {
            yield return kvp.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }  
}

