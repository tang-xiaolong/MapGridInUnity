using System.Collections.Generic;
using LMap;
using UnityEngine;

namespace LPathSearch
{
    public interface IPathSearch
    {
        List<IPathNode> SearchPath(IMapGrid mapGrid, Vector2Int startPoint, Vector2Int endPoint);
    }
}
