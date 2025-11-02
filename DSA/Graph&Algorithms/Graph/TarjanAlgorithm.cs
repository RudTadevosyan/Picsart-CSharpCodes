namespace Graph;

sealed partial class Graph<T> where T : IComparable<T>
{
    public List<List<T>> Tarjan()
    {
        List <List<T>> res = new();
        Stack<T> stack = new();
        HashSet<T> visited = new(_size);
        HashSet<T> onStack = new(_size);

        Dictionary<T, int> ll = new(_size);
        Dictionary<T, int> processId = new(_size);
        int id = 0;
        
        foreach (var u in _graph.Keys)
        {
            if (!visited.Contains(u))
            {
                TarjanDfs(u, ll, processId, visited, onStack, stack, res, ref id);
            }
        }

        return res;
    }
    
    private void TarjanDfs(T u, Dictionary<T, int> ll, Dictionary<T, int> processId, 
        HashSet<T> visited, HashSet<T> onStack, Stack<T> stack, List<List<T>> res, ref int id)
    {
        visited.Add(u);
        onStack.Add(u); //for searching o(1) in actual stack
        stack.Push(u);
        processId[u] = id;
        ll[u] = id++;

        foreach (var (neighbor, _) in _graph[u])
        {
            if (!visited.Contains(neighbor)) //traverse => discover vertices and add their processId and low links
            {
                TarjanDfs(neighbor, ll, processId, visited, onStack, stack, res, ref id);
            }

            if (onStack.Contains(neighbor))
            { 
                ll[u] = Math.Min(ll[u], ll[neighbor]);
            }
        }

        if (ll[u] == processId[u]) //the root of Scc
        {
            //onStack remove only here 
            List<T> currScc = new();
            while (stack.Count > 0 && !stack.Peek().Equals(u))
            {
                T v = stack.Pop();
                currScc.Add(v);
                onStack.Remove(v);
            }
            T last = stack.Pop();
            currScc.Add(last);
            onStack.Remove(last);
            
            res.Add(currScc);
        }
    }
}
