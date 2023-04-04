using System;
using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    public class NormalMapShow : MonoBehaviour, IMapShow
    {
        private GameObject _node;
        private Dictionary<INode, INodeEntity> _mapNodes = new Dictionary<INode, INodeEntity>();
        private Transform _transform;
        private IMapGrid _mapGrid;
        private List<INode> _highLightNodes = new List<INode>();
        private bool _disposed;

        private Transform MyTransform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;
                return _transform;
            }
        }

        public void SetNode(GameObject node)
        {
            _node = node;
        }

        public void Show(IMapGrid mapGrid)
        {
            DisposeNode();
            _mapGrid = mapGrid;
            if (mapGrid != null)
            {
                MapCoordinate mapCoordinate = mapGrid.GetMapCoordinate();
                for (int line = 0; line < mapGrid.GetHeight(); line++)
                {
                    for (int row = 0; row < mapGrid.GetWidth(); row++)
                    {
                        var newNode = Instantiate(_node, MyTransform);
                        newNode.name += $"{line}_{row}";
                        var nodeIndex = mapGrid.GetShowIndexByNormalIndex(line, row);
                        newNode.transform.position = mapGrid.GetNodeWorldPosition(nodeIndex.x, nodeIndex.y);
                        if (mapCoordinate == MapCoordinate.XZ)
                        {
                            newNode.transform.Rotate(newNode.transform.right, 90);
                        }

                        INodeEntity nodeEntity = newNode.GetComponent<INodeEntity>();
                        INode iNode = mapGrid.GetValue<INode>(nodeIndex.x, nodeIndex.y);
                        if (iNode == null)
                        {
                            Debug.Log(nodeIndex);
                        }

                        _mapNodes.Add(iNode, nodeEntity);
                        var hexPosition = newNode.GetComponentInChildren<HexPosition>();
                        if (hexPosition)
                        {
                            hexPosition.ShowPos(GetShowInfoByIndex(nodeIndex));
                        }
                    }
                }
            }
            
        }

        public INodeEntity GetNodeEntity(INode logicNode)
        {
            if (_mapNodes.TryGetValue(logicNode, out var resNode))
            {
                return resNode;
            }

            return null;
        }

        private void DisposeNode()
        {
            if (_mapNodes.Count > 0)
            {
                foreach (KeyValuePair<INode, INodeEntity> keyValuePair in _mapNodes)
                {
                    if (keyValuePair.Value != null)
                    {
                        keyValuePair.Value.Dispose();   
                    }
                }

                _mapNodes.Clear();
            }
        }

        public Vector3 Position()
        {
            return MyTransform.position;
        }

        public virtual string GetShowInfoByIndex(Vector3Int index)
        {
            return index.x + "\n" + index.y;
        }
        
        
        public void SetNormal()
        {
            var mapShow = _mapGrid.GetMapGridShow();
            if (mapShow != null && _highLightNodes != null)
            {
                foreach (INode hightLightNode in _highLightNodes)
                {
                    var nodeEntity = mapShow.GetNodeEntity(hightLightNode);
                    if (nodeEntity != null)
                    {
                        nodeEntity.SetToNormal();
                    }
                }

                _highLightNodes.Clear();
            }
        }
        
        //高亮范围内的格子
        public List<Vector3> HighLightRangeNode(Vector3 position, int highLightOutRange, int highLightInRange = 0)
        {
            SetNormal();
            if (_mapGrid != null)
            {
                _mapGrid.GetRangeNode(ref _highLightNodes, position, highLightInRange,
                    highLightOutRange);
                SetToValid();
            }

            return GetHighLightNodePos();
        }

        List<Vector3> GetHighLightNodePos()
        {
            if (_highLightNodes == null || _highLightNodes.Count == 0)
            {
                return null;
            }
            List<Vector3> res = new List<Vector3>(_highLightNodes.Count);
            for (int i = 0; i < _highLightNodes.Count; i++)
            {
                res.Add(_highLightNodes[i].Position);
            }

            return res;
        }

        public void SetToValid()
        {
            if (_highLightNodes != null)
            {
                foreach (INode hightLightNode in _highLightNodes)
                {
                    var nodeEntity = GetNodeEntity(hightLightNode);
                    if (nodeEntity != null)
                    {
                        nodeEntity.OnNodeInEffectRange();
                    }
                }
            }
        }

        //高亮指向方向的格子
        public List<Vector3> HighLightLineNode(Vector3 position, Vector3 targetPosition, int highLightOutRange, int highLightInRange = 0)
        {
            SetNormal();
            if (_mapGrid != null)
            {
                _mapGrid.GetLineNode(ref _highLightNodes, position, targetPosition,
                    highLightInRange,
                    highLightOutRange);
                SetToValid();
            }
            
            return GetHighLightNodePos();
        }


        //高亮扇形范围内格子
        public List<Vector3> HighLightSectorNode(Vector3 position, Vector3 targetPosition, int angleWidth, int highLightOutRange, int highLightInRange = 0)
        {
            SetNormal();
            if (_mapGrid != null)
            {
                _mapGrid.GetSectorNode(ref _highLightNodes, position, targetPosition,
                    angleWidth, highLightInRange,
                    highLightOutRange);
                SetToValid();
            }
            
            return GetHighLightNodePos();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeNode();
                    _mapNodes = null;
                    _node = null;
                    _transform = null;
                    _mapGrid = null;
                    _highLightNodes = null;
                }

                _disposed = true;
            }
        }
        
        ~NormalMapShow()
        {
            Dispose(false);
        }
    }
}