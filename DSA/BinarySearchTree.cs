using System.Collections;

namespace BinarySearchTree;

sealed class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }

    public Node(T val = default!, Node<T>? left = null, Node<T>? right = null)
    {
        Value = val;
        Left = left;
        Right = right;
    }
}

sealed class BinarySearchTree<T>:IEnumerable<T> where T : IComparable<T>
{
    private Node<T>? Root { get; set; }

    public BinarySearchTree(Node<T>? root = null)
    {
        Root = root;
    }

    public bool Contains(T value)
    {
        if(Root == null) return false;
        return ContainsHelper(Root, value);
    }

    private bool ContainsHelper(Node<T>? node, T value)
    { 
        if(node == null) return false;
        if(node.Value.Equals(value)) return true;
        
        return ContainsHelper(node.Left, value) || ContainsHelper(node.Right, value);
    }

    public bool ContainsIterative(T value)
    {
        if(Root == null) return false;
        
        Node<T>? curr = Root;

        while (curr != null)
        {
            if (curr.Value.CompareTo(value) == 0) return true;
            
            else if (curr.Value.CompareTo(value) > 0)
            {
                curr = curr.Left;
            }
            else
            {
                curr = curr.Right;
            }
        }

        return false;
    }

    public bool Insert(T value)
    {
        InsertHelper(Root, value, out bool inserted);
        
        return inserted;
    }

    private Node<T> InsertHelper(Node<T>? node, T value, out bool inserted)
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
        
        if (node.Value.CompareTo(value) > 0)
        {
            node.Left = InsertHelper(node.Left, value, out inserted);
        }
        else
        {
            node.Right = InsertHelper(node.Right, value, out inserted);
        }
        
