#include <stdio.h>

void heapify(int *arr, int i, int n)
{
	int largest = i;
	int leftChild = 2 * i + 1;
	int rightChild = 2 * i + 2;
	
	if(leftChild < n && arr[leftChild] > arr[largest]) largest = leftChild;
	if(rightChild < n && arr[rightChild] > arr[largest]) largest = rightChild;
	
	if(largest != i)
	{
		int tmp = arr[i];
		arr[i] = arr[largest];
		arr[largest] = tmp;
	
		heapify(arr, largest, n);
	}

}

void HeapSort(int *arr, int n)
{
	for(int i = n/2 - 1; i >=0; i--) //max Heap
	{
		heapify(arr, i, n);
	}
	
	for(int i = n - 1; i > 0; i--) 
	{
		int tmp = arr[0];
		arr[0] = arr[i];
		arr[i] = tmp;
	
		heapify(arr, 0, i);
	}
}


int main()
{

	int arr[] = {10, 30, 20, 5, 15, 25};
	
	int n = sizeof(arr)/sizeof(arr[0]);
	
	HeapSort(arr, n);

	for(int i = 0; i < n; i++)
	{
		printf("%d ",arr[i]);
	}	

	printf("\n");

	return 0;
}
