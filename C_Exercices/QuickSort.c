#include <stdio.h>

int partition(int *arr, int start, int end)
{
	int i = -1;
	
	for(int j = 0; j <= end - 1; j++)
	{
		if(arr[j] < arr[end])
		{
			i++;
			int tmp = arr[j];
			arr[j] = arr[i];
			arr[i] = tmp;
		}
	}

	i++;
	int tmp = arr[i];
	arr[i] = arr[end];
	arr[end] = tmp;

	return i;

}

void quickSort(int *arr, int start, int end)
{
	if(end <= start) return;

	int pivot = partition(arr, start, end);
	
	quickSort(arr, start, pivot - 1);
	quickSort(arr, pivot + 1, end);
}


int main()
{
	int arr[] = {38, 27, 43, 3, 9, 82, 4};
	
	int n = sizeof(arr)/sizeof(arr[0]);

	quickSort(arr, 0, n - 1);

	for(int i = 0; i < n; i++)
	{
		printf("%d ", arr[i]);
	}

	printf("\n");

	return 0;
}
