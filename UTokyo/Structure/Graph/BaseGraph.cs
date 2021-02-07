using C5;

namespace UTokyo.Structure.Graph
{
    public abstract class BaseGraph<TNodeType>
    {
        public delegate object NodeAccessStrategy(GraphNode<TNodeType> node);
        public delegate BaseEdge EdgeAddStrategy(GraphNode<TNodeType> sN, GraphNode<TNodeType> eN, BaseGraph<TNodeType> graph);
        public abstract void AddEdge(GraphNode<TNodeType> startNode, GraphNode<TNodeType> endNode, BaseEdge edge);
        public abstract void DeleteEdge(GraphNode<TNodeType> startNode, GraphNode<TNodeType> endNode, BaseEdge edge);
        public abstract void ChangeEdge(GraphNode<TNodeType> startNode, GraphNode<TNodeType> endNode, BaseEdge edge);
        public abstract System.Collections.Generic.HashSet<GraphNode<TNodeType>> GetExtendNodes(
            GraphNode<TNodeType> node);
        public abstract System.Collections.Generic.HashSet<GraphNode<TNodeType>> GetExtendNodes(
           int node);
    }
}