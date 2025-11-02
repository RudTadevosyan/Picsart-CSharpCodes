#include <stdio.h>
#include <stdlib.h>

#define MAX_VERTICES 6 //capacity of the queue items

typedef struct{
	int items[MAX_VERTICES];
	int front; // first items index
	int rear; // last items index
	int size; //how many items in the queue at the moment
}Queue;

void initQueue(Queue *q){
	if(!q) return;

	q->front = 0; 
	q->rear = -1; //there is no rear if queue is empty
	q->size = 0;
}

int isEmpty(Queue *q){
	if(!q) return -1;
	return q->size == 0;
}

void enqueue(Queue *q, int value){ //adding value in the queue
	if(!q) return;
	
	if(q->size == MAX_VERTICES) return; // queue is full no place to add
	q->rear = (q->rear + 1) % MAX_VERTICES; // we % to capacity to have circular [nul, nul, 2, 3] ->its not full, rear = 3, (3 + 1)%4 = 0 so the rear = 0 where is nul

	q->items[q->rear] = value; //adding
	q->size++; 
}

int dequeue(Queue *q){ //poping value from the queue where the front index is pointing
	if(!q) return -1;
	if(isEmpty(q)) return -1;
	
	int value = q->items[q->front]; 
	q->items[q->front] = 0;

	q->front = (q->front + 1) % MAX_VERTICES; //same purpose for circular
	q->size--; 
	
	return value;
}	


void BFS(int graph[MAX_VERTICES][MAX_VERTICES], int start){
	int visited[MAX_VERTICES] = {0}; //to mark visiteds
	
	Queue q; 
	initQueue(&q); //creating queue
	
	visited[start] = 1;
	enqueue(&q, start); // adding the first one
	
	while(!isEmpty(&q)){ // continue until queue is empty
		int vertex = dequeue(&q); //poping the value
		printf("%d ", vertex); //printing it
		
		for(int i = 0; i < MAX_VERTICES; i++){ // finding all edges for that vertex which we poped out
			if(graph[vertex][i] == 1 && !visited[i]){
				enqueue(&q, i); //add the next vertexes to where you have edge
				visited[i] = 1; //mark them 
			}
		}
	}

}

int main(){
	
	int graph[MAX_VERTICES][MAX_VERTICES] = {
    	{0, 1, 1, 0, 0, 0},  // Node 0 is connected to 1 and 2
    	{1, 0, 0, 1, 0, 0},  // Node 1 is connected to 0 and 3
    	{1, 0, 0, 0, 1, 0},  // Node 2 is connected to 0 and 4
    	{0, 1, 0, 0, 0, 0},  // Node 3 is connected to 1
    	{0, 0, 1, 0, 0, 1},  // Node 4 is connected to 2 and 5
    	{0, 0, 0, 0, 1, 0}   // Node 5 is connected to 4
	};

	BFS(graph, 0);
	
	printf("\n");
	return 0;		
}

