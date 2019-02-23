using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class HashMap<K,V> : IEnumerable<KeyValue<K,V>>
{
    private HashTable<K, V> Map;
    public int Count => this.Map.Count;
    public int Capacity => this.Map.Capacity;

    public HashMap()
    {
        this.Map = new HashTable<K, V>();
    }

    public bool ContainsKey(K key)
    {
        return this.Map.ContainsKey(key);
    }

    public void Put(K key, V value)
    {
        this.Map.Add(key, value);
    }

    public void Remove(K key)
    {
        this.Map.Remove(key);
    }

    public V Get(K key)
    {
        return this.Map.Get(key);
    }

    public bool AddOrReplace(K key, V value)
    {
        return this.Map.AddOrReplace(key,value);
    }

    public void Clear()
    {
        this.Map.Clear();
    }

    public IEnumerable<K> Keys
    {
        get
        {
            return this.Select(x => x.Key);
        }
    }

    public V this[K key]
    {
        get
        {
            return this.Get(key);
        }
        set
        {
            this.AddOrReplace(key, value);
        }
    }

    public IEnumerator<KeyValue<K, V>> GetEnumerator()
    {
        foreach(var item in this.Map)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
