using UnityEngine;

namespace LMap
{
    public class HexMapShow : NormalMapShow
    {
        public bool ShowY;

        public override string GetShowInfoByIndex(Vector3Int index)
        {
            if (ShowY)
                return "C=" + index.x + "\nL=" + index.y + "\nR=" + index.z;
            else
            {
                return "C=" + index.x + "\nL=" + index.y;
            }
        }
    }
}