#include <stdio.h>
#include <stdlib.h>

typedef struct Node{
	int data;
	struct Node* next;
}Node;

Node *createNode(int val){
	Node *head = (Node *)malloc(sizeof(Node));
	if(!head){
		return NULL;
	}
	
	head->next = NULL;
	head->data = val;
	
	return head;

}

void printNode(Node **head){
	if(!*head){
		return;
	}

	while(*head != NULL){
		printf("%d ", (*head)->data);
		*head = (*head)->next;
	}
	
	printf("\n");
}

void insertAtEnd(Node **head, int val){
	if(!head) return;

	Node *newNode = (Node *)malloc(sizeof(Node));
	newNode->data = val;
	newNode->next = NULL;
	
	if(!*head){
		*head = newNode;
		return;
	}
	
	Node *tmp = *head;
	while(tmp->next != NULL){
		tmp = tmp->next;
	}
	
	tmp->next = newNode;
}

void insertAtFront(Node **head, int val){
	if(!head) return;

	Node *newHead = (Node *)malloc(sizeof(Node));
	newHead->data = val;
	newHead->next = *head;
	
	*head = newHead;	
}

void insertAt(Node **head, int ind, int val){
	if(!head) return;
	Node *newNode = (Node*)malloc(sizeof(Node));
	newNode->data = val;
	
	if(!*head){
		*head = newNode;
		newNode->next = NULL;
		return;
	}
	
	if(ind == 0){
		newNode->next = *head;
		free(*head);
		*head = newNode;
		return;
	}	

	Node *tmp = *head;

	int count = 0;
	while(tmp){
		tmp = tmp->next;
		count++;
	}
	
	if(ind > count){
		return;
	}

	tmp = *head;
	
	for(int i = 0; i < ind - 1; i++){
		tmp = tmp->next;
	}
	
	Node *next = tmp->next;
	tmp->next = newNode;
	newNode->next = next;		
	
}

void removeAt(Node **head, int ind){
	if(!head) return;
	if(!*head) return;

	if(ind == 0){	
		Node *newHead = (*head)->next;
		free(*head);
		*head = newHead;
		return;
	}
	
	Node *tmp = *head;
	int k = 0;
	while(tmp){
		tmp = tmp->next;
		k++;
	}
	
	if(ind > k - 1) return;
	
	tmp = *head;
	
	for(int i = 0; i < ind - 1; i++){
		tmp = tmp->next;
	}
	
	Node* prev = tmp;
	tmp = tmp->next;
	prev->next = tmp->next;	
	
}

void removeNum(Node **head, int val){
	if(!head) return;
	if(!*head) return;
	
	if((*head)->data == val){
		Node *newHead = (*head)->next;
		*head = newHead;
	}
	
	Node *tmp = *head;
	
	while(tmp->next != NULL && tmp != NULL){
		if(tmp->next->data == val){
			Node *delete = tmp->next;
			tmp->next = delete->next;
			
			free(delete);
		}
		else{
			tmp = tmp->next;
		}	
	}

}


int main(){

	Node *src = createNode(7);

	insertAtEnd(&src, 9);
	insertAtEnd(&src, 11);
	insertAtEnd(&src, 7);

	insertAtFront(&src, 5);
	insertAtFront(&src, 1);
	insertAtFront(&src, 7);

	insertAt(&src, 1, 3);
	
	removeAt(&src, 2);

	removeNum(&src, 7);

	printNode(&src);


	return 0;

}
