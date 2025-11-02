namespace SLL;

sealed class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}

sealed class MyLinkedList<T> where T : IComparable<T>
{ 
    private Node<T>? Head { get; set; }
    private int _size = 0;
    public int Size => _size;
    public MyLinkedList()
    {
        Head = null;
    }
    
    public MyLinkedList(T value)
    {
        Node<T> newNode = new Node<T>(value);
        Head = newNode;
        _size++;
    }

    public void PushBack(T value)
    {
        if (Head == null)
        {
            Head = new Node<T>(value);
            _size++;
            return;
        }
        
        Node<T> newNode = new Node<T>(value);
        Node<T> current = Head;

        while (current.Next != null)
        {
            current = current.Next;
        }
        
        current.Next = newNode;
        _size++;
    }

    public void PushFront(T value)
    {
        Node<T> newNode = new Node<T>(value)
        {
            Next = Head
        };
        Head = newNode;
        _size++;
    }

    public void Print()
    {
        if (Head == null) return;
        Node<T> current = Head;

        while (current != null)
        {
            Console.Write(current.Value + " ");
            current = current.Next!;
        }
        Console.WriteLine();
    }
    public void Insert(T value, int index)
    {
        if (index < 0 || index > _size) throw new IndexOutOfRangeException();
        if (index == 0)
        {
            PushFront(value);
            _size++;
            return;
        }

        if (index == _size)
        {
            PushBack(value);
            _size++;
            return;
        }
        
        Node<T> newNode = new Node<T>(value);
        Node<T> current = Head!;

        for (int i = 0; i < index - 1; i++)
        {
            current = current.Next!;
        }
        
        newNode.Next = current.Next;
        current.Next = newNode;
        _size++;
    }

    public void Erase(int index)
    {
        if(index < 0 || index >= _size) throw new IndexOutOfRangeException();
        if(_size == 0) return;

        if (index == 0)
        {
            Head = Head?.Next;
            _size--;
            return;
        }
        
        Node<T> current = Head!;

        for (int i = 0; i < index - 1; i++)
        {
            current = current.Next!;
        }
        
        current.Next = current.Next?.Next;
        _size--;
    }


    public void Reverse()
    {
        if (_size == 0) return;
        Node<T> current = Head;
        Node<T> previous = null;

        while (current != null)
        {
            Node<T> next = current.Next!;
            current.Next = previous;
            previous = current;
            current = next;
        }
        
        Head = previous;
    }

    public void Sort()
    {
        if(_size < 2) return;
        Head = MergeSort(Head);
    }
    public Node<T> GetMidElement(Node<T> head)
    {
        Node<T> dummy = new Node<T>(default!)
        {
            Next = head,
        };
        Node<T> slow = dummy;
        Node<T> fast = dummy;

        while (fast != null && fast.Next != null)
        {
            slow = slow.Next;
            fast = fast.Next.Next;
        }

        return slow;

    }

    private Node<T> Merge(Node<T> left, Node<T> right)
    {
        Node<T> res = new Node<T>(default);
        Node<T> dummy = res;
        
        while (left != null && right != null)
        {
            if (Comparer<T>.Default.Compare(left.Value, right.Value) <= 0)
            {
                res.Next = left;
                left = left.Next;
            }
            else
            {
                res.Next = right;
                right = right.Next;
            }
            
            res = res.Next;
        }

        res.Next = (left == null) ? right : left;

        return dummy.Next;
    }
    private Node<T>? MergeSort(Node<T> head)
    {
        if (head == null ||head.Next == null) return head;
        
        Node<T> mid = GetMidElement(head);
        Node<T>? midNext = mid.Next;
        mid.Next = null;
        
        Node<T> left = MergeSort(head);
        Node<T> right = MergeSort(midNext);

        return Merge(left, right);
    }
    
}

class Program
{
    static void Main()
    {
        MyLinkedList<int> list = new MyLinkedList<int>();
        
        list.PushFront(7);
        list.PushBack(1);
        list.PushBack(2);
        list.PushBack(3);
        list.PushBack(4);
        list.PushBack(5);
        
        list.Insert(11, 6);
        list.Erase(3);
        
        list.Print();
        list.Reverse();
        list.Print();
        
        list.Sort();
        list.Print();
    }
}