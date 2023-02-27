using System.Collections.Generic;
using LMap;
using UnityEngine;

namespace LPathSearch
{
    public class AStarSearch : IPathSearch
    {
        private int _width;
        private List<IPathNode> _openList = new List<IPathNode>(200);
        private Dictionary<int, IPathNode> _closeList = new Dictionary<int, IPathNode>(200);
        private Dictionary<IPathNode, byte> _openFlag = new Dictionary<IPathNode, byte>(200);

        public List<IPathNode> SearchPath(IMapGrid mapGrid, Vector2Int startPoint, Vector2Int endPoint)
        {
            _openList.Clear();
            _closeList.Clear();
            _openFlag.Clear();

            _width = mapGrid.GetWidth();
            Vector3 halfGridSize = new Vector3(mapGrid.GetMapNodeSize() * .5f, mapGrid.GetMapNodeSize() * .5f);
            List<IPathNode> path = new List<IPathNode>();
            IPathNode preNode = null;
            bool havePath = false;
            //先将起点放入openList中，然后重复后续步骤
            //从openList中取出优先级最高的点curNode
            //判断curNode是否是终点? 如果是终点，结束寻路回溯路径返回
            //如果不是终点，将这个节点放入closeList中，并判断这个节点的周围四通节点是否在closeList中。
            //如果不在，计算fgh三个值,如果f值更优，更新f值同时更新它的父节点为自己，并把它放入openList中
            IPathNode startNode = mapGrid.GetValue<IPathNode>(startPoint.x, startPoint.y);
            IPathNode endNode = mapGrid.GetValue<IPathNode>(endPoint.x, endPoint.y);
            startNode.G = 0;
            startNode.H = mapGrid.GetDistance(startNode, endNode);
            startNode.F = startNode.G + startNode.H;
            startNode.ParentNode = null;
            _openList.Add(startNode);
            _openFlag.Add(startNode, 1);
            while (_openList.Count > 0)
            {
                IPathNode curNode = _openList[0];
                _openList.Remove(curNode);
                _openFlag[curNode] = 0;
                //curNode.parent = preNode;
                _closeList.Add(GetKey(curNode.Index1, curNode.Index2), curNode);
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
                        Vector2Int checkNode = new Vector2Int((int)(curNode.Index1 + vector2Int.x),
                            (int)(curNode.Index2 + vector2Int.y));
                        //有效区域 可通行 且不在close表
                        if (mapGrid.HasInMap(checkNode.x, checkNode.y))
                        {
                            var tempNode = mapGrid.GetValue<IPathNode>(checkNode.x, checkNode.y);
                            if (tempNode.CanPass() && !_closeList.ContainsKey(GetKey(checkNode.x, checkNode.y)))
                            {
                                //如果不在Open表，需要创建新节点并加入到Open表
                                //否则判断是否需要更新节点,判断g值
                                if (_openFlag.TryGetValue(tempNode, out byte b))
                                {
                                    if (b == 1)
                                    {
                                        if (tempNode.G > preNode.G + 1)
                                        {
                                            tempNode.G = preNode.G + 1;
                                            tempNode.ParentNode = preNode;
                                            tempNode.F = tempNode.G + tempNode.H;
                                        }
                                    }
                                }
                                else
                                {
                                    tempNode.ParentNode = curNode;
                                    tempNode.G = preNode.G + 1;
                                    tempNode.H = mapGrid.GetDistance(endNode, tempNode);
                                    tempNode.F = tempNode.G + tempNode.H;
                                    _openList.Add(tempNode);
                                    _openFlag.Add(tempNode, 1);
                                }

                                int minIndex = 0;
                                float minValue = _openList[0].F;
                                for (int j = 1; j < _openList.Count; j++)
                                {
                                    if (_openList[j].F < minValue)
                                    {
                                        minValue = _openList[j].F;
                                        minIndex = j;
                                    }
                                }

                                (_openList[0], _openList[minIndex]) = (_openList[minIndex], _openList[0]);
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

                if (path.Count > 0)
                {
                    path.Reverse();
                }
            }

            //Debug.Log(havePath);
            return path;
        }

        private int GetKey(int x, int y)
        {
            return x * _width + y;
        }
    }
}