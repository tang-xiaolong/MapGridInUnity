namespace LMap
{
    /// <summary>
    /// 可寻路的节点
    /// </summary>
    public interface IPathNode : INode
    {
        float F { get; set; }
        float G { get; set; }
        float H { get; set; }
        IPathNode ParentNode { get; set; }
        uint CloseSessionId { get; set; }
        uint OpenSessionId { get; set; }
        bool CanPass();
    }
}