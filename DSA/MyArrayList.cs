using System.Collections;

namespace MyArrayList;

sealed class ArrayList:IEnumerable, ICloneable
{
    private int Count { get; set; }
    private int _capacity = 8;
    private object[] _items;

    public ArrayList()
    {
        _items = new object[_capacity];
    }

    public ArrayList(int capacity)
    {
        _capacity = capacity;
        _items = new object[capacity];
    }

    public object this[int index]
    {
        get
        {
            if(index < 0 || index >= Count) throw new IndexOutOfRangeException();
            return _items[index];
        }
        set
        {
            if(index < 0 || index >= Count) throw new IndexOutOfRangeException();
            _items[index] = value;
        }
    }
    public void Resize()
    {
        _capacity *= 2;
        object[] tmp = new object[_capacity];

        for (int i = 0; i < Count; i++)
        {
            tmp[i] = _items[i];
        }

        _items = tmp;
    }

    public void Add(object item)
    {
        if (Count >= _capacity)
        {
            _capacity *= 2;
            Resize();
        }
        
        _items[Count] = item;
        Count++;
    }

    public int IndexOf(object item)
    {

        if (item == null) throw new ArgumentNullException(nameof(item));
        
        for (int i = 0; i < Count; i++)
        {
            if(_items[i].Equals(item)) return i;
        }
        
        return -1;
    }

    public void Remove(object item)
    {
        int index = IndexOf(item);
        if (index < 0) return;
        
        for (int i = index; i < Count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        
        _items[--Count] = null!;
        
    }
    public void RemoveAt(int index)
    {
        if(Count == 0) return;
        if(index < 0 || index >= Count) throw new IndexOutOfRangeException();

        for (int i = index; i < Count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        
        _items[--Count] = null!;
    }

    public bool Contains(object item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        
        if(Count == 0) return false;
        for (int i = 0; i < Count; i++)
        {
            if(_items[i].Equals(item)) return true; 
        }
        return false;
    }
    
    public void InsertAt(int index, object item)
    {
        if(index < 0 || index > Count) throw new IndexOutOfRangeException();
        if (Count >= _capacity)
        {
            Resize();
        }

        if (index == Count)
        {
            _items[Count++] = item;
            return;
        }
        
        for (int i = Count; i > index; i--)
        {
            _items[i] = _items[i - 1];
        }
        
        _items[index] = item;
        Count++;
    }
    
    public void Print()
    {
        if (Count == 0)
        {
            Console.WriteLine("Empty list");
            return;
        }
        
        for (int i = 0; i < Count; i++)
        {
            Console.Write($"{_items[i]} ");
        }
        Console.WriteLine();
    }
    
    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return _items[i];
        }
    }

    public object Clone()
    {
        ArrayList cloneList = new ArrayList(_capacity);
        for (int i = 0; i < Count; i++)
        {
            cloneList.Add(_items[i]);
        }
        
        return cloneList;
    }
    
}

class Program
{
    static void Main()
    {
        ArrayList arrayList = new ArrayList();
        arrayList.Add(38);
        arrayList.Add(27);
        arrayList.Add(43);
        arrayList.Add(3);
        arrayList.Add(9);
        arrayList.Add(82);
        arrayList.Add(10);
        
        arrayList.Remove(9);
        arrayList.RemoveAt(2);
        
        arrayList.InsertAt(5,'a');

        Console.WriteLine(arrayList.Contains(7));
        Console.WriteLine(arrayList.Contains('a'));
        
        arrayList.Print();
        
    }
}