namespace Graph;

sealed partial class Graph<T> where T : IComparable<T>
{
    public bool CanReach(T u, T v)
    {
        if(!ContainsVertex(u) || !ContainsVertex(v)) throw new Exception("Vertex is not in graph");
        
        HashSet<T> visited = new();
        return CanReach(u, v, visited);
    }

    private bool CanReach(T u, T v, HashSet<T> visited)
    {
        if(u.Equals(v)) return true;
        visited.Add(u);

        foreach (var (neighbor, _) in _graph[u])
        {
            if (!visited.Contains(neighbor))
            {
                if (CanReach(neighbor, v, visited)) return true;
            }
        }
        return false;
    }

    public int GetShortestPathSize(T u, T v)
    {
        if(!ContainsVertex(u) || !ContainsVertex(v)) throw new Exception("Vertex is not in graph");
        HashSet<T> visited = new();
        Queue<T> q = new();
        q.Enqueue(u);
        visited.Add(u);

        int level = 0;
        while (q.Count > 0)
        {
            int size = q.Count;
            while (size-- > 0)
            {
                u = q.Dequeue();
                if (u.Equals(v)) return level;

                foreach (var (neighbor, _) in _graph[u])
                {
                    if (!visited.Contains(neighbor))
                    {
                        q.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
            level++;
        }

        return -1;
    }

    public int GetNumberOfNodesAtAGivenLevel(T start, int level) //O(V + E)
    {
        HashSet<T> visited = new();
        return GetNumberOfNodesAtAGivenLevel(start, level, visited);
    }

    private int GetNumberOfNodesAtAGivenLevel(T start, int level, HashSet<T> visited)
    {
        visited.Add(start);
        Queue<T> q = new();
        q.Enqueue(start);

        int currLevel = 0;
        while (q.Count > 0)
        {
            int size = q.Count;
            if(currLevel == level) return size;
            
            while (size-- > 0)
            {
                T u = q.Dequeue();

                foreach (var (neighbor, _) in _graph[u])
                {
                    if (!visited.Contains(neighbor))
                    {
                        q.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
            currLevel++;
        }

        return -1;
    }

    public List<List<T>> GetAllPossiblePaths(T src, T dst) // O(V!) when there is complete graph unweighted
    {
        List<List<T>> paths = new();
        List<T> curr = new();
        HashSet<T> visited = new();
        GetAllPossiblePaths(src, dst, visited, curr, paths);
        return paths;
    }

    private void GetAllPossiblePaths(T u, T dst, HashSet<T> visited, List<T> curr, List<List<T>> path)
    {
        if (u.Equals(dst))
        {
            curr.Add(u);
            path.Add(new List<T>(curr));
            curr.RemoveAt(curr.Count - 1);
            return;
        } 
        
        visited.Add(u);
        curr.Add(u);

        foreach (var (neighbor, _) in _graph[u])
        {
            if (!visited.Contains(neighbor))
            {
                GetAllPossiblePaths(neighbor, dst, visited, curr, path);
            }
        }
        
        curr.RemoveAt(curr.Count - 1);
    }

    public bool HasCycle() //O(V + E)
    {
        HashSet<T> visited = new();
        if (_undirected)
        {
            foreach (var pair in _graph)
            {
                if (!visited.Contains(pair.Key))
                {
                    if(HasCycleUndirected(pair.Key, default, visited)) return true;
                }
            }
        }
        else
        {
            HashSet<T> onStack = new();
            foreach (var pair in _graph)
            {
                if (!visited.Contains(pair.Key))
                {
                    if(HasCycleDirected(pair.Key, onStack, visited)) return true;
                }
            }
        }
        return false;
    }

    private bool HasCycleUndirected(T u, T? parent, HashSet<T> visited) 
    {
        visited.Add(u);

        foreach (var (neighbor, _) in _graph[u])
        {
            if (!visited.Contains(neighbor))
            {
                if (HasCycleUndirected(neighbor, u, visited)) return true;
            }
            else
            {
                if(!neighbor.Equals(parent)) return true;
            }
        }

        return false;
    }

    private bool HasCycleDirected(T u, HashSet<T> onStack, HashSet<T> visited)
    {
        visited.Add(u);
        onStack.Add(u);

        foreach (var (neighbor, _) in _graph[u])
        {
            if(onStack.Contains(neighbor)) return true;
            if (!visited.Contains(neighbor))
            {
                if (HasCycleDirected(neighbor, onStack, visited)) return true;
            }
        }
        
        onStack.Remove(u);
        return false;
    }
}
