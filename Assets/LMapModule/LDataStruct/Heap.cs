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
    
        //创建堆
        //方法1、可以通过插入操作，将元素一个个插入到堆中，每个元素插入的复杂度为Log2N, N个元素插入总共复杂度为NLog2N
        //方法2、先将数据按顺序存入，使其满足完全二叉树的结构特性。然后调整各节点的位置，以满足最大堆的有序特性

        public bool IsMinHeap()
        {
            if (IsEmpty())
            {
                return true;
            }

            int c1;
            int c2;
            T tempNode;
            for (int i = 1; i * 2 <= count; i++)
            {
                c1 = i * 2;
                c2 = c1 + 1;
                tempNode = itemArray[i];
                if (itemArray[c1].CompareTo(tempNode) < 0 || (c2 <= count && itemArray[c2].CompareTo(tempNode) < 0))
                    return false;
            }
            return true;
        }
    
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

        public void Create(T[] items)
        {
            int length = items.Length;
            Init(length);
            for (int i = 0; i < length; i++)
            {
                itemArray.Add(items[i]);
            }

            count = length;

            int beginIndex = length / 2;
            //TODO：调整这个位置
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
            // return itemArray.Contains(item);
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

        public abstract void Insert(T item);
        public abstract void Adjust(T item);

        public abstract T DeleteHead();

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