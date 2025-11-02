namespace FinalExamPrep;

public static class BinarySearch
{
    public static int LowerBound(int[] arr, int target)
    {
        int left = 0;
        int right = arr.Length - 1;
        int res = -1;
        
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] >= target)
            {
                right = mid - 1;
                res = mid;
            }
            else
            {
                left = mid + 1;
            }
        }

        return res;
    }

    public static int UpperBound(int[] arr, int target)
    {
        int left = 0;
        int right = arr.Length - 1;
        int res = -1;
        
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] > target)
            {
                right = mid - 1;
                res = mid;
            }
            else
            {
                left = mid + 1;
            }
        }

        return res;
    }

    public static int FloorBound(int[] arr, int target)
    {
        int left = 0;
        int right = arr.Length - 1;
        int res = -1;
        
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] <= target)
            {
                left = mid + 1;
                res = mid;
            }
            else
            {
                right = mid - 1;
            }
        }

        return res;
    }
}