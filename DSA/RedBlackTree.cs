namespace RedBlackTree;

enum Color {Red, Black}
sealed class RbNode<T>
{
    public T Value { get; set; }
    public RbNode<T>? Left { get; set; }
    public RbNode<T>? Right { get; set; }
    public RbNode<T>? Parent { get; set; }
    public Color Color { get; set; }

    public RbNode(T value, RbNode<T>? left, RbNode<T>? right, RbNode<T>? parent, Color color)
    {
        Value = value;
        Left = left;
        Right = right;
        Parent = parent;
        Color = color;
    }
}

sealed class RbTree<T> where T : IComparable<T>
{
    private RbNode<T>? _root;
    private readonly RbNode<T> _nil = new RbNode<T>(default, null, null, null, Color.Black);

    public RbTree()
    {
        _root = null;
    }
    
    public RbTree(T value)
    {
        _root = new RbNode<T>(value, _nil, _nil, _nil, Color.Black);
    }

    private void LeftRotate(RbNode<T> x) // passing z's parent
    {
        RbNode<T> y = x.Right;
        x.Right = y.Left;

        if(y.Left != _nil) y.Left.Parent = x;
        y.Parent = x.Parent;
        
        if (x.Parent == _nil) _root = y; //if x was root
        else if(x.Parent.Left == x) x.Parent.Left = y;
        else if(x.Parent.Right == x) x.Parent.Right = y;

        y.Left = x;
        x.Parent = y;
    }

    private void RightRotate(RbNode<T> x) //passing z's parent
    {
        RbNode<T> y = x.Left;
        x.Left = y.Right;
        
        if (y.Right != _nil) y.Right.Parent = x;
        y.Parent = x.Parent;
        
        if(x.Parent == _nil) _root = y;
        else if(x.Parent.Left == x) x.Parent.Left = y;
        else if(x.Parent.Right == x) x.Parent.Right = y;
        
        y.Right = x;
        x.Parent = y;
    }

    public bool Insert(T value)
    {
        if (_root == null)
        {
            _root = new RbNode<T>(value, _nil, _nil, _nil, Color.Black);
            return true;
        }
        
        RbNode<T>? node = _root;
        RbNode<T>? anc = null;
        while (node != _nil)
        {
            anc = node;
            if(node.Value.CompareTo(value) > 0) node = node.Left;
            else if(node.Value.CompareTo(value) < 0) node = node.Right;
            else return false; 
        }
        
        RbNode<T> newNode = new RbNode<T>(value, _nil, _nil, anc, Color.Red);
        if(anc.Value.CompareTo(value) > 0) anc.Left = newNode;
        else if(anc.Value.CompareTo(value) < 0) anc.Right = newNode;

        InsertFixUp(newNode);
        
        return true;
    }

    private void InsertFixUp(RbNode<T> z)
    {
        if (z.Parent.Color == Color.Black) return;

        while (z.Parent.Color == Color.Red)
        {
            if (z.Parent.Parent.Left == z.Parent) //uncle is on right or left
            {
                RbNode<T> uncle = z.Parent.Parent.Right;

                if (uncle.Color == Color.Red) //case 1
                {
                    z.Parent.Parent.Color = Color.Red;
                    
                    z.Parent.Color = Color.Black;
                    uncle.Color = Color.Black;
                    
                    z = z.Parent.Parent; //to the red
                }
                else //case 2,3 (need left for case 3)
                {
                    if (z.Parent.Left != z) //case 2
                    {
                        z = z.Parent;
                        LeftRotate(z);
                    }
                    //case 3 (always)
                    z.Parent.Color = Color.Black;
                    z.Parent.Parent.Color = Color.Red;
                        
                    RightRotate(z.Parent.Parent); // z.p.p
                }
            }
            else
            {
                RbNode<T> uncle = z.Parent.Parent.Left;

                if (uncle.Color == Color.Red) //case 1
                {
                    z.Parent.Parent.Color = Color.Red;
                    
                    z.Parent.Color = Color.Black;
                    uncle.Color = Color.Black;
                    
                    z = z.Parent.Parent; //to the red
                }
                else //case 2,3 (need right for case 3)
                {
                    if (z.Parent.Right != z) //case 2
                    {
                        z = z.Parent;
                        RightRotate(z);
                    }
                    //case 3 ( always)
                    z.Parent.Color = Color.Black;
                    z.Parent.Parent.Color = Color.Red;
                        
                    LeftRotate(z.Parent.Parent); //z.p.p
                }
            }
        }
        _root.Color = Color.Black;
    }

    private void Transplant(RbNode<T> u, RbNode<T> v) // v transplant the u
    {
        if (u.Parent == _nil) _root = v;
        
        if(u.Parent.Left == u) u.Parent.Left = v;
        else if(u.Parent.Right == u) u.Parent.Right = v;
        
        v.Parent = u.Parent;
    }

    private RbNode<T> GetMin(RbNode<T> node)
    {
        RbNode<T> min = node;
        while (node.Left != _nil)
        {
            min = node;
            node = node.Left;
        }
        
        return min;
    }
    
    public void InOrderTraversal()
    {
        InOrderTraversal(_root);
        Console.WriteLine();
    }
    private void InOrderTraversal(RbNode<T>? node)
    {
        if (node == null || node == _nil) return;
        InOrderTraversal(node.Left);
        Console.Write($"{node.Value}({node.Color}) ");
        InOrderTraversal(node.Right);
    }
}

class Program
{
    static void Main()
    {
        var rbTree = new RbTree<int>();

        int[] valuesToInsert = { 10, 20, 30, 15, 25, 5, 1, 8, 6 };

        Console.WriteLine("Inserting values:");
        foreach (var val in valuesToInsert)
        {
            Console.WriteLine($"Insert: {val}");
            rbTree.Insert(val);
        }

        Console.WriteLine("\nIn-order traversal with node colors:");
        rbTree.InOrderTraversal();
    }
}
