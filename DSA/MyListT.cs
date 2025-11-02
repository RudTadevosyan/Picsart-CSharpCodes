using System.Collections;

namespace MyListT;

static class ExtensionMethods
{
    public static void Shuffle<T>(this  MyList<T> list)
    {
        if(list == null) throw new InvalidOperationException();
        if(list.IsReadOnly) throw new InvalidOperationException();
        if(list.Count < 2) return;
        
        Random rng = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            
            (list[i], list[j]) = (list[j], list[i]);
        }
        
    }

    public static void Reverse<T>(this MyList<T> list)
    {
        if( list == null ) throw new InvalidOperationException();
        if(list.IsReadOnly) throw new InvalidOperationException();
        
        if( list.Count < 2 ) return;

        for (int i = 0; i < list.Count / 2; i++)
        {
            (list[i], list[list.Count - i - 1]) = (list[list.Count - i - 1], list[i]);
        }
        
    }

    public static MyList<T> Slice<T>(this MyList<T> list, int start, int end)
    {
        if( list == null ) throw new InvalidOperationException();
        if(list.IsReadOnly) throw new InvalidOperationException();
        if(end < 0 || start > list.Count) throw new ArgumentOutOfRangeException();
        
        if( start < 0 ) start = 0;
        if( end > list.Count ) end = list.Count;
        
        MyList<T> res = new MyList<T>();

        for (int i = start; i < end; i++)
        {
            res.Add(list[i]);
        }
        
        return res;
    }

    public static T At<T>(this MyList<T> list, int index)
    {
        if(list == null) throw new InvalidOperationException();
        if (index < 0)
        {
            index = index + list.Count;
        }

        if (index < 0 || index >= list.Count)
        {
            throw new IndexOutOfRangeException();
        }
        
        
        return list[index];
    }
    
}
sealed class MyList<T>: IList<T>, ICloneable
{
    public int Count { get; private set;}
    public bool IsReadOnly { get; private set;}
    private int _capacity = 8;
    private T[] _items;
    
    public MyList()
    {
        _items = new T[_capacity];
        IsReadOnly = false;
    }

