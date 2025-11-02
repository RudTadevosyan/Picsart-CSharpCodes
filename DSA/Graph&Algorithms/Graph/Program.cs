using System.Diagnostics;
namespace Graph;

class Program
{
    static void Main()
    {
        //TestBasicGraphFeatures();
        //TestShortestPath();
        //TestLevelAndPaths();
        //TestUndirectedCycleDetection();
        //TestDirectedCycleDetection();
        //TestTopologicalSort();
        //TestKosarajou();
        //TestTarjan();
        TestDijkstra();
    }
    
    static void TestBasicGraphFeatures()
    {
        var graph = new Graph<string>(undirected: true);
        graph.AddVertex("A");
        graph.AddVertex("B");
        graph.AddVertex("C");
        graph.AddVertex("D");
        graph.AddVertex("E");
        graph.AddVertex("F"); // isolated

        graph.AddEdge("A", "B");
        graph.AddEdge("A", "C");
        graph.AddEdge("B", "D");
        graph.AddEdge("C", "D");
        graph.AddEdge("E", "F");

        Console.WriteLine("=== Basic Graph Feature Tests ===");
        Console.WriteLine("ContainsVertex(\"A\"): " + graph.ContainsVertex("A")); // True
        Console.WriteLine("ContainsEdge(\"A\", \"B\"): " + graph.ContainsEdge("A", "B")); // True
        Console.WriteLine("ContainsEdge(\"B\", \"A\"): " + graph.ContainsEdge("B", "A")); // True
        Console.WriteLine("ContainsEdge(\"A\", \"F\"): " + graph.ContainsEdge("A", "F")); // False
        Console.WriteLine("Size: " + graph.Size);

        Console.WriteLine("\nDFS Traversal:");
        graph.Dfs();

        Console.WriteLine("\nBFS Traversal:");
        graph.Bfs();

        Console.WriteLine("\nBFS Level-wise Traversal:");
        graph.BfsLevel();
    }

    static void TestShortestPath()
    {
        var graph = new Graph<string>(undirected: true);
        graph.AddEdge("A", "B");
        graph.AddEdge("A", "C");
        graph.AddEdge("B", "D");
        graph.AddEdge("C", "D");
        graph.AddEdge("E", "F");

        Console.WriteLine("\n=== Shortest Path Tests ===");
        Console.WriteLine($"Shortest path A → B: {graph.GetShortestPathSize("A", "B")}");
        Console.WriteLine($"Shortest path A → D: {graph.GetShortestPathSize("A", "D")}");
        Console.WriteLine($"Shortest path A → E: {graph.GetShortestPathSize("A", "E")}");
        Console.WriteLine($"Shortest path E → F: {graph.GetShortestPathSize("E", "F")}");
        Console.WriteLine($"Shortest path B → F: {graph.GetShortestPathSize("B", "F")}");
        Console.WriteLine($"Shortest path C → C: {graph.GetShortestPathSize("C", "C")}");

        try
        {
            graph.GetShortestPathSize("X", "A");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Expected exception for invalid vertex: {ex.Message}");
        }
    }

    static void TestLevelAndPaths()
    {
        var graph = new Graph<string>(undirected: true);
        graph.AddEdge("A", "B");
        graph.AddEdge("A", "C");
        graph.AddEdge("B", "D");
        graph.AddEdge("C", "D");

        Console.WriteLine("\n=== BFS Level + All Paths ===");
        Console.WriteLine("A's level 1 node count: " + graph.GetNumberOfNodesAtAGivenLevel("A", 1));

        var paths = graph.GetAllPossiblePaths("A", "C");
        foreach (var path in paths)
        {
            Console.WriteLine(string.Join(" -> ", path));
        }
    }

