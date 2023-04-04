using System;
using System.Collections.Generic;
using System.Text;

namespace LDataStruct
{
    public enum HeapType
    {
        MinHeap,
        MaxHeap
    }

    public class Heap<T> : IDisposable where T : IComparable
    {
        private bool _disposed = false;
        protected List<T> itemArray;
        private int capacity;
        protected int count;
        public int Count => count;
        private readonly Func<T, T, bool> _comparerFun;

        public Heap(int capacity, HeapType heapType)
        {
            if (heapType == HeapType.MinHeap)
                _comparerFun = MinComparerFunc;
            else
                _comparerFun = MaxComparerFunc;

            Init(capacity);
        }

        private bool MinComparerFunc(T t1, T t2)
        {
            return t1.CompareTo(t2) > 0;
        }

        private bool MaxComparerFunc(T t1, T t2)
        {
            return !MinComparerFunc(t1, t2);
        }

        void Init(int initCapacity)
        {
            if (initCapacity <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            capacity = initCapacity;
            //从下标为1开始存放数据
            itemArray = new List<T>(initCapacity + 1) { default };
            count = 0;
        }

        public bool HasItem(T item)
        {
            if (IsEmpty())
                return false;
            for (int i = 1; i <= count; i++)
            {
                if (itemArray[i].CompareTo(item) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetItemIndex(T item)
        {
            int index = -1;
            for (int i = 1; i <= count; i++)
            {
                if (itemArray[i].CompareTo(item) == 0)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// 堆是否已经满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return count == capacity;
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public void Insert(T item)
        {
            //i指向插入堆后的最后一个元素位置
            itemArray.Add(item);
            count += 1;
            Pop(count);
        }

        private void Pop(int index)
        {
            T targetItem = itemArray[index];
            while (index > 1 && _comparerFun(itemArray[index / 2], targetItem))
            {
                var parentIndex = index / 2;
                itemArray[index] = itemArray[parentIndex];
                index = parentIndex;
            }

            itemArray[index] = targetItem;
        }

        protected void Sink(int index)
        {
            T targetItem = itemArray[index];
            int parent = index;
            //节点i的左儿子下标为2*i，右儿子下标为2*i+1
            while (parent * 2 <= count)
            {
                var child = parent * 2;
                //Min:让Child指向左右节点中较小的那个
                //Max:让Child指向左右节点中较大的那个
                if (child != count && _comparerFun(itemArray[child], itemArray[child + 1]))
                    child++;
                if (_comparerFun(itemArray[child], targetItem))
                    break;
                itemArray[parent] = itemArray[child];
                //将temp元素移动到下一层
                //child移动到parent位置了，所以接下来需要从其他地方移动数据到child位置上。这里直接循环即可
                parent = child;
            }

            itemArray[parent] = targetItem;
        }

        public void Adjust(T item)
        {
            //如果Item有父节点
            int index = GetItemIndex(item);
            //如果不包含这个节点
            if (index == -1)
                return;
            //Min: 如果item变化后比父节点大  old: root < item < child   now: root < item  但是child和item关系不确定
            //Max: //如果item变化后比父节点小  old: root > item > child   now: root > item  但是child和item关系不确定
            if (_comparerFun(item, itemArray[index / 2]))
            {
                //调整item与子节点的
                Sink(index);
            }
            else
            {
                //oldMin: root < item < child  now:item < root < child  root已经没有资格再做root了，需要往上冒
                //oldMax: root > item > child  now:item > root > child  root已经没有资格再做root了，需要往上冒
                Pop(index);
            }
        }

        public T DeleteHead()
        {
            if (IsEmpty())
                throw new IndexOutOfRangeException();
            T deleteItem = itemArray[1];
            if (count > 1)
                itemArray[1] = itemArray[count];
            itemArray.RemoveAt(count);
            count -= 1;
            if (count > 1)
                Sink(1);
            return deleteItem;
        }

        public T GetHead()
        {
            if (IsEmpty())
            {
                throw new IndexOutOfRangeException("Heap is empty!");
            }

            return itemArray[1];
        }


        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i < count + 1; i++)
            {
                result.Append(itemArray[i] + " ");
            }

            return result.ToString();
        }

        public List<T>.Enumerator GetHeapEnumerator()
        {
            return itemArray.GetEnumerator();
        }

        public void Clear()
        {
            itemArray.Clear();
            itemArray.Add(default);
            count = 0;
        }

        public void Dispose()
        {
            // 调用Dispose(true)释放托管和非托管资源
            Dispose(true);
            // 阻止终结器运行
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // 如果已经释放，直接返回
            if (_disposed)
                return;
            // 如果disposing为true，表示由Dispose方法调用，释放托管资源
            if (disposing)
            {
                // 释放托管资源
                Clear();
                // 将托管资源设为null
                itemArray = null;
            }

            // 释放非托管资源

            // 将disposed设为true，表示已经释放
            _disposed = true;
        }
        
        // 定义终结器，以防止忘记调用Dispose方法
        ~Heap()
        {
            // 调用Dispose(false)只释放非托管资源
            Dispose(false);
        }
    }
}