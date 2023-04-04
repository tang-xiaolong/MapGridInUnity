using System;
using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public class HexGrid : IMapGrid
    {
        protected int _width;
        protected int _height;
        protected MapCoordinate _mapCoordinate = MapCoordinate.XZ;
        
        protected INode[,] _gridData;
        protected float _outnerSize = 1f;
        protected float _innerSize = 1f;
        protected bool _startNodeHasOffset = false;
        protected Func<int, int, bool> _shapeFilterFunc;
        protected IMapShow _mapShow = null;
        protected HexDir _hexDir = new HexDir();
        private bool _disposed;

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public float GetMapNodeSize()
        {
            return _outnerSize;
        }

        public TNode GetValue<TNode>(int index1, int index2) where TNode : class, INode
        {
            if (HasInMap(index1, index2))
            {
                var offsetIndex = GetNormalIndexByShowIndex(index1, index2);
                return _gridData[offsetIndex.x, offsetIndex.y] as TNode;
            }

            return null;
        }

        public virtual Vector3Int GetShowIndexByNormalIndex(int line, int row)
        {
            return Vector3Int.zero;
        }

        public virtual Vector2Int GetNormalIndexByShowIndex(int index1, int index2)
        {
            return Vector2Int.zero;
        }


        public MapCoordinate GetMapCoordinate()
        {
            return _mapCoordinate;
        }

        public List<Vector2Int> GetNeighborsIndexOffset()
        {
            return _hexDir.GetDirIndexOffset();
        }

        public float GetDistance(IPathNode node1, IPathNode node2)
        {
            var z1 = -node1.Index1 - node1.Index2;
            var z2 = -node2.Index1 - node2.Index2;
            // ReSharper disable once PossibleLossOfFraction
            return (Mathf.Abs(node1.Index1 - node2.Index1) + Mathf.Abs(node1.Index2 - node2.Index2) + Mathf.Abs(z1 - z2)) / 2;
        }


        public bool HasInMap(int index1, int index2)
        {
            var offsetIndex = GetNormalIndexByShowIndex(index1, index2);
            return offsetIndex.x < _height && offsetIndex.y < _width && offsetIndex.x >= 0 && offsetIndex.y >= 0;
        }

        public virtual Vector2Int GetMapIndex(Vector3 pos)
        {
            return Vector2Int.zero;
        }

        public virtual Vector3 GetNodeWorldPosition(int index1, int index2)
        {
            return Vector3.zero;
        }

        public Vector3 GetMapPos()
        {
            return _mapShow?.Position() ?? Vector3.zero;
        }

        public void GetRangeNode(ref List<INode> resNode, Vector3 pos, int inRange, int outRange,
            Func<INode, bool> checkFunc = null)
        {
            if (resNode == null)
                resNode = new List<INode>();
            else
                resNode.Clear();
            var nodeIndex = GetMapIndex(pos);
            for (int c = -outRange; c <= outRange; c++)
            {
                var lMin = Mathf.Max(-outRange, -c - outRange);
                var lMax = Mathf.Min(outRange, -c + outRange);
                for (int l = lMin; l <= lMax; l++)
                {
                    var node = GetValue<INode>(c + nodeIndex.x, l + nodeIndex.y);
                    if (node != null && (checkFunc == null || checkFunc(node)))
                        resNode.Add(node);
                }
            }

            //包在inRange以内的要去除
            for (int c = -inRange; c <= inRange; c++)
            {
                var lMin = Mathf.Max(-inRange, -c - inRange);
                var lMax = Mathf.Min(inRange, -c + inRange);
                for (int l = lMin; l <= lMax; l++)
                {
                    var node = GetValue<INode>(c + nodeIndex.x, l + nodeIndex.y);
                    if (node != null && (checkFunc == null || checkFunc(node)))
                        resNode.Remove(node);
                }
            }
        }

        public void GetSectorNode(ref List<INode> resNode, Vector3 pos, Vector3 targetPos, int sectorWidth, int inRange,
            int outRange)
        {
            if (resNode == null)
                resNode = new List<INode>();
            else
                resNode.Clear();
            if (sectorWidth == 0)
            {
                GetLineNode(ref resNode, pos, targetPos, inRange, outRange);
            }
            else if (sectorWidth >= 3)
            {
                GetRangeNode(ref resNode, pos, inRange, outRange);
            }
            else
            {
                var nodeIndex = GetMapIndex(pos);
                pos = GetNodeWorldPosition(nodeIndex.x, nodeIndex.y);
                var forwardVec = (targetPos - pos).normalized;
                Vector2Int maxSameDirVec = GetMatchDirVec(forwardVec, nodeIndex, pos);
                forwardVec = GetNodeWorldPosition(maxSameDirVec.x + nodeIndex.x, maxSameDirVec.y + nodeIndex.y) - pos;
                //正常是60，这里61是为了防止精度产生误差
                float maxAngle = 61 * sectorWidth;
                GetRangeNode(ref resNode, pos, inRange, outRange, (checkNode) =>
                {
                    return Vector3.Angle(GetNodeWorldPosition(checkNode.Index1, checkNode.Index2) - pos, forwardVec) <=
                           maxAngle;
                });
            }
        }

        public void GetLineNode(ref List<INode> resNode, Vector3 pos, Vector3 targetPos, int inRange, int outRange)
        {
            if (resNode == null)
                resNode = new List<INode>();
            else
                resNode.Clear();
            var mapIndex = GetMapIndex(pos);
            var curNode = GetValue<INode>(mapIndex.x, mapIndex.y);
            var curNodePos = GetNodeWorldPosition(mapIndex.x, mapIndex.y);
            //find dir
            var originDir = (targetPos - pos).normalized;
            Vector2Int maxSameDirVec = GetMatchDirVec(originDir, mapIndex, curNodePos);
            for (int i = 0; i <= outRange; i++)
            {
                if (curNode == null)
                    break;
                if (i > inRange)
                    resNode.Add(curNode);
                curNode = GetValue<INode>(curNode.Index1 + maxSameDirVec.x, curNode.Index2 + maxSameDirVec.y);
            }
        }

        Vector2Int GetMatchDirVec(Vector3 originDir, Vector2Int originNodeIndex, Vector3 originPos)
        {
            float maxSameDirDotValue = -1;
            Vector2Int maxSameDirVec = Vector2Int.zero;
            var offsetIndex = GetNeighborsIndexOffset();
            for (int i = 0; i < offsetIndex.Count; i++)
            {
                var tempDir =
                    (GetNodeWorldPosition(originNodeIndex.x + offsetIndex[i].x, originNodeIndex.y + offsetIndex[i].y) -
                     originPos).normalized;
                var tempDotValue = Vector3.Dot(tempDir, originDir);
                if (tempDotValue > maxSameDirDotValue)
                {
                    maxSameDirDotValue = tempDotValue;
                    maxSameDirVec = offsetIndex[i];
                }
            }

            return maxSameDirVec;
        }

        public void BindMapGridShow(IMapShow mapShow)
        {
            if (_mapShow != mapShow)
            {
                if (_mapShow != null)
                {
                    _mapShow.Dispose();
                }

                _mapShow = mapShow;
                if (_mapShow != null)
                {
                    //Refresh Node Position
                    RefreshNodePosition();
                    _mapShow.Show(this);
                }
            }
        }

        void RefreshNodePosition()
        {
            for (int line = 0; line < _height; line++)
            {
                for (int row = 0; row < _width; row++)
                {
                    Vector3Int nodeIndex = GetShowIndexByNormalIndex(line, row);
                    INode node = GetValue<INode>(nodeIndex.x, nodeIndex.y);
                    node.Position = GetNodeWorldPosition(node.Index1, node.Index2);
                }
            }
        }

        public IMapShow GetMapGridShow()
        {
            return _mapShow;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            
            if(_disposed)
                return;
            if (disposing)
            {
                if (_gridData != null)
                {
                    for (int i = 0; i < _width; i++)
                    {
                        for (int j = 0; j < _height; j++)
                        {
                            var node = _gridData[j, i];
                            if (node != null)
                            {
                                node.Dispose();
                            }
                        }
                    }
                }

                BindMapGridShow(null);
            }
            
            _disposed = true;
        }

        ~HexGrid()
        {
            Dispose(false);
        }
    }
}