        return node;
    }

    public bool InsertIterative(T value)
    {
        if(Root == null) return false;

        Node<T>? curr = Root;
        Node<T>? prev = null;

        while (curr != null)
        {
            if(curr.Value.CompareTo(value) == 0) return false;
            
            else if (curr.Value.CompareTo(value) > 0)
            {
                prev = curr;
                curr = curr.Left;
            }
            else
            {
                prev = curr;
                curr = curr.Right;
            }
        }
        
        Node<T> newNode = new Node<T>(value);
        if(prev != null && prev.Value.CompareTo(value) > 0) prev.Left = newNode;
        else if (prev != null) prev.Right = newNode;

        return true;
    }

    public bool Delete(T value)
    {
        if(Root == null) return false;
        Root = DeleteHelper(Root, value, out var deleted);
        
        return deleted;
    }

    private Node<T>? DeleteHelper(Node<T>? node, T value, out bool deleted)
    {
        if (node == null)
        {
            deleted = false;
            return null;
        }

        if (node.Value.CompareTo(value) > 0)
        {
            node.Left = DeleteHelper(node.Left, value, out deleted);
        }
        else if (node.Value.CompareTo(value) < 0)
        {
            node.Right = DeleteHelper(node.Right, value, out deleted);
        }
        else
        {
            if (node.Left == null)
            {
                deleted = true;
                return node.Right;
            }
            if (node.Right == null)
            {
                deleted = true;
                return node.Left;
            }

            Node<T> successor = GetMin(node.Right);
            node.Value = successor.Value;
            return DeleteHelper(node, successor.Value, out deleted);
        }
        
        return node;
    }

    private Node<T> GetMin(Node<T>? node)
    {
        if(node == null) throw new Exception();
        Node<T> min = node;
        while (node != null)
        {
            min = node;
            node = node.Left;
        }
        
        return min;
    }
    
    private Node<T> GetMax(Node<T>? node)
    {
        if(node == null) throw new Exception();
        Node<T> max = node;
        while (node != null)
        {
            max = node;
            node = node.Right;
        }
        
        return max;
    }

    private Node<T>? Successor(Node<T>? node)
    {
        if(node == null) return node;
        Node<T>? candidate = null;
        Node<T>? curr = Root;

        while (curr != null)
        {
            if (curr.Value.CompareTo(node.Value) > 0) //curr > node
            {
                candidate = curr; //suppose it is the biggest candidate, maybe there is less candidate go left 
                curr = curr.Left;
            }
            else
            {
                curr = curr.Right;
            }
        }
        
        return candidate;
    }

    private Node<T>? Predecessor(Node<T>? node)
    {
        if(node == null) return node;
        Node<T>? candidate = null;
        Node<T>? curr = Root;

        while (curr != null)
        {
            if (curr.Value.CompareTo(node.Value) < 0)
            {
                candidate = curr;
                curr = curr.Right;
            }
            else
            {
                curr = curr.Left;
            }
        }
        
        return candidate;
    } 
    public void InorderTraversal()
    {
        if(Root == null) return;
        InorderTraversalHelper(Root);
        Console.WriteLine();
    }
    private void InorderTraversalHelper(Node<T>? node)
    {
        if(node == null) return;
        
        InorderTraversalHelper(node.Left);
        Console.Write(node.Value + " ");
        InorderTraversalHelper(node.Right);
    }
    public void PreorderTraversal()
    {
        if(Root == null) return;
        InorderTraversalHelper(Root);
        Console.WriteLine();
    }

    private void PreorderTraversalHelper(Node<T>? node)
    {
        if(node == null) return;
        
        Console.Write(node.Value + " ");
        InorderTraversalHelper(node.Left);
        InorderTraversalHelper(node.Right);
    }
    
    public void PostorderTraversal()
    {
        if(Root == null) return;
        InorderTraversalHelper(Root);
        Console.WriteLine();
    }

    private void PostorderTraversalHelper(Node<T>? node)
    {
        if(node == null) return;
        
        InorderTraversalHelper(node.Left);
        InorderTraversalHelper(node.Right);
        Console.Write(node.Value + " ");
    }
    public IEnumerator<T> GetEnumerator()
    {
        return Inorder(Root).GetEnumerator();
    }

    private IEnumerable<T> Inorder(Node<T>? root)
    {
        if(root == null) yield break;

        foreach (var val in Inorder(root.Left))
        {
            yield return val;
        }
        
        yield return root.Value;

        foreach (var val in Inorder(root.Right))
        {
            yield return val;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


//----//


class Program
{
    //TESTING BST
    static void Main()
    {
        int testId = 1;

        Console.WriteLine("==== Manual Test Runner for BinarySearchTree ====");

        var bst = new BinarySearchTree<int>(new Node<int>(10));

        // Test 1: Insert values
        ExpectEqual(bst.Insert(5), true, testId++, "Insert 5");
        ExpectEqual(bst.Insert(15), true, testId++, "Insert 15");
        ExpectEqual(bst.Insert(5), false, testId++, "Insert duplicate 5");

        // Test 2: Contains
        ExpectEqual(bst.Contains(5), true, testId++, "Contains 5");
        ExpectEqual(bst.Contains(100), false, testId++, "Contains 100");

        // Test 3: Iterative Insert
        ExpectEqual(bst.InsertIterative(2), true, testId++, "InsertIterative 2");
        ExpectEqual(bst.InsertIterative(2), false, testId++, "InsertIterative duplicate 2");

        // Test 4: ContainsIterative
        ExpectEqual(bst.ContainsIterative(2), true, testId++, "ContainsIterative 2");
        ExpectEqual(bst.ContainsIterative(100), false, testId++, "ContainsIterative 100");

        // Test 5: Inorder Traversal output check (capture to string)
        Console.WriteLine("\n==== Test 5: InorderTraversal Output ====");
        var output = CaptureInOrderTraversal(bst); // Expected: 2 5 10 15
        var expected = "2 5 10 15";
        if (output != expected)
            Console.WriteLine($"[FAILED] Test {testId++}: InorderTraversal — expected '{expected}', got '{output}'");
        else
            Console.WriteLine($"[PASSED] Test {testId++}: InorderTraversal");

        // Test 6: Insert into BST with null root
        Console.WriteLine("\n==== Test 6: Insert into Empty Tree ====");
        var bst2 = new BinarySearchTree<int>(null!);
        ExpectEqual(bst2.Insert(50), true, testId++, "Insert into empty tree (should fail under current logic)");

        // Test 7: Custom class test
        Console.WriteLine("\n==== Test 7: Custom Class ====");
        var peopleBst = new BinarySearchTree<Person>(new Node<Person>(new Person("John", 30)));
        ExpectEqual(peopleBst.Insert(new Person("Anna", 25)), true, testId++, "Insert Person Anna");
        ExpectEqual(peopleBst.Insert(new Person("Zoe", 35)), true, testId++, "Insert Person Zoe");
        var outputPeople = CaptureInOrderTraversal(peopleBst);
        var expectedPeople = "Anna (25) John (30) Zoe (35)";
        if (outputPeople != expectedPeople)
            Console.WriteLine($"[FAILED] Test {testId++}: Person Inorder — expected '{expectedPeople}', got '{outputPeople}'");
        else
            Console.WriteLine($"[PASSED] Test {testId++}: Person Inorder");
        
        // Test 8: Preorder Traversal output check
        Console.WriteLine("\n==== Test 8: PreorderTraversal Output ====");
        var preOrderOutput = CapturePreOrderTraversal(bst); // Expected: 10 5 2 15
        var expectedPreOrder = "10 5 2 15";
        if (preOrderOutput != expectedPreOrder)
            Console.WriteLine($"[FAILED] Test {testId++}: PreorderTraversal — expected '{expectedPreOrder}', got '{preOrderOutput}'");
        else
            Console.WriteLine($"[PASSED] Test {testId++}: PreorderTraversal");

        // Test 9: Postorder Traversal output check
        Console.WriteLine("\n==== Test 9: PostorderTraversal Output ====");
        var postOrderOutput = CapturePostOrderTraversal(bst); // Expected: 2 5 15 10
        var expectedPostOrder = "2 5 15 10";
        if (postOrderOutput != expectedPostOrder)
            Console.WriteLine($"[FAILED] Test {testId++}: PostorderTraversal — expected '{expectedPostOrder}', got '{postOrderOutput}'");
        else
            Console.WriteLine($"[PASSED] Test {testId++}: PostorderTraversal");

        // Test 10: Delete leaf node (2)
        Console.WriteLine("\n==== Test 10: Delete leaf node ====");
        ExpectEqual(bst.Delete(2), true, testId++, "Delete leaf 2");
        ExpectEqual(bst.Contains(2), false, testId++, "Contains after delete leaf 2");

        // Test 11: Delete internal node (5)
        Console.WriteLine("\n==== Test 11: Delete internal node ====");
        ExpectEqual(bst.Delete(5), true, testId++, "Delete internal 5");
        ExpectEqual(bst.Contains(5), false, testId++, "Contains after delete internal 5");

        // Test 12: Delete root node (10)
        Console.WriteLine("\n==== Test 12: Delete root node ====");
        ExpectEqual(bst.Delete(10), true, testId++, "Delete root 10");
        ExpectEqual(bst.Contains(10), false, testId++, "Contains after delete root 10");

        // Test 13: Delete non-existent node (100)
        Console.WriteLine("\n==== Test 13: Delete non-existent node ====");
        ExpectEqual(bst.Delete(100), false, testId++, "Delete non-existent 100");

        // Test 14: Iterator test using foreach
        Console.WriteLine("\n==== Test 14: Iterator foreach test ====");
        var iteratedValues = new List<int>();
        foreach (var val in bst)
        { 
            iteratedValues.Add(val);
        }
        var expectedIterValues = new List<int> { 15 }; // After deletions, only 15 should remain
        bool iterPassed = iteratedValues.SequenceEqual(expectedIterValues);

        Console.WriteLine(iterPassed 
            ? $"[PASSED] Test {testId++}: Iterator foreach test" 
            : $"[FAILED] Test {testId++}: Iterator foreach test — expected {string.Join(", ", expectedIterValues)}, got {string.Join(", ", iteratedValues)}");


    }

    static string CapturePreOrderTraversal<T>(BinarySearchTree<T> tree) where T : IComparable<T>
    {
        var sw = new StringWriter();
        var original = Console.Out;
        Console.SetOut(sw);
        tree.PreorderTraversal();
        Console.SetOut(original);
        return sw.ToString().Trim();
    }

    static string CapturePostOrderTraversal<T>(BinarySearchTree<T> tree) where T : IComparable<T>
    {
        var sw = new StringWriter();
        var original = Console.Out;
        Console.SetOut(sw);
        tree.PostorderTraversal();
        Console.SetOut(original);
        return sw.ToString().Trim();
    }

    static void ExpectEqual<T>(T actual, T expected, int testId, string label)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
        {
            Console.WriteLine($"[FAILED] Test {testId}: {label} — expected '{expected}', got '{actual}'");
        }
        else
        {
            Console.WriteLine($"[PASSED] Test {testId}: {label}");
        }
    }

    static string CaptureInOrderTraversal<T>(BinarySearchTree<T> tree) where T : IComparable<T>
    {
        var sw = new StringWriter();
        var original = Console.Out;
        Console.SetOut(sw);
        tree.InorderTraversal();
        Console.SetOut(original);
        return sw.ToString().Trim();
    }
}

class Person : IComparable<Person>
{
    private string Name { get; set; }
    private int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public int CompareTo(Person? other)
    {
        return Age.CompareTo(other!.Age);
    }

    public override string ToString() => $"{Name} ({Age})";
}
