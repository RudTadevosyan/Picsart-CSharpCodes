using System.Collections;

namespace AVLTree;

sealed class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }

    public Node(T value = default(T), Node<T>? left = null, Node<T>? right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }
}

sealed class MyAvl<T>:IEnumerable<T> where T:IComparable<T>
{
    private Node<T>? _root;

    public MyAvl(Node<T>? root = null)
    {
        _root = root;
    }
    
    public bool IsEmpty() => _root == null;
    
    public void InOrderTraversal(Action<T> action)
    {
        InOrderTraversal(_root, action);
    }

    private void InOrderTraversal(Node<T>? root, Action<T> action)
    {
        if(root == null) return;
        
        InOrderTraversal(root.Left, action);
        action(root.Value);
        InOrderTraversal(root.Right, action);
    }
    
    public bool Contains(T value)
    {
        if(_root == null) return false;
        return Contains(value, _root);
    }

    private static int GetHeight(Node<T>? node)
    {
        if(node == null) return 0;
        
        int left = GetHeight(node.Left);
        int right = GetHeight(node.Right);
        
        return Math.Max(left, right) + 1;
    }
    private static int BalanceFactor(Node<T>? node)
    {
        return GetHeight(node?.Left) - GetHeight(node?.Right);
    }

    private static Node<T> RotateLeft(Node<T> y)
    {
        Node<T> x = y.Right;
        Node<T> t2 = x.Left;
        
        x.Left = y;
        y.Right = t2;

        return x;
    }
    
    private static Node<T> RotateRight(Node<T> y)
    {
        Node<T> x = y.Left;
        Node<T> t2 = x.Right;

        x.Right = y;
        y.Left = t2;
        
        return x;
    }
    private static bool Contains(T value, Node<T>? node)
    {
        if (node == null) return false;
        if(node.Value.Equals(value)) return true;
        
        if(node.Value.CompareTo(value) > 0) return Contains(value, node.Left);
        else return Contains(value, node.Right);
    }

    public bool Insert(T value)
    {
        if (_root == null)
        {
            _root = new Node<T>(value);
            return true;
        }
        
        _root = Insert(value, _root, out bool inserted);
        return inserted;
    }

    private static Node<T> Insert(T value, Node<T>? node, out bool inserted)
    {
        if (node == null)
        {
            inserted = true;
            return new Node<T>(value);
        }

        if (node.Value.Equals(value))
        {
            inserted = false;
            return node;
        }

        if (node.Value.CompareTo(value) > 0) node.Left = Insert(value, node.Left, out inserted);
        else node.Right = Insert(value, node.Right, out inserted);

        if (BalanceFactor(node) > 1 && node.Left.Value.CompareTo(value) > 0) //LL
        {
            return RotateRight(node);
        }
        else if (BalanceFactor(node) > 1 && node.Left.Value.CompareTo(value) < 0) //RL
        {
            if (node.Left != null) node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        else if (BalanceFactor(node) < -1 && node.Right.Value.CompareTo(value) < 0) //RR
        {
            return RotateLeft(node);
        }
        else if (BalanceFactor(node) < -1 && node.Right.Value.CompareTo(value) > 0) //RL
        {
            if (node.Right != null) node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }
        
        return node;
    }

    private static Node<T> GetMin(Node<T>? node)
    {
        if(node == null) throw new NullReferenceException();
        Node<T> ancestor = node;
        while (node != null)
        {
            ancestor = node;
            node = node.Left;
        }

        return ancestor;
    }
    public bool Remove(T value)
    {
        if (_root == null) return false;
        _root = Remove(value, _root, out bool removed);
        return removed;
    }

    private static Node<T>? Remove(T value, Node<T>? node, out bool removed)
    {
        if (node == null)
        {
            removed = false;
            return node;
        }

        if (node.Value.CompareTo(value) > 0)
        {
            node.Left = Remove(value, node.Left, out removed);
        }
        else if (node.Value.CompareTo(value) < 0)
        {
            node.Right = Remove(value, node.Right, out removed);
        }
        else
        {
            if (node.Left == null)
            {
                removed = true;
                return node.Right;
            }
            if (node.Right == null)
            {
                removed = true;
                return node.Left;
            }

            Node<T> successor = GetMin(node.Right);
            node.Value = successor.Value;
            node.Right = Remove(successor.Value, node.Right, out removed);
        }

        if (BalanceFactor(node) > 1 && BalanceFactor(node.Left) >= 0) //LL 
        {
            return RotateRight(node);    
        }
        if (BalanceFactor(node) > 1 && BalanceFactor(node.Left) < 0)
        {
            if (node.Left != null) node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        if (BalanceFactor(node) < -1 && BalanceFactor(node.Right) <= 0) //RR
        {
            return RotateLeft(node);
        }
        if (BalanceFactor(node) < -1 && BalanceFactor(node.Right) > 0)
        {
            if (node.Right != null) node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }
        
        return node;
    }
    
    private IEnumerable<T> InOrder(Node<T>? root)
    {
        if(root == null) yield break;
        
        foreach (var node in InOrder(root.Left))
        {
            yield return node;
        }
        
        yield return root.Value;

        foreach (var node in InOrder(root.Right))
        {
            yield return node;
        }
    }
    public IEnumerator<T> GetEnumerator()
    {
        return InOrder(_root).GetEnumerator();
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
        var tree = new MyAvl<int>();

        // Test LL Rotation (Left-Left)
        int[] llValues = [30, 20, 40, 10, 25];
        TestAvlTree(tree, llValues);

        // Test LR Rotation (Left-Right)
        int[] lrValues = [30, 20, 40, 10, 25, 35];
        TestAvlTree(tree, lrValues);

        // Test RR Rotation (Right-Right)
        int[] rrValues = [30, 20, 40, 50, 35];
        TestAvlTree(tree, rrValues);

        // Test RL Rotation (Right-Left)
        int[] rlValues = [30, 20, 40, 50, 35, 45];
        TestAvlTree(tree, rlValues);

        // Additional edge cases
        TestAvlTree(tree, []); // Empty tree

        // Testing removal of non-existent value
        tree.Insert(30);
        tree.Remove(10);

        Console.ReadLine();
    }

    static void TestAvlTree(MyAvl<int> tree, int[] values)
    {
        foreach (var val in values)
        {
            bool inserted = tree.Insert(val);
            Console.WriteLine($"Insert {val}: {(inserted ? "Success" : "Failed")}");
        }

        Console.WriteLine("\nIn-order Traversal:");
        tree.InOrderTraversal(Console.WriteLine);

        // Perform removals and check results
        int[] removals = values.ToArray(); // Copy of the array to remove all elements
        foreach (var val in removals)
        {
            bool removed = tree.Remove(val);
            Console.WriteLine($"Remove {val}: {(removed ? "Success" : "Failed")}");

            Console.WriteLine("Tree after removal:");
            tree.InOrderTraversal(Console.WriteLine);
        }

        Console.WriteLine();
    }
}
