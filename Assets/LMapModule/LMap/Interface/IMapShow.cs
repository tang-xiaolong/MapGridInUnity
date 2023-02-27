using System;
using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public interface IMapShow : IDisposable
    {
        void SetNode(GameObject node);
        void Show(IMapGrid mapGrid);
        INodeEntity GetNodeEntity(INode logicNode);
        Vector3 Position();
        string GetShowInfoByIndex(Vector3Int index);
        
        //高亮相关方法
        void SetNormal();
        
        //高亮范围内的格子
        List<Vector3> HighLightRangeNode(Vector3 position, int highLightOutRange, int highLightInRange = 0);

        //高亮指向方向的格子
        List<Vector3> HighLightLineNode(Vector3 position, Vector3 targetPosition, int highLightOutRange, int highLightInRange = 0);


        //高亮扇形范围内格子
        List<Vector3> HighLightSectorNode(Vector3 position, Vector3 targetPosition, int angleWidth, int highLightOutRange,
            int highLightInRange = 0);
    }
}