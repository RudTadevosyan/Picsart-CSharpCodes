#include <stdio.h>
#include <stdlib.h>

#define MAX_VERTICES 10	// you can change this, it means the graph can have maximum 10 vertices

typedef struct{ //graph 
	int vertices;
	int adj[MAX_VERTICES][MAX_VERTICES];
}Graph;

void initGraph(Graph *graph, int vertices) //grap's initalisation
{
	if(!graph) return;

	graph->vertices = vertices;
	
	for(int i = 0 ; i < vertices; i++){
		for(int j = 0; j < vertices; j++){
			graph->adj[i][j] = 0;
		}
	}
}

void addEdge(Graph *graph, int u, int v){ //adding edges in adj matrix in the graph (edge mean there is a path between vertices)
	if(!graph) return;
	
	if(u != v){
		graph->adj[u][v] = 1;
		graph->adj[v][u] = 1;
	}
}

void DFSUtil(Graph *graph, int v, int visited[]){ //DFS helper function, works recursively has a stack logic, 
	visited[v] = 1;
	printf("%d ", v);
	
	for(int i = 0; i < graph->vertices; i++){
		if(graph->adj[v][i] == 1 && !visited[i]){
			DFSUtil(graph, i, visited);
		}
	}

}
void DFS(Graph *graph, int startVertex){ //calling DFS to traverse the graph, with starting vertex
	if(!graph) return;

	int visited[MAX_VERTICES] = {0};
	DFSUtil(graph, startVertex, visited);
}

int main(){

	Graph g;
	int vertices = 6; // graph will have 6 x 6 adj matrix in it
	
	initGraph(&g, vertices); //initalisation of the graph
	
	addEdge(&g, 0, 1); 
    	addEdge(&g, 0, 2);
    	addEdge(&g, 1, 3);
    	addEdge(&g, 1, 4);
	addEdge(&g, 2, 5);

	DFS(&g, 0);
	printf("\n");
		
	return 0;
}
