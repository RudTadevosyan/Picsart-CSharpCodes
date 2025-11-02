namespace BinaryHeap;

sealed class MyBinaryHeap<T> where T : IComparable<T>
{
    private T[] _array;
    private int _count;
    private int _capacity = 2;

    public int Count => _count;
    public bool IsEmpty => _count == 0;

    public MyBinaryHeap()
    {
        _array = new T[_capacity];
    }

    public MyBinaryHeap(params T[] array)
    {
        _array = array;
        _capacity = array.Length * 2;
        _count = array.Length;
        BuildMaxHeap();
    }

    private void Resize()
    {
        T[] newArray = new T[_capacity * 2];
        Array.Copy(_array, newArray, _count);

        _capacity *= 2;
        _array = newArray;
    }

    private void Heapify(int index, int size)
    {
        int largest = index;
        int leftIndex = 2 * index + 1;
        int rightIndex = 2 * index + 2;

        if (leftIndex < size && _array[leftIndex].CompareTo(_array[largest]) > 0) largest = leftIndex;
        if (rightIndex < size && _array[rightIndex].CompareTo(_array[largest]) > 0) largest = rightIndex;

        if (largest != index)
        {
            (_array[index], _array[largest]) = (_array[largest], _array[index]);
            Heapify(largest, size);
        }
    }

    private void BuildMaxHeap()
    {
        for (int i = _count / 2 - 1; i >= 0; i--)
        {
            Heapify(i, _count);
        }
    }
    private void BubbleUp(int index)
    {
        int parentIndex = (index - 1) / 2;

        while (index > 0 && _array[parentIndex].CompareTo(_array[index]) < 0)
        {
            (_array[index], _array[parentIndex]) = (_array[parentIndex], _array[index]);
            index = parentIndex;
            parentIndex = (index - 1) / 2;
        }
    }

    public void Add(T item)
    {
        if (_count == _capacity) Resize();
        _array[_count++] = item;
        BubbleUp(_count - 1);
    }

    public T Peek()
    {
        if (IsEmpty) throw new IndexOutOfRangeException();
        return _array[0];
    }

    public T Pop()
    {
        if (IsEmpty) throw new IndexOutOfRangeException();
        T item = _array[0];
        _count--;

        (_array[0], _array[_count]) = (_array[_count], _array[0]);
        Heapify(0, _count);

        return item;
    }
}

class Program
{
    static void Main()
    {
    }
}