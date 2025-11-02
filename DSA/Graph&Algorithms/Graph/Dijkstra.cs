namespace Graph;

sealed partial class Graph<T> where T : IComparable<T>
{
    public Dictionary<T, int> Dijkstra(T u)
    {
        Dictionary<T, int> res = new();
        
        foreach (var key in _graph.Keys)
        {
            res[key] = int.MaxValue; 
        }
        
        Dijkstra(u, res);
        return res;
    }

    private void Dijkstra(T start, Dictionary<T, int> res)
    {
        PriorityQueue<(T Vertex, int Dist), int> pq = new();
        pq.Enqueue((start, 0), 0);
        res[start] = 0;
        
        while (pq.Count > 0)
        {
            var (vertex, dist) = pq.Dequeue();
            if(dist > res[vertex]) continue; // relaxation means there in not better paths, lazy visited set.

            foreach (var (neighbor, weight) in _graph[vertex])
            {
                int newDist = dist + weight;
                if (newDist < res[neighbor])
                {
                    res[neighbor] = newDist;
                    pq.Enqueue((neighbor, newDist), newDist);
                }
            }
        }
    }
}