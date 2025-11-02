#include <stdio.h>
#include <stdlib.h>

int prime(int n){
	if(n < 2){
		return 0;
	}

	for(int i = 2; i*i <= n; i++){
		if(n % i == 0){
			return 0;
		}
	}
	return 1;		
}
	
int * prime_row(int **matrix, int n, int *arr_size){
	int *sum = (int *)malloc(sizeof(int) * n);

	for(int i = 0; i < n; i++){
		int k = 0;
		for(int j = 0; j < n; j++){
			if(prime(matrix[i][j])){
				k++;
			}
			sum[*arr_size] += matrix[i][j];
		}

		if(k < 2){
			sum[*arr_size] = 0;
		}
		else{
			(*arr_size)++;
		}
	}
	
	if(*arr_size == n){
		return sum;
	}
	else{
		sum = (int*)realloc(sum, sizeof(int) * *arr_size);
		return sum;
	}	
	
}

void sum_prime_row(int ***matrix1, int *n){
	int ** matrix = *matrix1;
	int row_count = *n;	


	for(int i = 0; i < *n; i++){
		int sum = 0;
		for(int j = 0; j < *n; j++){
			sum += matrix[i][j];
		}
		if(!prime(sum)){
			free(matrix[i]);
			matrix[i] = NULL;
			row_count--;
		}
	}

	int ** new_matrix = (int **)malloc(sizeof(int *) * row_count);
	int k = 0;
	
	for(int i = 0; i < *n; i++){
		if(matrix[i]){
			new_matrix[k] = matrix[i];
			k++;
		}
	}
	
	*matrix1 = new_matrix;
	*n = row_count;	
	
}

int main(){

	int n = 0;
	
	
	printf("Input a size of matrix: ");
	scanf("%d", &n);

	int **matrix = (int **)malloc(sizeof(int *) * n);
	for(int i = 0; i < n; i++){
		matrix[i] = (int *)malloc(sizeof(int) * n);	
	}
	
	for(int i = 0; i < n; i++){
		for(int j = 0; j < n; j++){
			printf("Input a number for %d row and %d col: ", i + 1, j + 1);
			scanf("%d", &matrix[i][j]);
		}
		printf("\n");
	}
	
	int arr_size = 0;

	int *arr = prime_row(matrix, n, &arr_size);	
	int cols = n;
	sum_prime_row(&matrix, &n);
	
	for(int i = 0; i < n; i++){
		for(int j = 0; j < cols; j++){
			printf("%d ",matrix[i][j]);
		}
		printf("\n");
	}
	
	printf("\nSum: ");

	for(int i = 0; i < arr_size; i++){
		printf("%d ", arr[i]);
	}
	
	printf("\n");

	

	for(int i = 0; i < n; i++){
		free(matrix[i]);
	}
	free(matrix);


	return 0;
}
