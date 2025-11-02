namespace Dictionary;

sealed class Node<T>
{
    public string Key;
    public T? Value;
    public Node<T>? Next;
    public Node(string key ,T value, Node<T>? next = null)
    {
        this.Key = key;
        this.Value = value;
        this.Next = next;
    }
}

sealed class MyDictionary<K,T>
{
    private Node<T>?[] _buckets;
    private int _capacity = 7;
    private int _count;
    
    private readonly double _threshold = 0.55;

    public MyDictionary()
    {
        _buckets = new Node<T>?[_capacity];
    }

    private double GetLoadFactor()
    {
        return (double)_count / _capacity;
    }
    
    private int GetIndex(string key)
    {
        return GetHash(key) % _capacity;
    }

    private int GetReHashIndex(string key, int capacity)
    {
        return GetHash(key) % capacity;
    }

    private int GetHash(string key) //DJB2 Hash Function
    {
        ulong hash = 5381;
        foreach (char c in key)
        {
            hash = ((hash << 5) + hash) + c;
        }
        
        return (int)(hash & 0x7FFFFFFF);
    }

    private void ReHashing()
    {
        int newCapacity = _capacity * 2;
        Node<T>?[] newBuckets = new Node<T>?[newCapacity];

        for (int i = 0; i < _buckets.Length; i++)
        {
            Node<T>? curr = _buckets[i];

            while (curr != null)
            {
                Node<T>? next = curr.Next;
                int newIndex = GetReHashIndex(curr.Key, newCapacity);
                curr.Next = newBuckets[newIndex];
                newBuckets[newIndex] = curr;
                
                curr = next;
            }
        }
        
        _capacity = newCapacity;
        _buckets = newBuckets;
    }

    public bool ContainsKey(string key)
    {
        if (key == null) return false;
        
        int index = GetIndex(key);
        
        Node<T>? curr = _buckets[index];
        while (curr != null)
        {
            if (curr.Key == key) return true;
            curr = curr.Next;
        }
        
        return false;
    }
    
    public void Put(string key, T value)
    {
        if (GetLoadFactor() > _threshold)
        {
            ReHashing();
        }
        
        int index = GetIndex(key);
        Node<T> newNode = new Node<T>(key, value, _buckets[index]);
        _buckets[index] = newNode;
        _count++;
    }

    public bool Remove(string key)
    {
        if (key == null) return false;
        
        int index = GetIndex(key);
        Node<T>? curr = _buckets[index];
        Node<T>? prev = null;

        while (curr != null)
        {
            if (curr.Key == key)
            {
                if (prev == null)
                    _buckets[index] = curr.Next;
                else
                    prev.Next = curr.Next;

                _count--;
                return true;
            }

            prev = curr;
            curr = curr.Next;
        }

        return false;

    }

    public T? Get(string key)
    {
        if (key == null) return default;
        int index = GetIndex(key);
        Node<T>? curr = _buckets[index];

        while (curr != null)
        {
            if (curr.Key == key)
            {
                return curr.Value;
            }
            
            curr = curr.Next;
        }
        
        return default;
    }
    
}

class Program
{
    static void Main()
    {
        var dict = new MyDictionary<string, int>();

        dict.Put("apple", 5);
        dict.Put("banana", 10);
        dict.Put("cherry", 15);

        Console.WriteLine("Get 'apple': " + dict.Get("apple"));   // Should print 5
        Console.WriteLine("Contains 'banana'? " + dict.ContainsKey("banana")); // True
        Console.WriteLine("Contains 'orange'? " + dict.ContainsKey("orange")); // False

        dict.Put("banana", 20); // Update existing key
        Console.WriteLine("Updated 'banana': " + dict.Get("banana")); // Should print 20

        dict.Remove("cherry");
        Console.WriteLine("Contains 'cherry'? " + dict.ContainsKey("cherry")); // False

        // Add more to trigger rehashing
        dict.Put("grape", 100);
        dict.Put("melon", 200);
        dict.Put("kiwi", 300);
        dict.Put("pear", 400);
        dict.Put("plum", 500);

        Console.WriteLine("Get 'melon': " + dict.Get("melon")); // Should work after rehash

        try
        {
            Console.WriteLine("Try get 'cherry' (removed): " + dict.Get("cherry"));
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("'cherry' not found (as expected)");
        }

        Console.WriteLine("Done testing.");
    }
}