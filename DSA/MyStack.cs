using System.Collections;

namespace MyStack;

sealed class MyStack<T>: IEnumerable<T>
{
    private T[] _items;
    private int _count;
    private int _capacity = 4;

    public int Count => _count;

    public MyStack()
    {
        _items = new T[_capacity];
    }

    public MyStack(int capacity)
    {
        if(capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        _capacity = capacity;
        _items = new T[_capacity];
    }

    private void Resize()
    {
        _capacity *= 2;
        T[] newItems = new T[_capacity];
        
        Array.Copy(_items, newItems, _count);
        _items = newItems;
    }
    
    public void Push(T item)
    {
        if (_count >= _capacity)
        {
            Resize();
        }
        
        _items[_count] = item;
        _count++;
    }

    public T Pop()
    {
        if(_count <= 0) throw new InvalidOperationException();
        T pop = _items[--_count];
        _items[_count] = default(T);
        
        return pop;
    }

    public T Peek()
    {
        if(_count <= 0) throw new InvalidOperationException();
        return _items[_count - 1];
    }

    public bool Contains(T item)
    {
        if(item == null) throw new ArgumentNullException(nameof(item));

        for (int i = 0; i < _count; i++)
        {
            if(EqualityComparer<T>.Default.Equals(_items[i], item)) return true;
        }
        
        return false;
    }

    public T[] ToArray()
    {
        T[] array = new T[_count];
        
        Array.Copy(_items, array, _count);
        return array;
    }

    public void Clear()
    {
        for (int i = 0; i < _count; i++)
        {
            _items[i] = default(T);
        }

        _count = 0;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0 ; i--)
        {
            yield return _items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Program
{
    static void Main()
    {
        MyStack<int> stack = new MyStack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);

        Console.WriteLine(stack.Pop()); 
        Console.WriteLine(stack.Peek()); 
        Console.WriteLine(stack.Contains(2));
        
        foreach (var i in stack.ToArray())
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();
        
        stack.Clear();
        Console.WriteLine(stack.Count);
    }
}