#include <stdio.h>
#include <limits.h> //for INT_MAX

#define V 5

int minRoad(int road[], int mstSet[]){ //just finding in a road array the minimum road weight also checking are they visited or not by mstSet array
	int min = INT_MAX;
	/*
		we could find the min number int this array by init min as road[0] and starting the for loop from 1
		but at that case we must be sure that the first one always works out for us which is not as we cant'
		be sure wheter the first one is visited in mstSET or what, so we use INT_MAX to check every number in 
		road array as a more general approach and minIndex giving -1 not 0;
	*/
	int minIndex = -1;

	for(int i = 0; i < V; i++){
		if(road[i] < min && mstSet[i] == 0){
			min = road[i];
			minIndex = i;
		}		
	}

	return minIndex;
}

void primMST(int graph[V][V]){
	int parent[V];
	int road[V];
	int mstSet[V];
	
	for(int i = 0; i < V; i++){
		road[i] = INT_MAX;
		mstSet[i] = 0;
	}

	//taking the vertex = 0 as a start
	parent[0] = -1; // there is no parent for starting vertex
	road[0] = 0; // there is no weight for reaching starting vertex
	
	int edge = V - 1; // prim's algorithm works with tree where -> edges = vertiices - 1 so it will be connected minumly and without cycles
	
	for(int i = 0; i < edge; i++){ // we will do this loop until we have all edges
		int minIndex = minRoad(road, mstSet);
		mstSet[minIndex] = 1;
		
		for(int j = 0; j < V; j++){ //checking in adj matrix all edges for the minIndex vertex has
			if(graph[minIndex][j] != 0 && mstSet[j] == 0 && graph[minIndex][j] < road[j]){ //if there is an edge, its not visited, and the road is less
				road[j] = graph[minIndex][j];
				parent[j] = minIndex;
			}
		}
		
	}
	
	printf("Edge \tWeight\n"); // printing the result 
    	for (int i = 1; i < V; i++) {
        	printf("%d - %d \t%d\n", parent[i], i, graph[i][parent[i]]);
    	}	
}


int main(){

	int graph[V][V] = {
        {0, 2, 0, 6, 0},
        {2, 0, 3, 8, 5},
        {0, 3, 0, 0, 7},
        {6, 8, 0, 0, 9},
        {0, 5, 7, 9, 0}
    	};

    	primMST(graph);
	
	return 0;
}
