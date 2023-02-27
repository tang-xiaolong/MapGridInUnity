using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public class HexDir : IDir
    {
        static List<Vector2Int> _hexDirectionVec => new List<Vector2Int>()
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1)
        };

        public List<Vector2Int> GetDirIndexOffset()
        {
            return _hexDirectionVec;
        }
    }
}