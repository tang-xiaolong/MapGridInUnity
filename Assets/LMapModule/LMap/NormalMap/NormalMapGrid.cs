using System;
using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public class NormalMapGrid : IMapGrid
    {
        private int _width;
        private int _height;
        private IDir _nodeDir;
        private MapCoordinate _mapCoordinate = MapCoordinate.XZ;

        private INode[,] _gridData;
        private float _gridSize = 1f;
        private IMapShow _mapShow = null;
        protected bool _startNodeHasOffset = false;
        private bool _disposed;

        public NormalMapGrid(Func<INode> createNodeFunc, int width, int height, float gridSize, IDir nodeDir, MapCoordinate mapCoordinate, bool startNodeHasOffset = false)
        {
            _width = width;
            _height = height;
            _gridSize = gridSize;
            _gridData = new INode[width, height];
            _nodeDir = nodeDir;
            _mapCoordinate = mapCoordinate;
            _startNodeHasOffset = startNodeHasOffset; 
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    INode node = createNodeFunc();
                    node.Index1 = i;
                    node.Index2 = j;
                    _gridData[i, j] = node;
                }
            }
        }

        public bool HasInMap(int index1, int index2)
        {
            return index1 < _width && index2 < _height && index1 >= 0 && index2 >= 0;
        }

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
            return _gridSize;
        }


        public Vector2Int GetMapIndex(Vector3 pos)
        {
            var mapPos = GetMapPos();
            switch (_mapCoordinate)
            {
                case MapCoordinate.XY:
                    if (!_startNodeHasOffset)
                    {
                        pos.x += _gridSize * 0.5f;
                        pos.y += _gridSize * 0.5f;
                    }
                    return new Vector2Int(Mathf.FloorToInt((pos.x - mapPos.x) / _gridSize), Mathf.FloorToInt(
                        (pos.y - mapPos.y) / _gridSize));

                case MapCoordinate.XZ:
                    if (!_startNodeHasOffset)
                    {
                        pos.x += _gridSize * 0.5f;
                        pos.z += _gridSize * 0.5f;
                    }
                    return new Vector2Int(Mathf.FloorToInt((pos.x - mapPos.x) / _gridSize),
                        Mathf.FloorToInt((pos.z - mapPos.z) / _gridSize));
            }

            return Vector2Int.one;
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
                for (int l = -outRange; l <= outRange; l++)
                {
                    var node = GetValue<INode>(c + nodeIndex.x, l + nodeIndex.y);
                    if (node != null && (checkFunc == null || checkFunc(node)))
                        resNode.Add(node);
                }
            }

            //包在inRange以内的要去除
            for (int c = -inRange; c <= inRange; c++)
            {
                for (int l = -inRange; l <= inRange; l++)
                {
                    var node = GetValue<INode>(c + nodeIndex.x, l + nodeIndex.y);
                    if (node != null && (checkFunc == null || checkFunc(node)))
                        resNode.Remove(node);
                }
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
        

        public Vector3 GetNodeWorldPosition(int index1, int index2)
        {
            var pos = GetMapPos();
            switch (_mapCoordinate)
            {
                case MapCoordinate.XY:
                    pos.x = pos.x += index1 * _gridSize;
                    pos.y = pos.y += index2 * _gridSize;
                    if (_startNodeHasOffset)
                    {
                        pos.x += _gridSize * 0.5f;
                        pos.y += _gridSize * 0.5f;
                    }
                    break;
                case MapCoordinate.XZ:
                    pos.x = pos.x += index1 * _gridSize;
                    pos.z = pos.z += index2 * _gridSize;
                    if (_startNodeHasOffset)
                    {
                        pos.x += _gridSize * 0.5f;
                        pos.z += _gridSize * 0.5f;
                    }
                    break;
            }

            return pos;
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
            else if (sectorWidth >= 4)
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
                //正常是45，这里46是为了防止精度产生误差
                float maxAngle = 46 * sectorWidth;
                GetRangeNode(ref resNode, pos, inRange, outRange, (checkNode) =>
                {
                    return Vector3.Angle(GetNodeWorldPosition(checkNode.Index1, checkNode.Index2) - pos, forwardVec) <=
                           maxAngle;
                });
            }
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
                    _mapShow.Show(this);
                }
            }
        }

        public IMapShow GetMapGridShow()
        {
            return _mapShow;
        }

        public T1 GetValue<T1>(int index1, int index2) where T1 : class, INode
        {
            if (HasInMap(index1, index2))
            {
                return _gridData[index1, index2] as T1;
            }

            return default;
        }

        public Vector3Int GetShowIndexByNormalIndex(int line, int row)
        {
            return new Vector3Int(line, row, 0);
        }

        public Vector2Int GetNormalIndexByShowIndex(int index1, int index2)
        {
            return new Vector2Int(index1, index2);
        }

        public MapCoordinate GetMapCoordinate()
        {
            return _mapCoordinate;
        }

        public List<Vector2Int> GetNeighborsIndexOffset()
        {
            return _nodeDir.GetDirIndexOffset();
        }

        public float GetDistance(IPathNode node1, IPathNode node2)
        {
            if (_nodeDir is FourDir)
            {
                return Mathf.Abs(node1.Index1 - node2.Index1) + Mathf.Abs(node1.Index2 - node2.Index2);
            }
            else
            {
                return Vector3.Distance(node1.Position, node1.Position);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                if (_gridData != null)
                {
                    for (int i = 0; i < _width; i++)
                    {
                        for (int j = 0; j < _height; j++)
                        {
                            var node = _gridData[i, j];
                            if (node != null)
                            {
                                node.Dispose();
                            }
                        }
                    }

                    _gridData = null;
                }

                BindMapGridShow(null);
            }

            _disposed = true;
        }

        ~NormalMapGrid()
        {
            Dispose(false);
        }
    }
}