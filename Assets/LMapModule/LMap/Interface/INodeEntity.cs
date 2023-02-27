using System;
using UnityEngine;

namespace LMap
{
    public interface INodeEntity: IDisposable
    {
        void SetToNormal();
        void OnNodeInSelectRange();
        void OnNodeInEffectRange();
        void OnNodeInEffectCenter();
    }
}