    static void TestUndirectedCycleDetection()
    {
        Console.WriteLine("\n=== Undirected Cycle Detection ===");

        var g1 = new Graph<string>(undirected: true);
        g1.AddEdge("A", "B");
        g1.AddEdge("B", "C");
        g1.AddEdge("C", "A");
        Console.WriteLine("Test 1 (Cycle): " + g1.HasCycle()); // True

        var g2 = new Graph<string>(undirected: true);
        g2.AddEdge("A", "B");
        g2.AddEdge("A", "C");
        g2.AddEdge("C", "D");
        Console.WriteLine("Test 2 (No cycle): " + g2.HasCycle()); // False

        var g3 = new Graph<string>(undirected: true);
        g3.AddEdge("A", "A"); // Self-loop
        Console.WriteLine("Test 3 (Self-loop): " + g3.HasCycle()); // True
    }

    static void TestDirectedCycleDetection()
    {
        Console.WriteLine("\n=== Directed Cycle Detection ===");

        var g1 = new Graph<string>(undirected: false);
        g1.AddEdge("A", "B");
        g1.AddEdge("B", "C");
        g1.AddEdge("C", "A");
        Console.WriteLine("Test 4 (Cycle): " + g1.HasCycle()); // True

        var g2 = new Graph<string>(undirected: false);
        g2.AddEdge("A", "B");
        g2.AddEdge("B", "C");
        g2.AddEdge("A", "C");
        Console.WriteLine("Test 5 (No cycle): " + g2.HasCycle()); // False

        var g3 = new Graph<string>(undirected: false);
        g3.AddEdge("X", "X"); // Self-loop
        Console.WriteLine("Test 6 (Self-loop): " + g3.HasCycle()); // True
    }
    static void TestKosarajou()
    {
        void PrintScCs(Graph<string> g, string label, int expectedSccCount)
        {
            var sccs = g.KosarajouAlgorithm();
            Console.WriteLine($"\n{label} (Expected SCCs: {expectedSccCount})");
            Console.WriteLine($"Actual SCC count: {sccs.Count}");
            int count = 1;
            foreach (var scc in sccs)
            {
                Console.WriteLine($"SCC {count++}: [{string.Join(", ", scc)}]");
            }
            Debug.Assert(sccs.Count == expectedSccCount, $"❌ {label} failed — Expected {expectedSccCount}, got {sccs.Count}");
        }
        
        Console.WriteLine("\n=== Kosarajou SCC Tests ===");

        // Test 1: A → B → C → A, D → E
        var g1 = new Graph<string>(undirected: false);
        g1.AddEdge("A", "B");
        g1.AddEdge("B", "C");
        g1.AddEdge("C", "A");
        g1.AddEdge("D", "E");
        PrintScCs(g1, "Test 1 (Cycle + Chain)", 3);

        // Test 2: A, B, C (no edges)
        var g2 = new Graph<string>(undirected: false);
        g2.AddVertex("A");
        g2.AddVertex("B");
        g2.AddVertex("C");
        PrintScCs(g2, "Test 2 (Isolated nodes)", 3);

        // Test 3: A → B → C → D
        var g3 = new Graph<string>(undirected: false);
        g3.AddEdge("A", "B");
        g3.AddEdge("B", "C");
        g3.AddEdge("C", "D");
        PrintScCs(g3, "Test 3 (Chain, no cycles)", 4);

        // Test 4: A ↔ B, B ↔ C, D
        var g4 = new Graph<string>(undirected: false);
        g4.AddEdge("A", "B");
        g4.AddEdge("B", "A");
        g4.AddEdge("B", "C");
        g4.AddEdge("C", "B");
        g4.AddVertex("D");
        PrintScCs(g4, "Test 4 (Mutual edges)", 2);

        // Test 5: Self-loop
        var g5 = new Graph<string>(undirected: false);
        g5.AddEdge("A", "A"); // Self-loop
        g5.AddEdge("B", "C");
        PrintScCs(g5, "Test 5 (Self-loop)", 3);

        // Test 6: Isolated node in transpose
        var g6 = new Graph<string>(undirected: false);
        g6.AddVertex("F");
        g6.AddEdge("G", "H");
        PrintScCs(g6, "Test 6 (Isolated + normal edge)", 3);
    }
    
