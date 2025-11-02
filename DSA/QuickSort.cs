namespace FinalExamPrep;

public static class QuickSortAlgorithm<T> where T : IComparable<T>
{
    public static void QuickSort(T[] array)
    {
        if(array.Length < 2) return;
        QuickSort(array, 0, array.Length - 1);
    }

    private static void QuickSort(T[] array, int left, int right)
    {
        if (left >= right) return;

        int pivot = Partition(array, left, right);
        
        QuickSort(array, left, pivot - 1);
        QuickSort(array, pivot + 1, right);
    }

    private static int Partition(T[] array, int left, int right)
    {
        //int pivotIndex = MedianOfThree(array, left, right);
        int pivotIndex = Random(left, right);
        (array[pivotIndex], array[right]) = (array[right], array[pivotIndex]);

        int j = left - 1;
        for (int i = left; i < right; i++)
        {
            if (array[i].CompareTo(array[right]) <= 0)
            {
                j++;
                (array[i], array[j]) = (array[j], array[i]);
            }    
        }

        j++;
        (array[right], array[j]) = (array[j], array[right]);
        
        return j;

    }

    private static int MedianOfThree(T[] array, int left, int right)
    {
        int mid = (left + (right - left) / 2);
        T a = array[left];
        T b = array[right];
        T c = array[mid];

        if ((a.CompareTo(b) < 0) && (a.CompareTo(c) > 0) || (a.CompareTo(b) > 0 && a.CompareTo(c) < 0)) return left;
        if ((b.CompareTo(a) > 0 && b.CompareTo(c) < 0) || (b.CompareTo(a) < 0 && b.CompareTo(c) > 0)) return right;
        return mid;
    }

    private static int Random(int left, int right)
    {
        Random random = new Random();
        return random.Next(left, right + 1);
    }
}