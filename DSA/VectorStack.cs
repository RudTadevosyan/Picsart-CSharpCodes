using System.Collections;

namespace MyVectorStack;

sealed class MyStack<T>
{
    private MyVector<T> _vector;

    public MyStack()
    {
        _vector = new MyVector<T>();
    }

    public void Push(T value)
    {
        _vector.Push(value);
    }

    public T Pop()
    {
        return _vector.Pop();
    }

    public T Peek()
    {
        return _vector.Peek();
    }

    public int Size()
    {
        return _vector.Size;
    }

    public bool Empty()
    {
        return Size() == 0;
    }
    
}
sealed class MyVector<T> : IEnumerable<T>, ICloneable
{
   private T[] _array = [];
   private int _size;
   private int _capacity;

   public int Size => _size;
   public int Capacity => _capacity;

   public T this[int index]
   {
       get
       {
           if(index < 0 || index >= _size) throw new IndexOutOfRangeException();
           return _array[index];
       }
       set
       {
           if(index < 0 || index >= _size) throw new IndexOutOfRangeException();
           _array[index] = value;
       }
   }

   private void Resize(int newSize)
   {
       T[] newArray = new T[newSize];
       int length = (newSize > _size) ? _size : newSize;
       Array.Copy(_array, newArray, length);
       
       _array = newArray;
       _capacity = newSize;
   }
   
   public void Push(T item, int count = 1)
   {
       if ((_capacity - _size) < count)
       {
           int newSize = (count == 1) ? _capacity * 2 : _capacity * 2 + count;
           if (_capacity == 0) newSize = count;
           Resize(newSize);
       }

       for (int i = 0; i < count; i++)
       {
           _array[_size++] = item;
       }
       
   }
   private void ResizeWithIndex(int newSize, int index, T item, int count)
   {
       T[] newArray = new T[newSize];
       for (int i = 0; i < index; i++) //before index
       {
           newArray[i] = _array[i];
       }

       for (int i = index; i < index + count; i++) //insert 
       {
           newArray[index] = item;
       }

       for (int i = index; i < _size - index ; i++) //after index copy
       {
           newArray[i] = _array[i];
       }
       
       _array = newArray;
       _capacity = newSize;
   }

   public void Insert(T item, int index, int count = 1)
   {
       if(index < 0 || index > _size) throw new IndexOutOfRangeException();
       if ((_capacity - _size) < count)
       {
           int newSize = (count == 1) ? _capacity * 2 : _capacity * 2 + count;
           if (_capacity == 0) newSize = count;
           ResizeWithIndex(newSize, index, item, count);
       }
       else
       {
           for (int i = _size - 1 ; i >= index; i--)
           {
               _array[i + count] = _array[i];
           }

           for (int i = index; i < index + count; i++)
           {
               _array[i] = item;
           }
       }
       
       _size += count;
   }

   public T Pop()
   {
       if (_size <= 0) throw new IndexOutOfRangeException();
       T item = _array[--_size];
       return item;
   }

   public T Peek()
   {
       if (_size <= 0) throw new IndexOutOfRangeException();
       return _array[_size - 1];
   }

   public void Erase(int index)
   {
       if(_size <= 0 || _size <= index || index < 0) throw new IndexOutOfRangeException(); // [1-index)

       for (int i = index; i < _size - 1; i++)
       {
           _array[i] = _array[i + 1];
       }
       
       _size--;
       
   }

   public void Shrink()
   {
       if (_size <= 0) throw new IndexOutOfRangeException();
       Resize(_size);
   }
   public void Print()
   {
       if(_size <= 0) return;
       
       for (int i = 0; i < _size; i++)
       {
           Console.Write($"{_array[i]} ");
       }
       Console.WriteLine();
   }

   public void ResizeVal(T item, int count)
   {
       if(count < 0) throw new IndexOutOfRangeException();
       Resize(count);
       for (int i = _size; i < _capacity; i++) 
       { 
           _array[i] = item;
       }
       _size = _capacity;
   }

   public void Clear()
   {
       _size = 0;
       _capacity = 0;
       _array = [];
   }
   public IEnumerator<T> GetEnumerator()
   {
       for (int i = 0; i < _size; i++)
       {
           yield return _array[i];
       }
   }

   IEnumerator IEnumerable.GetEnumerator()
   { 
       return GetEnumerator();
   }

   public object Clone()
   {
       MyVector<T> clone = new MyVector<T>
       {
           _size = this._size,
           _capacity = this._capacity,
           _array = new T[this._capacity]
       };
       
       Array.Copy(_array, clone._array, _size);
       return clone;
   }
}

class Program
{
    static void Main() { }
}