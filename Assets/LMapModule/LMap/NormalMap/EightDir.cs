using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public class EightDir : IDir
    {
        //获取八方向的index偏移
        static List<Vector2Int> _eightDirectionVec => new List<Vector2Int>()
        {
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 0),
        };

        public List<Vector2Int> GetDirIndexOffset()
        {
            return _eightDirectionVec;
        }
    }
}