    public MyList(int capacity)
    {
        if(capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        _items = new T[capacity];
        _capacity = capacity;
        IsReadOnly = false;
    }

    public void MakeReadOnly()
    {
        IsReadOnly = true;
    }
    
    public int IndexOf(T item)
    {
        
        for (int i = 0; i < Count; i++)
        {
            if(_items[i] != null && EqualityComparer<T>.Default.Equals(_items[i], item)) return i;
        }
        
        return -1;
    }
    
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            return _items[index];
        }
        set
        {
            if (IsReadOnly) throw new NotSupportedException();
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            _items[index] = value;
        }
    }

    private void Resize()
    {
        if(IsReadOnly) throw new NotSupportedException();
        
        _capacity *= 2;
        T[] temp = new T[_capacity];

        for (int i = 0; i < Count; i++)
        {
            temp[i] = _items[i];
        }
        
        _items = temp;
    }
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return _items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        if (IsReadOnly) throw new NotSupportedException();
        
        if (Count >= _capacity)
        {
            Resize();
        }
        _items[Count] = item;
        Count++;
    }

    public void Clear()
    {
        if (IsReadOnly) throw new NotSupportedException();
        for (int i = 0; i < Count; i++)
        {
            _items[i] = default(T);
        }
        Count = 0;
    }

    public bool Contains(T item)
    {
        if(Count == 0) return false;

        for (int i = 0; i < Count; i++)
        {
            if(_items[i] != null && EqualityComparer<T>.Default.Equals(_items[i], item)) return true;
        }
        
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if(arrayIndex < 0 || array.Length - arrayIndex < Count) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        for (int i = 0; i < Count; i++)
        {
            array[arrayIndex + i] = _items[i];
        }
    }

    public bool Remove(T item)
    {
        if (IsReadOnly) throw new NotSupportedException();
        
        if(Count == 0) return false;
        
        int index = IndexOf(item);
        if(index < 0) return false;
            
        for(int i = index; i < Count - 1; i++) 
        { 
            _items[i] = _items[i + 1];
        }
        
        _items[--Count] = default(T); 
        return true;
        
    }

    public void Insert(int index, T item)
    {
        if (IsReadOnly) throw new NotSupportedException();
        if (index < 0 || index > Count) throw new IndexOutOfRangeException();

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

    public void RemoveAt(int index)
    {
        if (IsReadOnly) throw new NotSupportedException();
        if (index < 0 || index >= Count) throw new IndexOutOfRangeException();

        for (int i = index; i < Count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        
        _items[--Count] = default(T);
    }
    
    public object Clone()
    {
        MyList<T> copy = new MyList<T>(_capacity);
        for (int i = 0; i < Count; i++)
        {
            copy.Add(_items[i]);
        }
        
        if (IsReadOnly) copy.MakeReadOnly();
        return copy;
    }

    public int BinarySearch(T item)
    {
        if(item == null) throw new ArgumentNullException(nameof(item));
        if(!IsSorted()) throw new InvalidOperationException();
        if(!(item is IComparable<T> compItem)) throw new InvalidOperationException();
        
        
        return BinarySearchImp(compItem, 0, Count - 1);
    }

    private int BinarySearchImp(IComparable<T> item, int startIndex, int endIndex)
    {
        if (startIndex > endIndex) return -1;

        int midIndex = startIndex + (endIndex - startIndex) / 2;

        int comparison = item.CompareTo(_items[midIndex]);

        if (comparison < 0) return BinarySearchImp(item, startIndex, midIndex - 1);
        if (comparison > 0) return BinarySearchImp(item, midIndex + 1, endIndex);

        return midIndex;
    }

    public void Sort()
    {
        if (IsReadOnly) throw new NotSupportedException();
        if(Count < 2) return;

        for (int i = 0; i < Count; i++)
        {
            if (!(_items[i] is IComparable<T>)) throw new InvalidOperationException();
        }
        
        Array.Sort(_items, 0, Count);
    }

    private bool IsSorted()
    {
        if(Count < 2) return true;

        for (int i = 1; i < Count; i++)
        {
            if (_items[i - 1] is IComparable<T> num && _items[i] is IComparable<T>)
            {
                if (num.CompareTo(_items[i]) > 0) return false;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        return true;
    }

    public void PrintAll()
    {
        for (int i = 0; i < Count; i++)
        {
            Console.Write(_items[i] + " ");
        }

        Console.WriteLine();
    }
    
}

class Program
{
    static void Main()
    {
        MyList<int> list = new MyList<int>();
            
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            
            Console.WriteLine("Test 1 - Add");
            list.PrintAll();

            Console.WriteLine("Test 2 - Count");
            Console.WriteLine(list.Count);
            
            list.Remove(3);
            Console.WriteLine("Test 3 - Remove");
            list.PrintAll();
            Console.WriteLine(list.Count);
            Console.WriteLine(list.Contains(3));
            Console.WriteLine(list.Contains(4)); 

            Console.WriteLine("Test 4 - IndexOf");
            Console.WriteLine(list.IndexOf(2));
            Console.WriteLine(list.IndexOf(5));
            Console.WriteLine(list.IndexOf(6)); 

            Console.WriteLine("Test 5 - Insert");
            list.Insert(1, 10);
            list.PrintAll();

            Console.WriteLine("Test 6 - RemoveAt");
            list.RemoveAt(2);
            list.PrintAll();
            

            Console.WriteLine("Test 7 - Clone");
            MyList<int> clonedList = (MyList<int>)list.Clone();
            clonedList.PrintAll();
            Console.WriteLine(clonedList.Count); 

            Console.WriteLine("Test 8 - Shuffle");
            list.Shuffle();
            list.PrintAll();

            Console.WriteLine("Test 9 - Reverse");
            list.Reverse();
            list.PrintAll();

            Console.WriteLine("Test 10 - Slice");
            MyList<int> slicedList = list.Slice(1, 3);
            slicedList.PrintAll();

            Console.WriteLine("Test 11 - At");
            Console.WriteLine(list.At(1)); 
            Console.WriteLine(list.At(-1));
    }
}