    static void TestTopologicalSort()
    {
        Console.WriteLine("== Topological Sort Tests ==");

        // Valid DAG
        var g1 = new Graph<string>(false);
        g1.AddEdge("A", "B");
        g1.AddEdge("A", "C");
        g1.AddEdge("B", "D");
        g1.AddEdge("C", "D");
        g1.AddEdge("D", "E");

        Console.WriteLine("Test 1 (Valid DAG):");
        try
        {
            var res = g1.KahnAlgorithm();
            Console.WriteLine("Topological Order: " + string.Join(" → ", res));
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED: " + ex.Message);
        }

        // Graph with Cycle
        var g2 = new Graph<string>(false);
        g2.AddEdge("X", "Y");
        g2.AddEdge("Y", "Z");
        g2.AddEdge("Z", "X"); // cycle

        Console.WriteLine("\nTest 2 (Cycle Exists):");
        try
        {
            var res = g2.KahnAlgorithm();
            Console.WriteLine("Topological Order: " + string.Join(" → ", res));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Correctly Detected Cycle: " + ex.Message);
        }

        // Mixed Case: partially ordered, partially cyclic
        var g3 = new Graph<string>(false);
        g3.AddEdge("M", "N");
        g3.AddEdge("N", "O");
        g3.AddEdge("O", "P");
        g3.AddEdge("Q", "R");
        g3.AddEdge("R", "S");
        g3.AddEdge("S", "Q"); // cycle between Q-R-S

        Console.WriteLine("\nTest 3 (Partial Cycle):");
        try
        {
            var res = g3.KahnAlgorithm();
            Console.WriteLine("Topological Order: " + string.Join(" → ", res));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Correctly Detected Cycle: " + ex.Message);
        }

        // Disconnected DAG with isolated vertex
        var g4 = new Graph<string>(false);
        g4.AddVertex("U"); // isolated node
        g4.AddEdge("A", "B");
        g4.AddEdge("B", "C");

        Console.WriteLine("\nTest 4 (Disconnected DAG):");
        try
        {
            var res = g4.KahnAlgorithm();
            Console.WriteLine("Topological Order: " + string.Join(" → ", res));
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED: " + ex.Message);
        }
    }

