namespace HashTable;

sealed class Node<T>
{
    private T? _value;
    private Node<T>? _next;

    public T Value { get => _value; set => _value = value; }
    public Node<T> Next { get => _next; set => _next = value; }
    
    public Node()
    {
        _value = default(T);
        _next = null;
    }

    public Node(T value, Node<T> next)
    {
        _value = value;
        this._next = next;
    }
}

sealed class MyHashSet
{
    private Node<int>[] _array;
    private int _count;
    private int _capacity = 7;
    private readonly double _threshold = 0.55;

    public int Size => _count;

    public MyHashSet()
    {
        _array = new Node<int>[_capacity];
    }

    private int GetIndex(int key)
    {
        return GetHash(key) % _capacity;
    }

    private int GetHash(int key) 
    {
        uint x = (uint)key;
        x = (x ^ 61) ^ (x >> 16);
        x = x + (x << 3);
        x = x ^ (x >> 4);
        x = x * 0x27d4eb2d;
        x = x ^ (x >> 15);
        
        return (int)(x & 0x7fffffff);
    }

    private double GetLoadFactor()
    {
        return (double)_count/_capacity;
    }

    private void ReHashing()
    {
        _capacity *= 2; // better would be if it is the closest prime number to the double of it
        Node<int>[] newArray = new Node<int>[_capacity];

        for (int i = 0; i < _array.Length; i++)
        {
            Node<int> curr = _array[i];

            while (curr != null)
            {
                Node<int> next = curr.Next;
                
                int index = GetIndex(curr.Value);
                curr.Next = newArray[index];
                newArray[index] = curr;
                
                curr = next;
            }
        }
        
        _array = newArray;
    }
    
    public void Add(int item)
    {
        if (Contains(item)) return;
        if (GetLoadFactor() > _threshold)
        {
            ReHashing();
        }
        
        int index = GetIndex(item);

        
        Node<int> newNode = new Node<int>(item, _array[index]);

        _array[index] = newNode;
        _count++;
    }

    public bool Remove(int item)
    {
        int index = GetIndex(item);
        Node<int> curr = new Node<int>(0, _array[index]);
        
        if (curr.Next == null) return false;
        
        while(curr.Next != null)
        {
            if (curr.Next.Value.Equals(item))
            {
                curr.Next = curr.Next.Next;
                _count--;
                return true;
            }
            curr = curr.Next;
        }

        return false;
    }

    public bool Contains(int item)
    {
        int index = GetIndex(item);
        Node<int> curr = _array[index];
        
        while(curr != null)
        {
            if (curr.Value.Equals(item))
            {
                return true;
            }
            curr = curr.Next;
        }
        return false;
    }
    
}

class Program
{
    static void Main()
    {
        MyHashSet set = new MyHashSet();

        set.Add(5);
        set.Add(10);
        set.Add(15);
        set.Add(5);    // duplicate, should not be added again

        Console.WriteLine("Contains 10? " + set.Contains(10)); // True
        Console.WriteLine("Contains 7? " + set.Contains(7));   // False

        set.Remove(10);
        Console.WriteLine("Contains 10 after remove? " + set.Contains(10)); // False

        set.Add(20);
        set.Add(25);

        Console.WriteLine("Contains 20? " + set.Contains(20)); // True
        Console.WriteLine("Contains 25? " + set.Contains(25)); // True

        Console.WriteLine("Trying to remove 30 (not in set):");
        set.Remove(30); // Should not crash

        Console.WriteLine("Done testing.");
    }
}