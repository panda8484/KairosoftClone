using System;
using System.Collections.Generic;

// made by gpt
public class PriorityQueue<T>
{
    private List<Tuple<T, float>> heap = new List<Tuple<T, float>>();

    public int Count => heap.Count;

    public void Enqueue(T item, float priority)
    {
        heap.Add(Tuple.Create(item, priority));
        int childIndex = heap.Count - 1;
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (heap[childIndex].Item2 >= heap[parentIndex].Item2) break;

            var tmp = heap[childIndex];
            heap[childIndex] = heap[parentIndex];
            heap[parentIndex] = tmp;
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (Count == 0) throw new InvalidOperationException("The priority queue is empty");

        T result = heap[0].Item1;
        heap[0] = heap[Count - 1];
        heap.RemoveAt(Count - 1);

        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = 2 * parentIndex + 1;
            int rightChildIndex = 2 * parentIndex + 2;
            int smallestIndex = parentIndex;

            if (leftChildIndex < heap.Count && heap[leftChildIndex].Item2 < heap[smallestIndex].Item2)
            {
                smallestIndex = leftChildIndex;
            }
            if (rightChildIndex < heap.Count && heap[rightChildIndex].Item2 < heap[smallestIndex].Item2)
            {
                smallestIndex = rightChildIndex;
            }
            if (smallestIndex == parentIndex) break;

            var tmp = heap[parentIndex];
            heap[parentIndex] = heap[smallestIndex];
            heap[smallestIndex] = tmp;
            parentIndex = smallestIndex;
        }
        return result;
    }

    public bool Contains(T item)
    {
        return heap.Exists(x => x.Item1.Equals(item));
    }
}
