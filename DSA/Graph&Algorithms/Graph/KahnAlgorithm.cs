namespace Graph;

sealed partial class Graph<T> where T : IComparable<T>
{
    public List<T> KahnAlgorithm() // Topological sort for DAG
    {
        if (_undirected) throw new Exception("Must be directed graph");

        List<T> res = new List<T>();
        Dictionary<T, int> inDegree = new Dictionary<T, int>();
        Queue<T> q = new Queue<T>();
        
        foreach (var u in _graph.Keys)
        {
            if (!inDegree.ContainsKey(u)) inDegree[u] = 0;
            foreach (var (neighbor, _) in _graph[u])
            {
                if (!inDegree.ContainsKey(neighbor)) inDegree[neighbor] = 0;
                inDegree[neighbor]++;
            }
        }

        foreach (var pair in inDegree)
        {
            if(pair.Value == 0) q.Enqueue(pair.Key);
        }

        while (q.Count > 0)
        {
            var u = q.Dequeue();
            res.Add(u);
            
            foreach (var (neighbor, _) in _graph[u])
            {
                if(--inDegree[neighbor] == 0) q.Enqueue(neighbor);
            }
        }
        
        if(res.Count != _graph.Keys.Count) 
            throw new Exception("Graph has cycle — impossible to get topological sort");

        return res;
    }
}