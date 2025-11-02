namespace CountingSort;

class Program
{

    static void RadixSort(ref int[] arr)
    {
        int n = arr.Length; //c1
        int max = arr[0]; //c2

        for (int i = 1; i < n; i++) //n
        {
            if (arr[i] > max)
            {
                max = arr[i]; //(n - 1) * c3
            }
        }

        int k = 0; //c4
        while (max > 0) //k + 1
        {
            max /= 10; //k * c5
            k++; //k * c6
        }
        
        k = (int)Math.Pow(10, k); //c7

        for (int exp = 1; exp < k; exp *= 10) //k + 1
        {
            CountingSort(ref arr, exp); //k
        }
        
        //g(k,n,m) = k(n + m); k = log10_maxNumber
    }
    static void CountingSort(ref int[] arr, int exp = 1) //exp for radix sort;
    {
        int n = arr.Length; // o(1) - c1
        int maxNum = (arr[0] / exp) % 10; // o(1) - c2 # WARNING IN RADIX I COULD TAKE 10, AND FOR COUNTING I SHOULD REMOVE (%,EXP)
        
        for (int i = 1; i < n; i++) // o(n)
        {
            if ((arr[i] / exp) % 10 > maxNum) //#
            {
                maxNum = (arr[i] / exp) % 10; //worst case o(n - 1) - c3 #
            }
        }
        
        int[] count = new int[maxNum + 1]; //o(1) - c4
        int[] subArr = new int[maxNum + 1]; //o(1)  - c5

        for (int i = 0; i < n; i++) //o(n + 1)
        {
            count[(arr[i] / exp) % 10]++; //o(n) - c6 #
        }

        subArr[0] = count[0]; //o(1) - c7
        for (int i = 1; i < count.Length; i++) //o(countLength) = o(m + 1)
        {
            subArr[i] = count[i] + subArr[i - 1]; //o(m) - c8
        }
        
        int[] newArr = new int[n]; //o(1) - c9
        for (int i = n - 1; i >= 0; i--) //o(n + 1)
        {
            newArr[--subArr[(arr[i] / exp) % 10]] = arr[i]; //o(n) - c10 #
        }
        
        arr = newArr; //o(1) - c11
        
        /*
           T(n, m) = c1 + c2 + (n - 1)c3 + c4 + c5 + n * c6 + c7 + m * c8 + c9 + n * c10 + c11 = const - c3 + n*(c3 + c6 + c10) - c3 + m * c8 = 
           = A + Bn + Cm
           g(n, m) = O(n + m);
         */
    }
    
    static void Main()
    {
        int[] arr = [170, 45, 75, 90, 802, 24, 2, 66];
        Console.WriteLine("Radix + Counting Sort");
        Console.WriteLine("Unsorted array:");
        foreach (int number in arr)
        {
            Console.Write(number + " ");
        }
        
        Console.WriteLine("\n\nSorted Array:");
        RadixSort(ref arr);
        
        foreach (int number in arr)
        {
            Console.Write(number + " ");
        }
        Console.WriteLine();
        
    }
}