    static void TestTarjan()
    {
        void PrintScCs(Graph<string> g, string label, int expectedSccCount)
        {
            var sccs = g.Tarjan();
            Console.WriteLine($"\n{label} (Expected SCCs: {expectedSccCount})");
            Console.WriteLine($"Actual SCC count: {sccs.Count}");
            int count = 1;
            foreach (var scc in sccs)
            {
                Console.WriteLine($"SCC {count++}: [{string.Join(", ", scc)}]");
            }

            Debug.Assert(sccs.Count == expectedSccCount,
                $"❌ {label} failed — Expected {expectedSccCount}, got {sccs.Count}");
        }

        Console.WriteLine("\n=== Tarjan SCC Tests ===");

        // Test 1: A → B → C → A, D → E
        var g1 = new Graph<string>(undirected: false);
        g1.AddEdge("A", "B");
        g1.AddEdge("B", "C");
        g1.AddEdge("C", "A");
        g1.AddEdge("D", "E");
        PrintScCs(g1, "Test 1 (Cycle + Chain)", 3);

        // Test 2: A, B, C (no edges)
        var g2 = new Graph<string>(undirected: false);
        g2.AddVertex("A");
        g2.AddVertex("B");
        g2.AddVertex("C");
        PrintScCs(g2, "Test 2 (Isolated nodes)", 3);

        // Test 3: A → B → C → D
        var g3 = new Graph<string>(undirected: false);
        g3.AddEdge("A", "B");
        g3.AddEdge("B", "C");
        g3.AddEdge("C", "D");
        PrintScCs(g3, "Test 3 (Chain, no cycles)", 4);

        // Test 4: A ↔ B, B ↔ C, D
        var g4 = new Graph<string>(undirected: false);
        g4.AddEdge("A", "B");
        g4.AddEdge("B", "A");
        g4.AddEdge("B", "C");
        g4.AddEdge("C", "B");
        g4.AddVertex("D");
        PrintScCs(g4, "Test 4 (Mutual edges)", 2);

        // Test 5: Self-loop
        var g5 = new Graph<string>(undirected: false);
        g5.AddEdge("A", "A"); // Self-loop
        g5.AddEdge("B", "C");
        PrintScCs(g5, "Test 5 (Self-loop)", 3);

        // Test 6: Isolated node in transpose
        var g6 = new Graph<string>(undirected: false);
        g6.AddVertex("F");
        g6.AddEdge("G", "H");
        PrintScCs(g6, "Test 6 (Isolated + normal edge)", 3);

        //Test 7 custom
        var gr7 = new Graph<string>(undirected: false);
        gr7.AddEdge("0", "1");
        gr7.AddEdge("1", "2");
        gr7.AddEdge("2", "0");
        gr7.AddEdge("2", "3");
        gr7.AddEdge("3", "4");
        gr7.AddEdge("4", "8");
        gr7.AddEdge("8", "4");
        gr7.AddEdge("1", "5");
        gr7.AddEdge("5", "6");
        gr7.AddEdge("6", "7");
        gr7.AddEdge("7", "5");
        
        gr7.AddEdge("7", "8");
        gr7.AddVertex("F");
        
        PrintScCs(gr7, "Test 7 Custom", 5);
    }
    static void TestDijkstra()
    {
        Console.WriteLine("\n=== Dijkstra Algorithm Tests ===");

        void PrintDistances<T>(Graph<T> g, T start) where T : IComparable<T>
        {
            var distances = g.Dijkstra(start);
            Console.WriteLine($"Distances from {start}:");
            foreach (var kv in distances)
            {
                string distStr = kv.Value == int.MaxValue ? "∞" : kv.Value.ToString();
                Console.WriteLine($"  Vertex {kv.Key} → {distStr}");
            }
        }

        // Test 1: Simple integer graph
        var g1 = new Graph<int>(undirected: false);
        g1.AddEdge(0, 1, 4);
        g1.AddEdge(0, 2, 1);
        g1.AddEdge(2, 1, 2);
        g1.AddEdge(1, 3, 1);
        g1.AddEdge(2, 3, 5);
        PrintDistances(g1, 0);

        // Test 2: Disconnected graph
        var g2 = new Graph<int>(undirected: false);
        g2.AddEdge(0, 1, 2);
        g2.AddEdge(1, 2, 3);
        g2.AddEdge(3, 4, 1); // disconnected component
        PrintDistances(g2, 0);

        // Test 3: Graph with string vertices
        var g3 = new Graph<string>(undirected: false);
        g3.AddEdge("A", "B", 5);
        g3.AddEdge("A", "C", 2);
        g3.AddEdge("B", "C", 1);
        g3.AddEdge("B", "D", 2);
        g3.AddEdge("C", "D", 7);
        PrintDistances(g3, "A");

        // Test 4: Multiple edges (take smallest)
        var g4 = new Graph<int>(undirected: false);
        g4.AddEdge(0, 1, 10);
        g4.AddEdge(0, 1, 5); // smaller weight
        g4.AddEdge(1, 2, 2);
        PrintDistances(g4, 0);
        
        // Test 5: Custom
        var g5 = new Graph<int>(undirected: false);
        g5.AddEdge(0, 1, 7);
        g5.AddEdge(0, 2, 12);
        g5.AddEdge(1, 2, 2);
        g5.AddEdge(1, 3, 9);
        g5.AddEdge(2, 4, 10);
        g5.AddEdge(3, 5, 1);
        g5.AddEdge(4, 3, 4);
        g5.AddEdge(4, 5, 5);
        PrintDistances(g5, 0);

        // Optional: Assert distances for automated verification
        var res = g4.Dijkstra(0);
        Debug.Assert(res[0] == 0, "Vertex 0 distance");
        Debug.Assert(res[1] == 5, "Vertex 1 distance");
        Debug.Assert(res[2] == 7, "Vertex 2 distance");
    }

}
