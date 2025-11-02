namespace FinalExamPrep;
public class ListNode<T>
{
    public T Val { get; set; }
    public ListNode<T>? Next { get; set; }

    public ListNode(T val, ListNode<T>? next = null)
    {
        Val = val;
        Next = next;
    }
}

public static class MergeSortAlgorithm<T> where T : IComparable
{
    public static void MergeSort(T[] arr)
    {
        if(arr.Length < 2) return;
        MergeSort(arr, 0, arr.Length - 1);
    }

    private static void MergeSort(T[] arr, int left, int right)
    {
        if(left >= right) return;
        int middle = left + (right - left) / 2;
        
        MergeSort(arr, left, middle);
        MergeSort(arr, middle + 1, right);
        
        Merge(arr, left, middle, right);
    }

    private static void Merge(T[] arr, int left, int middle, int right)
    {
        int l1 = left, l2 = middle;
        int r1 = middle + 1, r2 = right;
        
        T[] tmp = new T[right - left + 1];

        int k = 0;
        while (l1 <= l2 && r1 <= r2)
        {
            if (arr[l1].CompareTo(arr[r1]) <= 0)
            {
                tmp[k++] = arr[l1++];
            }
            else
            {
                tmp[k++] = arr[r1++];
            }
        }
        
        while(l1 <= l2) tmp[k++] = arr[l1++];
        while(r1 <= r2) tmp[k++] = arr[r1++];

        for (int i = left; i <= right; i++)
        {
            arr[i] = tmp[i - left];
        }
    }

    public static ListNode<T>? MergeSortLinkedList(ListNode<T>? head)
    {
        if (head == null || head.Next == null) return head;
        return MergeSortHelper(head);
    }

    private static ListNode<T>? MergeSortHelper(ListNode<T>? head)
    {
        if(head == null || head.Next == null) return head;

        ListNode<T> mid = GetMid(head);
        ListNode<T>? midNext = mid.Next;
        mid.Next = null;
        
        head = MergeSortHelper(head);
        midNext = MergeSortHelper(midNext);

        return MergeIterative(head, midNext);
    }

    private static ListNode<T> GetMid(ListNode<T> head)
    {
        ListNode<T>? slow = new ListNode<T>(default(T)!, head);
        ListNode<T>? fast = new ListNode<T>(default(T)!, head);

        while (fast != null && fast.Next != null)
        {
            slow = slow.Next;
            fast = fast.Next.Next;
        }
        return slow;
    }

    private static ListNode<T>? MergeIterative(ListNode<T>? p, ListNode<T>? q)
    {
        ListNode<T> tail = new ListNode<T>(default(T)!, p);
        ListNode<T> dummy = tail;

        while (p != null && q != null)
        {
            if (p.Val.CompareTo(q.Val) <= 0)
            {
                tail.Next = p;
                p = p.Next;
            }
            else
            {
                tail.Next = q;
                q = q.Next;
            }
            tail = tail.Next;
        }
        tail.Next = (p == null) ? q : p;

        return dummy.Next;
    }

    private static ListNode<T>? MergeRecursive(ListNode<T>? p, ListNode<T>? q)
    {
        if(p == null && q == null) return p;
        if(p == null) return q;
        if(q == null) return p;

        if (p.Val.CompareTo(q.Val) <= 0)
        {
            p.Next = MergeRecursive(p.Next, q);
            return p;
        }
        else
        {
            q.Next = MergeRecursive(p, q.Next);
            return q;
        }
    }
    
}