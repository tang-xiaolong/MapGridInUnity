using System.Collections.Generic;
using LDataStruct;
using LMap;
using UnityEngine;

namespace LPathSearch
{
    /// <summary>
    /// 相对原来的最小堆，优化了OpenList和CloseList的判定
    /// OpenList和CloseList原来是使用字典来存放，这样每次判断是否在列表内可以通过一次哈希来确定。
    /// 全局增加一个SectionId，标记这是第几次寻路，每个节点增加两个SectionId, 来判定是否在两个列表中。
    /// 现在每次寻路前，全局SectionId自增1。
    /// 同时，每个Node放进OpenList时，将Node的的OpenSectionId设置为全局SectionId，每次取出时，设置为全局SectionId-1，这样判断是否在OpenList时可以通过对比Node的OpenSectionId和全局的SectionId
    /// 而原本放入CloseList的操作改成修改Node的CloseSectionId。判断是否在CloseList就可以改成直接使用Node的CloseSectionId与全局的SectionId来判断了。
    /// 对应sectionId与寻路组件的不一致，表示没有在对应列表内。
    /// 全局每次自增1是为了去除寻路结束后或者寻路开始时对节点的初始化处理。这种一次性的初始化对于短路径来说，消耗占比比较大。
    /// </summary>
    public class AStarMinHeapPro : IPathSearch
    {
        private uint _sessionId;
        private int _width;

        public List<IPathNode> SearchPath(IMapGrid mapGrid, Vector2Int startPoint, Vector2Int endPoint)
        {
            _width = mapGrid.GetWidth();
            _sessionId += 1;
            Vector3 halfGridSize = new Vector3(mapGrid.GetMapNodeSize() * .5f, mapGrid.GetMapNodeSize() * .5f);
            Heap<IPathNode> openMinHeap = new Heap<IPathNode>((_width + 1) * 2, HeapType.MinHeap);
            List<IPathNode> path = new List<IPathNode>();
            IPathNode preNode = null;
            bool havePath = false;
            //先将起点放入openList中，然后重复后续步骤
            //从openList中取出优先级最高的点curNode
            //判断curNode是否是终点? 如果是终点，结束寻路回溯路径返回
            //如果不是终点，将这个节点放入closeList中，并判断这个节点的周围四通节点是否在closeList中。
            //如果不在，计算fgh三个值,如果f值更优，更新f值同时更新它的父节点为自己，并把它放入openList中

            IPathNode starNode = mapGrid.GetValue<IPathNode>(startPoint.x, startPoint.y);
            IPathNode endNode = mapGrid.GetValue<IPathNode>(endPoint.x, endPoint.y);
            starNode.ParentNode = null;
            // starIPathNode.Source = -1;
            starNode.G = 0;
            starNode.H = mapGrid.GetDistance(starNode, endNode);
            starNode.F = starNode.G + starNode.H;
            openMinHeap.Insert(starNode);
            starNode.OpenSessionId = _sessionId;
            Vector2Int checkNode = Vector2Int.zero;
            while (openMinHeap.Count > 0)
            {
                IPathNode curNode = openMinHeap.DeleteHead();
                curNode.OpenSessionId = _sessionId - 1;
                curNode.CloseSessionId = _sessionId;
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
                        checkNode.x = Mathf.FloorToInt(curNode.Index1 + vector2Int.x);
                        checkNode.y = Mathf.FloorToInt(curNode.Index2 + vector2Int.y);
                        IPathNode tempNode;
                        //有效区域 可通行 且不在close表  sectionId不一致，表示没有被放入过CloseList
                        if (mapGrid.HasInMap(checkNode.x, checkNode.y))
                        {
                            tempNode = mapGrid.GetValue<IPathNode>(checkNode.x, checkNode.y);
                            if (tempNode.CanPass() && tempNode.CloseSessionId != _sessionId)
                            {
                                //如果不在Open表，需要创建新节点并加入到Open表
                                //否则判断是否需要更新节点,判断g值
                                // if (tempNode.IsOpen)
                                if (tempNode.OpenSessionId == _sessionId)
                                {
                                    if (tempNode.G > preNode.G + 1)
                                    {
                                        tempNode.G = preNode.G + 1;
                                        tempNode.ParentNode = preNode;
                                        // tempNode.Source = 3 - i;
                                        tempNode.F = tempNode.G + tempNode.H;
                                        //由于使用了最小堆，所以这里F值变化时需要及时调整堆
                                        openMinHeap.Adjust(tempNode);
                                    }
                                }
                                else
                                {
                                    tempNode.ParentNode = curNode;
                                    // tempNode.Source = 3 - i;
                                    tempNode.G = preNode.G + 1;
                                    tempNode.H = mapGrid.GetDistance(endNode, tempNode);
                                    tempNode.F = tempNode.G + tempNode.H;
                                    openMinHeap.Insert(tempNode);
                                    tempNode.OpenSessionId = _sessionId;
                                }
                            }
                        }
                    }
                }
            }

            if (havePath)
            {
                path.Add((IPathNode)preNode);
                while (preNode.ParentNode != null)
                {
                    var node = preNode.ParentNode;
                    path.Add((IPathNode)node);
                    Debug.DrawLine(
                        mapGrid.GetNodeWorldPosition(preNode.Index1, preNode.Index2) + halfGridSize,
                        mapGrid.GetNodeWorldPosition(node.Index1, node.Index2) +
                        halfGridSize, Color.red, 1f);
                    preNode = node;
                }

                // while (preNode.Source != 0)
                // {
                //     var d = dir[preNode.Source];
                //     var tmpNode = grid.GetValue(preNode.Index1 + (int) d.x, preNode.Index2 + (int) d.y);
                //     path.Add(tmpNode);
                //     Debug.DrawLine(
                //         grid.GetPosition(preNode.Index1, preNode.Index2) + halfGridSize,
                //         grid.GetPosition(tmpNode.Index1, tmpNode.Index2) +
                //         halfGridSize, Color.red, 1f);
                //     preNode = tmpNode;
                //
                // }

                if (path.Count > 1)
                {
                    path.Reverse();
                }
            }

            // int pathCount = path.Count;
            // for (int i = 0; i < pathCount; i++)
            // {
            //     path[i].position = grid.GetPosition(path[i].Index1, path[i].Index2) + halfGridSize;
            // }
            //Debug.Log(havePath);
            return path;
        }
    }
}