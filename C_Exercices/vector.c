#include <stdio.h>
#include <stdlib.h>

typedef struct{
	int size;
	int capacity;
	int *data;
}Vector;

void vector_init(Vector *this){
	if(!this){
		return;
	}
	
	this->data = NULL;
	this->size = 0;
	this->capacity = 1;
}

void vector_init_fill(Vector *this, int size, int value){
	if(!this){
		return;
	}
	this->size = size;
	this->capacity = size;	

	this->data = (int *)malloc(sizeof(int) * this->capacity);
	if(!this->data){
		return;
	}
	
	for(int i = 0; i < size; i++){
		this->data[i] = value;
	}
	
}

void printVector(Vector *this){
	if(!this){
		return;
	}
	
	for(int i = 0; i < this->size; i++){
		printf("%d ", this->data[i]);
	}
	printf("\n");
	printf("Capacity: %d\nSize: %d\n", this->capacity, this->size);
	
	printf("\n");

}

void vector_destroy(Vector *this){
	if(!this){
		return;
	}
	
	free(this->data);
	this->data = NULL;
	this->size = 0;
	this->capacity = 0;	

}

void vector_copy(Vector *this, Vector const *other){
	if(!other && !this){
		return;
	}
	
	this->data = (int *)malloc(sizeof(int) * other->capacity);
	if(!this->data){
		return;
	}
	
	this->capacity = other->capacity;
	this->size = other->size;

	for(int i = 0; i < this->size; i++){
		this->data[i] = other->data[i];
	}
	

}

void vector_assign(Vector *this, Vector const *other){
	if(!other && !this){
		return;
	}
	
	free(this->data);

	this->data = (int *)malloc(sizeof(int) * other->capacity);
	if(!this->data){
		return;
	}
	
	this->capacity = other->capacity;
	this->size = other->size;

	for(int i = 0; i < this->size; i++){
		this->data[i] = other->data[i];
	}
	
}

void push_back(Vector *this, int val){
	if(!this){
		return;
	}
	if(this->size == this->capacity){
		this->capacity *= 2;
		this->data = (int *)realloc(this->data, sizeof(int) * this->capacity);
		if(!this->data){
			return;
		}
	
	}
	
	if(!this->data){
		this->data = (int *)malloc(sizeof(int) * this->capacity);	
	}
	
	this->data[this->size] = val;	
	this->size++;
	
}


void pop_back(Vector *this, int val){
	if(!this){
		return;
	}
	if(this->size == this->capacity){
		this->capacity *= 2;
		this->data = (int *)realloc(this->data, sizeof(int) * this->capacity);
		if(!this->data){
			return;
		}
	}
	
	

	this->data[this->size] = val;
	this->size++;
	
	for(int i = 1; i < this->size; i++){
		int tmp = this->data[0];
		this->data[0] = this->data[i];
		this->data[i] = tmp;
	}

	
}

void vector_insert(Vector *v, int ind, int val){
	if(!v || ind > v->capacity){
		return;
	}
	
	if(v->size == v->capacity){
		v->capacity *= 2;
		v->data = (int *)malloc(sizeof(int) * v->capacity);
	}

	v->data[v->size] = val;	
	v->size++;
	for(int i = ind + 1; i < v->size; i++){
		int tmp = v->data[ind];
		v->data[ind] = v->data[i];
		v->data[i] = tmp;
	}
		
	
}
void vector_remove(Vector *v, int ind){
	if(!v || ind >= v->size){
		return;
	}
	
	for(int i = ind; i < v->size - 1; i++){
		int tmp = v->data[i];
		v->data[i] = v->data[i + 1];
		v->data[i + 1] = tmp;
	}
	
	v->data[v->size] = 0;
	v->size--;	

}


void swapVector(Vector *vec, Vector *src){
	if(!vec){
		return;
	}
	if(!src){
		return;
	}	

	
	int *tmp = vec->data;
	vec->data = src->data;
	src->data = tmp;
	
	int temp = vec->size;
	vec->size = src->size;
	src->size = temp;
	
	temp = vec->capacity;
	vec->capacity = src->capacity;
	src->capacity = temp;	
	
}

int main(){

	Vector vec;
	Vector src;

	vector_init_fill(&vec, 3, 7);
	vector_init(&src);

	push_back(&vec, 7);

	vector_insert(&vec, 2, 0);
	vector_assign(&src, &vec);
	vector_remove(&vec, 2);

	swapVector(&vec, &src);


	printVector(&vec);
	printVector(&src);

	return 0;


}
