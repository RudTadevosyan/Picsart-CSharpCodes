using System.Collections;

namespace MySortedSet;

sealed class MySortedSet<T> : IEnumerable<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node? Left;
        public Node? Right;

        public Node(T value)
        {
            Value = value;
        }
    }

    private Node? _rootNode;
    public int Count { get; private set; }

    
    
    public MySortedSet()
    {
        _rootNode = null;
        Count = 0;
    }

    public MySortedSet(T value)
    {
        _rootNode = new Node(value);
        Count++;
    }

    public bool Add(T value)
    {
        _rootNode = Add(_rootNode, value, out bool added);
        if(added) Count++;
        return added;
    }

    private Node Add(Node? node, T value, out bool added)
    {
        if (node == null)
        {
            added = true;
            return new Node(value);
        }
        
        int compare = value.CompareTo(node.Value);

        if (compare < 0)
            node.Left = Add(node.Left, value, out added);
        
        else if (compare > 0)
            node.Right = Add(node.Right, value, out added);
        
        else
            added = false;
        
        return node;
    }

    public bool Contains(T value)
    {
        return Contains(_rootNode, value);
    }

    private bool Contains(Node? node, T value)
    {
        if (node == null)
        {
            return false;
        }
        
        int compare = value.CompareTo(node.Value);

        if (compare < 0)
            return Contains(node.Left, value);
        if (compare > 0)
            return Contains(node.Right, value);

        return true;
    }

    public bool Remove(T value)
    {
        Remove(_rootNode, value, out bool removed);
        if(removed) Count--;
        return removed;
    }

    private Node? Remove(Node? node, T value, out bool removed)
    {
        if (node == null)
        {
            removed = false;
            return null;
        }
        
        int compare = value.CompareTo(node.Value);
        
        if(compare < 0)
            node.Left = Remove(node.Left, value, out removed);
        else if (compare > 0)
            node.Right = Remove(node.Right, value, out removed);
        else
        {
            removed = true;
            if (node.Left == null && node.Right == null)
            {
                return null;
            }
            
            else if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }
            else
            {
                Node successor = Min(node.Right);
                node.Value = successor.Value;
                node.Right = Remove(node.Right, node.Value, out _);
            }
        }
        return node;
    }

    private Node Min(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        
        return node;
    }
    public IEnumerator<T> GetEnumerator()
    {
        return InOrder(_rootNode).GetEnumerator();
    }

    private IEnumerable<T> InOrder(Node? node)
    {
        if (node == null)
        {
            yield break;
        }

        foreach (var value in InOrder(node.Left))
        {
            yield return value;
        }
        
        yield return node.Value;

        foreach (var value in InOrder(node.Right))
        {
            yield return value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Print()
    {
        Console.WriteLine($"Count: {Count}");
        foreach (var val in this)
        {
            Console.Write(val + " ");
        }

        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        MySortedSet<int> mySortedSet = new MySortedSet<int>();

        mySortedSet.Add(5);
        mySortedSet.Add(10);
        mySortedSet.Add(15);
        mySortedSet.Add(12);
        mySortedSet.Add(18);
        mySortedSet.Add(2);
        mySortedSet.Add(17);
        

        Console.WriteLine(mySortedSet.Contains(2));
        Console.WriteLine(mySortedSet.Contains(25));
        
        mySortedSet.Remove(15);
        mySortedSet.Print();
    }
}