using System;
using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public interface IMapGrid : IDisposable
    {
        int GetWidth();
        int GetHeight();
        float GetMapNodeSize();
        T GetValue<T>(int index1, int index2) where T : class, INode;
        MapCoordinate GetMapCoordinate();
        List<Vector2Int> GetNeighborsIndexOffset();
        float GetDistance(IPathNode node1, IPathNode node2);
        bool HasInMap(int index1, int index2);
        //返回地图所用的坐标系中的索引
        Vector2Int GetMapIndex(Vector3 pos);
        //通过普通索引获取地图所用坐标系的索引
        Vector3Int GetShowIndexByNormalIndex(int line, int row);
        Vector2Int GetNormalIndexByShowIndex(int index1, int index2);
        Vector3 GetNodeWorldPosition(int index1, int index2);
        Vector3 GetMapPos();
        //获取范围内的节点
        void GetRangeNode(ref List<INode> resNode, Vector3 pos, int inRange, int outRange,
            Func<INode, bool> checkFunc = null);
        void GetLineNode(ref List<INode> resNode, Vector3 pos, Vector3 targetPos, int inRange, int outRange);
        void GetSectorNode(ref List<INode> resNode, Vector3 pos, Vector3 targetPos, int sectorWidth, int inRange,
            int outRange);
        void BindMapGridShow(IMapShow mapShow);
        IMapShow GetMapGridShow();
    }
}