using System;
using System.Collections.Generic;
using System.Text;

namespace LDataStruct
{
    public abstract class Heap<T>:IDisposable where T :IComparable
    {
        protected List<T> itemArray;
        private int capacity;
        protected int count;
        public int Count => count;
    
        public Heap(int capacity)
        {
            Init(capacity);
        }

        void Init(int initCapacity)
        {
            if (initCapacity <= 0)
            {
                throw new IndexOutOfRangeException();
            }
            capacity = initCapacity;
            //从下标为1开始存放数据
            itemArray = new List<T>(initCapacity + 1) {default};
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
        protected abstract void Pop(int index);
        protected abstract void Sink(int index);
        public abstract void Adjust(T item);

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
            Clear();
            itemArray = null;
        }
    }
}