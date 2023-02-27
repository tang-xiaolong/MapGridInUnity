using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public class FourDir : IDir
    {
        //获取四方向的index偏移
        static List<Vector2Int> _fourDirectionVec => new List<Vector2Int>()
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
        };

        public List<Vector2Int> GetDirIndexOffset()
        {
            return _fourDirectionVec;
        }
    }
}