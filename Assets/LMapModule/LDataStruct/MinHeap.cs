using System;

namespace LDataStruct
{
    public class MinHeap<T> : Heap<T> where T : IComparable
    {
        public MinHeap(int capacity) : base(capacity)
        {
        }

        public override void Adjust(T item)
        {
            //如果Item有父节点
            int index = GetItemIndex(item);
            //如果不包含这个节点
            if (index == -1)
                return;
            //如果item变化后比父节点大  old: root < item < child   now: root < item  但是child和item关系不确定
            if (item.CompareTo(itemArray[index / 2]) > 0)
            {
                //调整item与子节点的
                Sink(index);
            }
            else
            {
                //old: root < item < child  now:item < root < child  root已经没有资格再做root了，需要往上冒
                Pop(index);
            }
        }

        protected override void Pop(int index)
        {
            T targetItem = itemArray[index];
            while (index > 1 && itemArray[index / 2].CompareTo(targetItem) > 0)
            {
                var parentIndex = index / 2;
                itemArray[index] = itemArray[parentIndex];
                index = parentIndex;
            }

            itemArray[index] = targetItem;
        }

        protected override void Sink(int index)
        {
            T targetItem = itemArray[index];
            int parent = index;
            //节点i的左儿子下标为2*i，右儿子下标为2*i+1
            while (parent * 2 <= count)
            {
                var child = parent * 2;
                //让Child指向左右节点中较小的那个
                if (child != count && itemArray[child].CompareTo(itemArray[child + 1]) > 0)
                    child++;
                if (targetItem.CompareTo(itemArray[child]) < 0)
                    break;
                itemArray[parent] = itemArray[child];
                //将temp元素移动到下一层
                // CommonUtility.Swap(itemArray, parent, child);
                //child移动到parent位置了，所以接下来需要从其他地方移动数据到child位置上。这里直接循环即可
                parent = child;
            }

            itemArray[parent] = targetItem;
        }
    }
}