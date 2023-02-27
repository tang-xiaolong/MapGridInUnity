using System.Collections.Generic;
using LDataStruct;
using LMap;
using UnityEngine;

namespace LPathSearch
{
    public class AStarMinHeap : IPathSearch
    {
        private int _width;

        public List<IPathNode> SearchPath(IMapGrid mapGrid, Vector2Int startPoint, Vector2Int endPoint)
        {
            _width = mapGrid.GetWidth();
            Vector3 halfGridSize = new Vector3(mapGrid.GetMapNodeSize() * .5f, mapGrid.GetMapNodeSize() * .5f);
            MinHeap<IPathNode> openMinHeap = new MinHeap<IPathNode>((_width + 1) * 2);
            Dictionary<int, IPathNode> closeList = new Dictionary<int, IPathNode>((_width + 1) * 2);
            Dictionary<IPathNode, byte> openFlag = new Dictionary<IPathNode, byte>((_width + 1) * 2);
            List<IPathNode> path = new List<IPathNode>();
            IPathNode preNode = default;
            bool havePath = false;
            //先将起点放入openList中，然后重复后续步骤
            //从openList中取出优先级最高的点curNode
            //判断curNode是否是终点? 如果是终点，结束寻路回溯路径返回
            //如果不是终点，将这个节点放入closeList中，并判断这个节点的周围四通节点是否在closeList中。
            //如果不在，计算fgh三个值,如果f值更优，更新f值同时更新它的父节点为自己，并把它放入openList中

            IPathNode startNode = mapGrid.GetValue<IPathNode>(startPoint.x, startPoint.y);
            IPathNode endNode = mapGrid.GetValue<IPathNode>(endPoint.x, endPoint.y);
            startNode.ParentNode = null;
            startNode.G = 0;
            startNode.H = mapGrid.GetDistance(startNode, endNode);
            startNode.F = startNode.G + startNode.H;
            openMinHeap.Insert(startNode);
            openFlag.Add(startNode, 1);
            Vector2Int checkNode = Vector2Int.zero;
            while (openMinHeap.Count > 0)
            {
                IPathNode curNode = openMinHeap.DeleteHead();
                openFlag[curNode] = 0;
                closeList.Add(GetKey(curNode.Index1, curNode.Index2), curNode);

                preNode = curNode;
                if ((curNode.Index1 == endPoint.x && curNode.Index2 == endPoint.y))
                {
                    havePath = true;
                    break;
                }
                else
                {
                    //检查四通的点是否在close列表中    
                    var indexOffset = mapGrid.GetNeighborsIndexOffset();
                    foreach (Vector2Int vector2Int in indexOffset)
                    {
                        checkNode.x = (int)(curNode.Index1 + vector2Int.x);
                        checkNode.y = (int)(curNode.Index2 + vector2Int.y);
                        // Vector2Int checkNode = new Vector2Int((int)(curNode.Index1 + Grid.Grid<TNode>.FourDirectionVec[i].x), (int)(curNode.Index2 + Grid.Grid<TNode>.FourDirectionVec[i].y));
                        //有效区域 可通行 且不在close表
                        if (mapGrid.HasInMap(checkNode.x, checkNode.y))
                        {
                            var tempNode = mapGrid.GetValue<IPathNode>(checkNode.x, checkNode.y);
                            if (tempNode.CanPass() && !closeList.ContainsKey(GetKey(checkNode.x, checkNode.y)))
                            {
                                //如果不在Open表，需要创建新节点并加入到Open表
                                //否则判断是否需要更新节点,判断g值
                                // if (openMinHeap.HasItem(tempNode))
                                if (openFlag.TryGetValue(tempNode, out byte b))
                                {
                                    if (b == 1)
                                    {
                                        if (tempNode.G > preNode.G + 1)
                                        {
                                            tempNode.G = preNode.G + 1;
                                            tempNode.ParentNode = preNode;
                                            tempNode.F = tempNode.G + tempNode.H;
                                            //由于使用了最小堆，所以这里F值变化时需要及时调整堆
                                            openMinHeap.Adjust(tempNode);
                                        }
                                    }
                                }
                                else
                                {
                                    tempNode.ParentNode = curNode;
                                    tempNode.G = preNode.G + 1;
                                    tempNode.H = mapGrid.GetDistance(endNode, tempNode);
                                    tempNode.F = tempNode.G + tempNode.H;
                                    openMinHeap.Insert(tempNode);
                                    openFlag.Add(tempNode, 1);
                                }
                            }
                        }
                    }
                }
            }

            if (havePath)
            {
                path.Add(preNode);
                while (preNode.ParentNode != null)
                {
                    path.Add(preNode.ParentNode);
                    Debug.DrawLine(
                        mapGrid.GetNodeWorldPosition(preNode.Index1, preNode.Index2) + halfGridSize,
                        mapGrid.GetNodeWorldPosition(preNode.ParentNode.Index1, preNode.ParentNode.Index2) +
                        halfGridSize, Color.red, 1f);
                    preNode = preNode.ParentNode;
                }

                if (path.Count > 1)
                {
                    path.Reverse();
                }
            }

            return path;
        }

        private int GetKey(int x, int y)
        {
            return x * _width + y;
        }
    }
}