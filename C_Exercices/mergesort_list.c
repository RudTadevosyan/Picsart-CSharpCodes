#include <stdio.h>
#include <stdlib.h>

typedef struct ListNode
{
	int value;
	struct ListNode *next;

}ListNode;

void CreateListNode(ListNode **head, int value)
{
	ListNode *Node = (ListNode *)malloc(sizeof(ListNode));
	if(!Node) 
	{
		printf("Failed to create Node\n");
		return;
	}

	*head = Node;
	
	Node->value = value;
	Node->next = NULL;

}

void AddNode(ListNode **head, int value)
{
	if(!head) return;
	if(!*head)
	{
		CreateListNode(head, value);
		return;
	}

	ListNode *Node = (ListNode *)malloc(sizeof(ListNode));
	if(!Node) return;

	Node->value = value;
	Node->next = NULL;

	ListNode *tmp = *head;

	while(tmp->next)
	{
		tmp = tmp->next;
	}
	
	tmp->next = Node;
	
}


void PrintList(ListNode *head)
{
	if(!head) return;
	
	ListNode *tmp = head;
	while(tmp)
	{
		printf("%d ", tmp->value);
		tmp = tmp->next;
	}

	printf("\n");
}
void splitList(ListNode *head, ListNode **left, ListNode **right)
{
    
	ListNode *fast = head;
	ListNode *slow = head;

	while(fast && fast->next)
	{
		fast = fast->next;
		if(fast->next)
		{
		    slow = slow->next;
		    fast = fast->next;
		}
	}

	*left = head;
	*right = slow->next;
	slow->next = NULL;
}

ListNode *merge(ListNode *left, ListNode *right)
{
	ListNode dummy;
	dummy.next = NULL;
	ListNode *tail = &dummy;

	while(left && right)
	{
		if(left->value <= right->value)
		{
			tail->next = left;
			left = left->next;
		}
		else
		{
			tail->next = right;
			right = right->next;	
		}

		tail = tail->next;
	}

	tail->next = (left != NULL)? left: right;
	
	return dummy.next; 
}

void merge_sort(ListNode **head)
{
	if(!head || !*head || !(*head)->next) return;

	ListNode *left;
	ListNode *right;

	splitList(*head, &left, &right);

	merge_sort(&left);
	merge_sort(&right);

	*head = merge(left, right);

}


int main()
{

	ListNode *list = NULL;


	CreateListNode(&list, 38);

	AddNode(&list, 27);
	AddNode(&list, 43);
	AddNode(&list, 3);
	AddNode(&list, 9);
	AddNode(&list, 82);
	AddNode(&list, 10);

	merge_sort(&list);

	PrintList(list);

	return 0;
}

