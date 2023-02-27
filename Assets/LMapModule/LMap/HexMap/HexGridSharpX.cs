using System;
using UnityEngine;

namespace LMap
{
    public class HexGridSharpX : HexGrid
    {
        public HexGridSharpX(Func<INode> createNodeFunc, int width, int height, float outnerSize, MapCoordinate mapCoordinate,
            bool startNodeHasOffset = false, Func<int, int, bool> shapeFilterFunc = null)
        {
            _width = width;
            _height = height;
            _outnerSize = outnerSize;
            _innerSize = _outnerSize * Mathf.Sqrt(3) * 0.5f;
            _startNodeHasOffset = startNodeHasOffset;
            _mapCoordinate = mapCoordinate;
            _shapeFilterFunc = shapeFilterFunc;
            _gridData = new INode[height, width];
            //一行一行处理
            for (int line = 0; line < height; line++)
            {
                for (int row = 0; row < width; row++)
                {
                    if (_shapeFilterFunc == null || _shapeFilterFunc(line, row))
                    {
                        INode node = createNodeFunc();
                        var axisVec = GetShowIndexByNormalIndex(line, row);
                        node.Index1 = axisVec.x;
                        node.Index2 = axisVec.y;
                        node.Index3 = axisVec.z;
                        Debug.Log($"{line}_{row}  {axisVec.x}_{axisVec.y} {node.Index1}_{node.Index2}_{node.Index3}");
                        _gridData[line, row] = node;
                    }
                }
            }
        }

        public sealed override Vector3Int GetShowIndexByNormalIndex(int line, int row)
        {
            var axisCoordinate = HexMapUtil.Oc2AcShapeX(line, row, _startNodeHasOffset);
            return new Vector3Int(axisCoordinate.x, axisCoordinate.y,
                HexMapUtil.Ac2Cc(axisCoordinate.x, axisCoordinate.y));
        }

        public sealed override Vector2Int GetNormalIndexByShowIndex(int index1, int index2)
        {
            return HexMapUtil.Ac2OcShapeX(index1, index2, _startNodeHasOffset);
        }

        public sealed override Vector2Int GetMapIndex(Vector3 pos)
        {
            var resPos = Vector3.one;
            var mapPos = GetMapPos();
            switch (_mapCoordinate)
            {
                case MapCoordinate.XY:
                    pos.x -= mapPos.x;
                    pos.y -= mapPos.y;
                    pos.x -= _outnerSize;
                    pos.y -= (_innerSize + (_startNodeHasOffset ? _innerSize : 0));
                    resPos.x = pos.x;
                    resPos.z = pos.y;
                    break;
                case MapCoordinate.XZ:
                    //先减去地图的偏移
                    pos.x -= mapPos.x;
                    pos.z -= mapPos.z;
                    pos.x -= _outnerSize;
                    pos.z -= (_innerSize + (_startNodeHasOffset ? _innerSize : 0));
                    resPos.x = pos.x;
                    resPos.z = pos.z;
                    break;
            }

            return HexMapUtil.Pos2AcSharpX(_outnerSize, _innerSize, resPos.x, resPos.z);
        }

        public sealed override Vector3 GetNodeWorldPosition(int index1, int index2)
        {
            var normalIndex = GetNormalIndexByShowIndex(index1, index2);
            index1 = normalIndex.x;
            index2 = normalIndex.y;
            var pos = GetMapPos();
            bool isOdd;
            switch (_mapCoordinate)
            {
                case MapCoordinate.XY:
                    pos.x = pos.x + _outnerSize + index2 * _outnerSize * 1.5f;
                    //初始index从0开始，因此等于0是奇数
                    isOdd = (index2 & 1) == 0;
                    if (_startNodeHasOffset)
                        isOdd = !isOdd;
                    pos.y = pos.y + _innerSize + (isOdd ? 0 : _innerSize) + index1 * _innerSize * 2;

                    break;
                case MapCoordinate.XZ:
                    pos.x = pos.x + _outnerSize + index2 * _outnerSize * 1.5f;
                    //初始index从0开始，因此等于0是奇数
                    isOdd = (index2 & 1) == 0;
                    if (_startNodeHasOffset)
                        isOdd = !isOdd;
                    pos.z = pos.z + _innerSize + (isOdd ? 0 : _innerSize) + index1 * _innerSize * 2;

                    break;
            }

            return pos;
        }
    }
}