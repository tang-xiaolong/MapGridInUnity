namespace LMap
{
    public class TestNodeFactory : INodeFactory
    {
        public T CreateNode<T>() where T : INode, new()
        {
            return new T();
        }
    }
}