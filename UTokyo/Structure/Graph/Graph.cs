using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using C5;
using UTokyo.Math;

namespace UTokyo.Structure.Graph
{

    public static class GraphTest
    {
        public static void Test()
        {
            var matrixGraph = new MatrixGraph<int>(5);

            var n1 = matrixGraph.AddNode(0);
            var n2 = matrixGraph.AddNode(1);
            var n3 = matrixGraph.AddNode(1);
            matrixGraph.AddEdge(n1, n2, GraphEdge.FromObject(5));
            matrixGraph.Edges.Count.PrintToConsole();
            matrixGraph.Nodes.Count.PrintToConsole();
            matrixGraph.GetExtendNodes(matrixGraph[1]).Count.PrintToConsole();
        }


        public static (List<List<int>>, int[,]) Floyd(MatrixGraph<int> matrixGraph)
        {
            var n = matrixGraph.NodeCount;
            var dist = Utils.CreateTwoDimensionList(new int[n * n].Select(e => int.MaxValue).ToArray(), n, n);
            var previous = new int[n, n];
            for (var i = 0; i < n; i++)
            {
                dist[i][i] = 0;
                previous[i, i] = i;
            }
            //init
            foreach (var e in matrixGraph.Edges)
            {
                dist[e.From][e.To] = (int) matrixGraph[e.From, e.To].Data;
            }

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    for (var k = 0; k < n; k++)
                    {
                        var c = dist[i][j] + dist[j][k];
                        if (c < dist[i][k])
                        {
                            dist[i][k] = c;
                            previous[i, k] = j;
                        }
                    }
                }
            }
            return (dist, previous);
        }
        
        //dfs和回溯是相似的。path其实就是当前路径。遍历某个节点时扔进去，回溯的时候弹出。因为是iddfs要有limit,还要记录目前的cost
        public static (int cost, bool found) IDAStartDFS(MatrixGraph<int> matrixGraph, List<int> path, int g, int limit, int target)
        {
            if (!path.Any())
                return (g, false);
            var lastNode = path.Last();
            var h = 0; //估计函数
            var f = g + h;
            ///////////叶子判断
            //超过限制则返回
            if (f > limit)
                return (f, false);
            //找到目标
            if (lastNode == target)
                return (f, true);
            
            var min = int.MaxValue;
            
            
            ///////////分支操作
            //任意没访问过的节点进行dfs
            foreach (var e in 
                matrixGraph.GetExtendNodes(matrixGraph[lastNode]).Where(e => !path.Contains(matrixGraph[e])))
            {
                var x = matrixGraph[e];
                //dfs
                path.Add(x);
                
                var t = IDAStartDFS(matrixGraph, path, g + (int) (matrixGraph[lastNode, x].Data), limit, target);
                //搜索到惹
                if (t.found)
                    return t;
                if (t.cost < min)
                    min = t.cost;
                
                //回溯惹
                path.Remove(path.Count - 1);
            }

            return (min, false);
        }
        
        

        //多源最短惹。NB，但是O(n^3惹惹惹)
        public static void FloydWarshall(MatrixGraph<int> matrixGraph)
        {
            var n = matrixGraph.NodeCount;
            var mat = new Matrix<int>(n, n, int.MaxValue);
            var previous = new int[n, n];
            for (var i = 0; i < n; i++)
            {
                mat[i, i] = 0;
            }

            foreach (var x in matrixGraph.Edges)
            {
                mat[x.From, x.To] = mat[x.From, x.To];
            }

            for (var x = 0; x < n; x++)
            {
                for (var y = 0; y < n; y++)
                {
                    for (var z = 0; z < n; z++)
                    {
                        if (mat[x, z] > mat[x, y] + mat[y, z])
                        {
                            mat[x, z] = mat[x, y] + mat[y, z];
                            previous[x, z] = y; //以x为起点的最短路径下，z的前置是什么
                        }
                    }
                }
            }
        }
        
        
        //O(VE) 单源最短，允许负边
        public static void BellmanFord(MatrixGraph<int> matrixGraph, int start)
        {
            var n = matrixGraph.NodeCount;
            var previous = new int[n];
            var distance = new int[n].Select(e => int.MaxValue).ToArray();
            distance[start] = 0;
            for (var i = 0; i < n; i++)
            {
                //对于所有边
                foreach (var e in matrixGraph.Edges)
                {
                    var u = e.From;
                    var v = e.To;
                    //经过u到v
                    if (distance[v] > distance[u] + (int) matrixGraph[u, v].Data)
                    {
                        distance[v] = distance[u] + (int) matrixGraph[u, v].Data;
                        previous[v] = u;
                    }
                }
            }
        }

        
        //A* - 启发式算法。 这里启发函数=0，相当于迪杰特斯拉
        public static int AStart(MatrixGraph<int> matrixGraph, int start, int target)
        {
            var n = matrixGraph.NodeCount;
            var openSet = new PriorityQueue<int, int>();
            var closeSet = new System.Collections.Generic.HashSet<int>();
            var distance = new int[n].Select(e => int.MaxValue).ToArray();
            var previous = new int[n];
            distance[start] = 0;
            previous[start] = start;
            openSet.EnQueue(0, start);
            
            while (openSet.Any())
            {
                var firstNodeCode = openSet.DeQueue();
                closeSet.Add(firstNodeCode.item);
                foreach (var x in matrixGraph.GetExtendNodes(matrixGraph[firstNodeCode.item])
                    .Where(e => !closeSet.Contains(matrixGraph[e])))
                {
                    var xCode = matrixGraph[x];
                    var newDistance = distance[firstNodeCode.item] + (int) matrixGraph[firstNodeCode.item, xCode].Data;
                    if (newDistance < distance[xCode])
                    {
                        distance[xCode] = newDistance;
                        previous[xCode] = firstNodeCode.item;
                        openSet.UpdateOrSetPriority(xCode, newDistance);
                    }
                }
            }

            return distance[target];

        }
    }

    public abstract class BaseEdge
    {
        public object Data = null;
        public int From;
        public int To;
    }

    public class GraphEdge : BaseEdge
    {
        
        
        public static GraphEdge FromObject(object obj)
        {
            var g = new GraphEdge {Data = obj};
            return g;
        }
    }

    public class GraphNode<TNodeData>
    {
      
        public GraphNode(TNodeData data, int nodeCode)
        {
            Data = data;
            NodeCode = nodeCode;
        }

        public int NodeCode;
        public TNodeData Data { get; set; }
    }

    public class MatrixGraph<TNodeType> : BaseGraph<TNodeType>
    {
        public int NodeCount => Nodes.Count;
        public int EdgeCount => Edges.Count;
        
        //索引 -> 节点
        public List<GraphNode<TNodeType>> Nodes => _nodesList;
        public List<BaseEdge> Edges => 
            _matrix.SelectMany(e => e).Where(e => e != null).ToList();
        private Matrix<BaseEdge> _matrix;


        public GraphNode<TNodeType> this[int nodeIndex]
        {
            get => Nodes[nodeIndex];
        }

        public int this[GraphNode<TNodeType> node]
        {
            get => _nodeMapping[node];
        }
        //编号对应节点
        private List<GraphNode<TNodeType>> _nodesList = new List<GraphNode<TNodeType>>();
        
        //节点对应的编号
        private Dictionary<GraphNode<TNodeType>, int> _nodeMapping = new Dictionary<GraphNode<TNodeType>, int>();

        public MatrixGraph()
        {
            _matrix = new Matrix<BaseEdge>(0,0);
        }

        
        public void Dfs(NodeAccessStrategy accessStrategy, int from)
        {
            var n = _nodesList.Count;
            var stack = new Stack<GraphNode<TNodeType>>();
            var vis = new bool[n];
            stack.Push(_nodesList[from]);
            while (stack.Count != 0)
            {
                var p = _nodeMapping[stack.Peek()];
                if (!vis[p])
                {
                    vis[p] = true;
                    accessStrategy.Invoke(stack.Peek());
                }
                var s = GetExtendNodes(stack.Peek());
                if (s.Count == 0 || s.All(e => vis[_nodeMapping[e]]))
                {
                    stack.Pop();
                    continue;
                }
                var np = s.First(e => vis[_nodeMapping[e]]);
                stack.Push(np);
            }
        }

        
        

        public void Bfs(NodeAccessStrategy accessStrategy, int from)
        {
            var n = _nodesList.Count;
            var queue = new Queue<int>();
            queue.Enqueue(from);
            var vis = new bool[n];
            while (queue.Count != 0)
            {
                var p = queue.Dequeue();
                vis[p] = true;
                accessStrategy.Invoke(_nodesList[p]);
                var ext = GetExtendNodes(_nodesList[p]).Where(e => !vis[_nodeMapping[e]]);
                foreach (var e in ext)
                {
                    queue.Enqueue(_nodeMapping[e]);
                }
            }
        }
        
        
    
        //空白图
        public MatrixGraph(int nodeCount)
        {
            _matrix = new Matrix<BaseEdge>(nodeCount,nodeCount);
            for (var i = 0; i < nodeCount; i++)
            {
                var n = new GraphNode<TNodeType>(default, i + 1);
                _nodesList.Add(n);
                _nodeMapping.Add(n, i + 1);
            }
        }


        public GraphNode<TNodeType> AddNode(TNodeType data)
        {
            var node = new GraphNode<TNodeType>(data,_matrix.ColumnsCount + 1);
            var len = _nodeMapping.Count;
            _matrix.AddARow();
            _matrix.AddColumn();
            _nodeMapping.Add(node,len);
            _nodesList.Add(node);
            return node;
        }
        
        public List<GraphNode<TNodeType>> CreateFromNodes(IEnumerable<TNodeType> nodes)
        {
            //编号与nodes同顺序
            var cur = 0;
            var res = nodes.Select(n => new GraphNode<TNodeType>(n, cur++)).ToList();
            var len = res.Count;
            _matrix = new Matrix<BaseEdge>(len,len);
            _nodeMapping = null;
            _nodeMapping = res.ToDictionary(x => x, x => x.NodeCode);
            _nodesList = null;
            _nodesList = res;
            return res;
        } 
        

        public BaseEdge this[int node1, int node2]
        {
            set
            {
                var n1 = _nodesList[node1];
                var n2 = _nodesList[node2];
                if (n1 == null || n2 == null) return;
                _matrix[node1, node2] = value;
            }
            get
            {
                var nodeCount = _nodeMapping.Count;
                if (node1 >= nodeCount || node2 >= nodeCount) return null;
                return _matrix[node1, node2];
            }
        }
        
        public override void AddEdge(GraphNode<TNodeType> startNode, GraphNode<TNodeType> endNode, BaseEdge edge)
        {
            var nI1 = _nodeMapping[startNode];
            var nI2 = _nodeMapping[endNode];
            edge.From = nI1;
            edge.To = nI2;
            if (nI1 < 0 || nI2 < 0) return;
            this[nI1, nI2] = edge;
        }

        public override void DeleteEdge(GraphNode<TNodeType> startNode, GraphNode<TNodeType> endNode, BaseEdge edge)
        {
            var nI1 = _nodeMapping[startNode];
            var nI2 = _nodeMapping[endNode];
            if (nI1 < 0 || nI2 < 0) return;
            this[nI1, nI2] = default;
        }

        public override void ChangeEdge(GraphNode<TNodeType> startNode, GraphNode<TNodeType> endNode, BaseEdge edge)
        {
            AddEdge(startNode,endNode,edge);
        }

        
        //从某点到达可能的所有节点
        public override System.Collections.Generic.HashSet<GraphNode<TNodeType>> GetExtendNodes(
            GraphNode<TNodeType> node)
        {
            var set = new System.Collections.Generic.HashSet<GraphNode<TNodeType>>();
            var code = _nodeMapping[node];
            if (code < 0) return set;

            var r = _matrix[code];
            for (var i = 0; i < r.Count; i++)
            {
                if (r[i] != null) 
                    set.Add(_nodesList[i]);
            }
            return set;
        }

        public override System.Collections.Generic.HashSet<GraphNode<TNodeType>> GetExtendNodes(int node)
        {
            return GetExtendNodes(this[node]);
        }
    }
    
}