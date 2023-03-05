using UnityEngine;

namespace LMap
{
    public static class HexMapUtil
    {
        public static Vector2Int Oc2AcShapeY(int line, int row, bool hasOffset = false)
        {
            return new Vector2Int(row - (line + (hasOffset ? 1 : 0)) / 2, line);
        }

        public static Vector2Int Oc2AcShapeX(int line, int row, bool hasOffset = false)
        {
            return new Vector2Int(row, line - (row + (hasOffset ? 1 : 0)) / 2);
        }

        public static Vector2Int Ac2OcShapeY(int iC, int iL, bool hasOffset = false)
        {
            return new Vector2Int(iL, iC + (iL + (hasOffset ? 1 : 0)) / 2);
        }

        public static Vector2Int Ac2OcShapeX(int iC, int iL, bool hasOffset = false)
        {
            return new Vector2Int(iL + (iC + (hasOffset ? 1 : 0)) / 2, iC);
        }

        public static int Ac2Cc(int index1, int index2)
        {
            return -index1 - index2;
        }

        public static float Ac2Cc(float index1, float index2)
        {
            return -index1 - index2;
        }

        public static Vector2Int Pos2AcSharpX(float outerRadius, float inRadius, float posIndex1, float posIndex2)
        {
            float l = posIndex2 / (inRadius * 2);
            float r = -l;

            float offset = posIndex1 / (outerRadius * 3f);
            l -= offset;
            r -= offset;

            //C L R
            int iX = Mathf.RoundToInt(Ac2Cc(l, r));
            int iY = Mathf.RoundToInt(l);
            int iZ = Mathf.RoundToInt(r);

            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(Ac2Cc(l, r) - iX);
                float dY = Mathf.Abs(l - iY);
                float dZ = Mathf.Abs(r - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = Ac2Cc(iY, iZ);
                }
                else if (dZ > dY)
                {
                    iZ = Ac2Cc(iX, iY);
                }
            }

            return new Vector2Int(iX, iY);
        }

        public static Vector2Int Pos2AcSharpY(float outerRadius, float inRadius, float posIndex1, float posIndex2)
        {
            float c = posIndex1 / (inRadius * 2);
            float r = -c;

            float offset = posIndex2 / (outerRadius * 3f);
            c -= offset;
            r -= offset;


            int iC = Mathf.RoundToInt(c);
            int iR = Mathf.RoundToInt(r);
            int iL = Mathf.RoundToInt(Ac2Cc(c, r));

            if (iC + iR + iL != 0)
            {
                float dX = Mathf.Abs(c - iC);
                float dY = Mathf.Abs(r - iR);
                float dZ = Mathf.Abs(Ac2Cc(c, r) - iL);

                if (dX > dY && dX > dZ)
                {
                    iC = Ac2Cc(iR, iL);
                }
                else if (dZ > dY)
                {
                    iL = Ac2Cc(iC, iR);
                }
            }

            return new Vector2Int(iC, iL);
        }
    }
}