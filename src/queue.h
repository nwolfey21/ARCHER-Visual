#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <time.h>

#define MAXIM 10

struct node
{
	struct node *next;
	double *fSpaceTime;
};

struct queue
{
	struct node *head;
	struct node *tail;
	int size;
};

void initQueue(struct queue *que, struct node *nod1, struct node *nod2, int nx)
{
	que->size = 2;
	que->head = nod1;
	que->tail = nod2;
	nod1->next = nod2;
	nod2->next = nod1;
	nod1->fSpaceTime = (double *)calloc(nx,sizeof(double));
	nod2->fSpaceTime = (double *)calloc(nx,sizeof(double));
}

void ENQUEUE(struct queue *que, struct node *nod)
{
	if(que->size == 0)
	{
		que->head = nod;
		que->tail = nod;
		nod->next = NULL;
	}
	else
	{
		que->tail->next = nod;
		que->tail = nod;
		nod->next = NULL;
	}
	que->size++;
	if(que->size == MAXIM)
	{
		printf("Queue is Full\n");
	}
}

void DEQUEUE(struct queue *que)
{
	que->head = que->head->next;
	que->size--;
}
