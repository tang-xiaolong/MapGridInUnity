using System;
using UnityEngine;

namespace LMap
{
    public class MapNode : IPathNode
    {
        private readonly NodeType nodeType;
        private IPathNode _parent;
        private bool _disposed;

        public override string ToString()
        {
            // return $"[Index1 = {Index1},Index2 = {Index2}, F = {F}, G = {G}, H = {H} Position = {position}]";
            return $"{Index1}_{Index2}: {F}";
        }

        public int CompareTo(object obj)
        {
            return CompareTo((MapNode)obj);
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
                    _parent = null;
                }

                _disposed = true;
            }
            
        }
        
        ~MapNode()
        {
            Dispose(false);
        }

        public int CompareTo(MapNode other)
        {
            if (other == null)
            {
                return 1;
            }

            if (F > other.F)
                return 1;
            else if (F < other.F)
                return -1;
            else
            {
                if (H > other.H)
                    return 1;
                else if (H < other.H)
                    return -1;
                else
                    return 0;
            }
        }

        public int Index1 { get; set; }
        public int Index2 { get; set; }
        public int Index3 { get; set; }
        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        IPathNode IPathNode.ParentNode
        {
            get => _parent;
            set => _parent = value;
        }

        public uint CloseSessionId { get; set; }
        public uint OpenSessionId { get; set; }
        public Vector3 Position { get; set; }

        public bool CanPass()
        {
            return nodeType == NodeType.Walkable;
        }
    }
}