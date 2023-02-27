using System;
using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
    /// <summary>
    /// 普通地图节点
    /// </summary>
    public interface INode : IDisposable, IComparable
    {
        int Index1 { get; set; }
        int Index2 { get; set; }
        int Index3 { get; set; }
        Vector3 Position { get; set; }
    }
}