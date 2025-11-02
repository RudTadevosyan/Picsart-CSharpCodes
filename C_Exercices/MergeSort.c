#include <stdio.h>

void merge(int *arr, int l, int m, int r)
{
	int leftLength = m - l + 1;
	int rightLength = r - m ;

	int tempLeft[leftLength];
	int tempRight[rightLength];

	for(int i = 0; i < leftLength; i++)
	{
		tempLeft[i] = arr[l + i];
	}

	for(int i = 0; i < rightLength; i++)
	{
		tempRight[i] = arr[m + 1 + i];
	}

	int i = 0;
	int j = 0;
	int k = l;

	while(i < leftLength && j < rightLength)
	{
		if(tempLeft[i] <= tempRight[j])
		{
			arr[k] = tempLeft[i];
			i++;
		}
		else
		{
			arr[k] = tempRight[j];
			j++;
		}
		k++;
	}

	while(i < leftLength)
	{	
		arr[k] = tempLeft[i];
		i++;
		k++;
	}

	while(j < rightLength)
	{	
		arr[k] = tempRight[j];
		j++;
		k++;
	}

}
	
void merge_sort_recursion(int *arr, int l, int r)
{
	if(l < r) 
	{
		int m = l + (r - l)/2;

		merge_sort_recursion(arr, l, m);
		merge_sort_recursion(arr, m + 1, r);

		merge(arr, l, m, r);
	}	
}

void merge_sort(int *arr, int size)
{
	merge_sort_recursion(arr, 0, size - 1);
}


int main()
{

	int size = 7;
	int array[] = {38,27,43,3,9,82,10};

	merge_sort(array, size);

	for(int i = 0; i < size; i++)
	{
		printf("%d ", array[i]);
	}

	printf("\n");

	return 0;
}
