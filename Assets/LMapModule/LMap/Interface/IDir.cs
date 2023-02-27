using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public interface IDir
    {
        List<Vector2Int> GetDirIndexOffset();
    }
}