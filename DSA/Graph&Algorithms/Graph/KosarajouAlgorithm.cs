namespace Graph;

public partial class Graph<T>
{
    public IList<IList<T>> KosarajouAlgorithm()
    {
        if(_undirected) throw new Exception("Must be directed Graph!");
        
        Dictionary<T, List<T>> transposeGraph = new();
        List<IList<T>> res = new();
        
        HashSet<T> visited = new();
        Stack<T> s = new();

        foreach (var pair in _graph) //Finishing time 
        {
            if(!visited.Contains(pair.Key)) GetFinishingTime(pair.Key, s, visited);
        }

        foreach (var u in _graph.Keys) // transposing graph
        {
            if(!transposeGraph.ContainsKey(u)) transposeGraph[u] = new List<T>(); // for isolated vertices
            foreach (var (neighbor, _) in _graph[u])
            {
                AddEdgeNewGraph(neighbor, u, transposeGraph, true);       
            }
        }

        visited.Clear(); //Collecting SCCs
        while (s.Count > 0)
        {
            T u = s.Pop();

            if (!visited.Contains(u))
            {
                List<T> curr = new();
                GetScc(u, visited, transposeGraph, curr);
                res.Add(curr);
            }
        }

        return res;
    }

    private void GetFinishingTime(T u, Stack<T> s, HashSet<T> visited) //Postorder 
    {
        visited.Add(u);

        foreach (var (neighbor, _) in _graph[u])
        {
            if (!visited.Contains(neighbor))
            {
                GetFinishingTime(neighbor, s, visited);
            }
        }
        
        s.Push(u);
    }

    private void GetScc(T u, HashSet<T> visited, Dictionary<T, List<T>> transposeGraph, List<T> curr)
    {
        visited.Add(u);
        curr.Add(u);
        foreach (var v in transposeGraph[u])
        {
            if (!visited.Contains(v))
            {
                GetScc(v, visited, transposeGraph, curr);
            }
        }
    }
    
    private void AddEdgeNewGraph(T u, T v, Dictionary<T, List<T>> transposeGraph, bool directed = false)
    {
        if(!transposeGraph.ContainsKey(u)) transposeGraph[u] = new List<T>();
        if(!transposeGraph.ContainsKey(v)) transposeGraph[v] = new List<T>();
        
        transposeGraph[u].Add(v);
        if(!directed) transposeGraph[v].Add(u);
    }
}
