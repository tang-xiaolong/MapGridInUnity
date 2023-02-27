using LDataStruct;
using UnityEngine;

public class TestHeap : MonoBehaviour
{
    public int count = 8;
    private Heap<int> heap;
    public int insertItem = 10;
    public string nunSequence = "398 400 398 400 400 398 398 400 400 400 400 398 398 400 398 400 400 400 400 400 400 400 400 400 400 398 398";
    [ContextMenu("CreateMinHeap")]
    public void CreateMinHeap()
    {
        heap = new MinHeap<int>(count);
        for (int i = 0; i < count; i++)
            heap.Insert(Random.Range(1, 100));
        Debug.Log(heap);
        Debug.Log(heap.IsMinHeap());
        
    }
    [ContextMenu("CreateMinHeapBySequence")]
    public void CreateMinHeapBySequence()
    {
        var numbers = nunSequence.Split(' ');
        count = numbers.Length;
        heap = new MinHeap<int>(count);
        int[] intNums = new int[count];
        for (int i = 0; i < count; i++)
        {
            intNums[i] = int.Parse(numbers[i]);
            // heap.Insert(int.Parse(numbers[i]));
        }
        heap.Create(intNums);
        Debug.Log(heap);
        Debug.Log(heap.IsMinHeap());
        
    }
    
    [ContextMenu("CreateMaxHeap")]
    public void CreateMaxHeap()
    {
        heap = new MaxHeap<int>(count);
        for (int i = 0; i < count; i++)
            heap.Insert(Random.Range(1, 100));
        Debug.Log(heap);
        
    }
    [ContextMenu("InsertValue")]
    public void InsertValue()
    {
        if (heap != null)
        {
            heap.Insert(insertItem);
            Debug.Log($"{heap}  Is MinHeap:{heap.IsMinHeap()}");
        }
    }
    [ContextMenu("DeleteValue")]
    public void DeleteValue()
    {
        if (heap != null && heap.Count > 0)
        {
            Debug.Log($"Delete:{heap.DeleteHead()}");
            Debug.Log($"{heap}  Is MinHeap:{heap.IsMinHeap()}");
        }
    }
    [ContextMenu("PrintHeap")]
    public void PrintHeap()
    {
        if (heap != null && heap.Count > 0)
        {
            Debug.Log(heap);
        }
    }
}