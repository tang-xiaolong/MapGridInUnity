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
        heap = new Heap<int>(count, HeapType.MinHeap);
        for (int i = 0; i < count; i++)
            heap.Insert(Random.Range(1, 100));
        Debug.Log(heap);
        
    }
    
    [ContextMenu("CreateMaxHeap")]
    public void CreateMaxHeap()
    {
        heap = new Heap<int>(count, HeapType.MaxHeap);
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
        }
    }
    [ContextMenu("DeleteValue")]
    public void DeleteValue()
    {
        if (heap != null && heap.Count > 0)
        {
            Debug.Log($"Delete:{heap.DeleteHead()}");
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