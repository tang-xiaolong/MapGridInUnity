namespace LMap
{
    public interface INodeFactory
    {
        public T CreateNode<T>() where T : INode, new();
    }
}