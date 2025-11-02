using System.Collections;

namespace MyQueue;

sealed class MyQueue<T>: IEnumerable<T>
{
    private T[] _items;
    private int _capacity = 4;
    private int _size;
    private int _front;
    private int _rear;

    public MyQueue()
    {
        _items = new T[_capacity];
    }

    public MyQueue(int capacity)
    {
        if(capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        _items = new T[capacity];
    }

    private void Resize()
    {
        _capacity *= 2;
        T[] newItems = new T[_capacity];

        for (int i = 0; i < _size; i++)
        {
            newItems[i] = _items[(_front + i) % _capacity];
        }
        
        _items = newItems;
        _front = 0;
        _rear = _size;
    }
    
    public void Enqueue(T item)
    {
        if (_size == _capacity)
        {
            Resize();
        }
        
        _items[_rear] = item;
        
        _rear = (_rear + 1) % _capacity;
        _size++;
    }

    public T Dequeue()
    {
        if(_size == 0) throw new InvalidOperationException("Queue is empty");
            
        T item = _items[_front];
        
        _front = (_front + 1) % _capacity;
        _size--;
        
        return item;
    }
    
    public T Peek()
    {
        if (_size == 0)
            throw new InvalidOperationException("Queue is empty.");
        
        return _items[_front];
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _size; i++)
        {
            yield return _items[(_front + i) % _capacity];
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
        MyQueue<int> myQueue = new MyQueue<int>();
        
        myQueue.Enqueue(1);
        myQueue.Enqueue(2);
        myQueue.Enqueue(3);
        myQueue.Enqueue(4);
        
        foreach (var item in myQueue)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();

        Console.WriteLine(myQueue.Dequeue());
        myQueue.Enqueue(5);
        
        Console.WriteLine(myQueue.Peek());
        Console.WriteLine(myQueue.Dequeue());
        myQueue.Enqueue(6);
        
        foreach (var item in myQueue)
        {
            Console.Write(item + " "); 
        }
        Console.WriteLine();
        
    }
}