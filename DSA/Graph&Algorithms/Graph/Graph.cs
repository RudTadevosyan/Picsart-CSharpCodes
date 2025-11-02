namespace Graph;

sealed partial class Graph<T> where T : IComparable<T>
{
    private readonly Dictionary<T, List<(T neighbor, int weight)>> _graph;
    private readonly bool _undirected;
    private int _size;
    
    public int Size => _size;

    public Graph(bool undirected = true)
    {
        _graph = new Dictionary<T, List<(T, int)>>();
        _undirected = undirected;
        _size = 0;
    }
    
    public bool ContainsEdge(T u, T v)
        => _graph.ContainsKey(u) && _graph[u].Any(x => x.neighbor.Equals(v));
    
    public bool ContainsVertex(T u) => _graph.ContainsKey(u);
    
    public void AddEdge(T u, T v, int weight = 1)
    {
        if(!_graph.ContainsKey(u)) _graph[u] = new List<(T, int)>();
        if(!_graph.ContainsKey(v)) _graph[v] = new List<(T, int)>();
        
        _graph[u].Add((v, weight));
        if (_undirected) _graph[v].Add((u, weight));
    }
    
    public void AddVertex(T vertex)
    {
        if(!_graph.ContainsKey(vertex)) _graph[vertex] = new List<(T, int)>();
        _size++;
    }
    public void Dfs()
    {
        HashSet<T> visited = new();
        foreach (var pair in _graph)
        {
            if (!visited.Contains(pair.Key)) DfsHelper(pair.Key, visited);
        }
    }

    private void DfsHelper(T u, HashSet<T> visited)
    {
        visited.Add(u);
        Console.WriteLine(u);
        foreach (var (neighbor, _) in _graph[u])
        {
            if (!visited.Contains(neighbor)) DfsHelper(neighbor, visited);
        }
    }

    private void DfsHelperIterative(T u, HashSet<T> visited)
    {
        Stack<T> s = new();
        s.Push(u);
        visited.Add(u);

        while (s.Count > 0)
        {
            u = s.Pop();
            Console.WriteLine(u);
            
            for (int i = _graph[u].Count - 1; i >= 0; i--)
            {
                T neighbor = _graph[u][i].neighbor;
                if (!visited.Contains(neighbor))
                {
                    s.Push(neighbor);
                    visited.Add(neighbor);
                }
            }
        }
    }

    public void Bfs()
    {
        HashSet<T> visited = new();
        foreach (var pair in _graph)
        {
            if (!visited.Contains(pair.Key)) BfsHelper(pair.Key, visited);
        }
    }

    private void BfsHelper(T u, HashSet<T> visited)
    {
        Queue<T> q = new();
        q.Enqueue(u);
        visited.Add(u);

        while (q.Count > 0)
        {
            u = q.Dequeue();
            Console.WriteLine(u);

            foreach (var (neighbor, _) in _graph[u])
            {
                if (!visited.Contains(neighbor))
                {
                    q.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }
    }

    public void BfsLevel()
    {
        HashSet<T> visited = new();
        foreach (var pair in _graph)
        {
            if (!visited.Contains(pair.Key)) BfsHelperLevel(pair.Key, visited);
        }
    }

    private void BfsHelperLevel(T u, HashSet<T> visited)
    {
        Queue<T> q = new();
        q.Enqueue(u);
        visited.Add(u);

        int level = 0;

        while (q.Count > 0)
        {
            int size = q.Count;
            Console.WriteLine($"current level {level++}");

            while (size-- > 0)
            {
                u = q.Dequeue();
                Console.Write($"{u} ");

                foreach (var (neighbor, _) in _graph[u])
                {
                    if (!visited.Contains(neighbor))
                    {
                        q.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
