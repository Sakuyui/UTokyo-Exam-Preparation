using System.Collections.Generic;
using System.Linq;
using UTokyo.Structure;
using UTokyo.Structure.Graph;

namespace UTokyo.Praticle.TextBook
{
    public class GraphAlgorithm
    {
        public delegate int HeuristicCalc(int s, int t);
        public static (int[] previous, int cost) AStart(MatrixGraph<int> matrixGraph, int s, int t, HeuristicCalc h = null)
        {
            var n = matrixGraph.NodeCount;
            if (s == t || n == 0)
                return (new int[]{}, 0);

            var distance = new int[n].Select(e => int.MaxValue).ToArray();
            var previous = new int[n];
            previous[s] = s;
            distance[s] = 0 + (h?.Invoke(s,t) ?? 0);
            
            var openSet = new PriorityQueue<int, int>();
            var closeSet = new HashSet<int>();
            openSet.EnQueue(0, s);
            while (openSet.Any())
            {
                var fNode = openSet.DeQueue();
                closeSet.Add(fNode.item);
                foreach (var node in matrixGraph.GetExtendNodes(matrixGraph[fNode.item])
                    .Where(e => !closeSet.Contains(matrixGraph[e])))
                {
                    var x = matrixGraph[node];
                    var newDistance = distance[fNode.item] + (int) matrixGraph[fNode.item, x].Data;
                    if (newDistance < distance[x])
                    {
                        previous[x] = fNode.item;
                        distance[x] = newDistance;
                        openSet.UpdateOrSetPriority(x, distance[fNode.item] + (int) matrixGraph[fNode.item, x].Data);
                    }
                }
            }
            
            return (previous, distance[t]); //返回路径
        }


        public static void BellmanFordSPFA(MatrixGraph<int> matrixGraph, int s, int t, HeuristicCalc h)
        {
            var n = matrixGraph.NodeCount;
            var distance = new int[n].Select(e => int.MaxValue).ToArray();
            var previous = new int[n].Select(e => -1).ToList();
            distance[s] = 0;
            previous[s] = s;
            var queue = new Queue<int>();
            queue.Enqueue(s);
            while (queue.Any())
            {
                var node = queue.Dequeue();
                var ext = matrixGraph.GetExtendNodes(node).Select(e => matrixGraph[e]);
                foreach (var e in ext)
                {
                    var eFrom = node;
                    var eTo = e;
                    if (distance[eTo] > distance[eFrom] + (int) matrixGraph[eFrom, eTo].Data)
                    {
                        previous[eTo] = eFrom;
                        distance[eTo] = distance[eFrom] + (int) matrixGraph[eFrom, eTo].Data;
                        queue.Enqueue(e);
                    }
                }
            }

        }
        
        public static (int cost, bool isFound) IDAStart(MatrixGraph<int> matrixGraph, int s, int t,List<int> path, int g, int limit)
        {
            if (path.Count == 0)
                return (g, false);
            var lastNode = path.Last();
            var f = g + 0;
            if (f > limit)
            {
                return (f, false);
            }

            if (s == t)
            {
                return (f, true);
            }

            var min = int.MaxValue;
            foreach (var node in matrixGraph.GetExtendNodes(matrixGraph[lastNode])
                .Where(e => path.BinarySearch(matrixGraph[e]) < 0))
            {
                var x = matrixGraph[node];
                path.Add(x);
                var newDistance = g + (int) matrixGraph[lastNode, x].Data;
                var result = IDAStart(matrixGraph, x, t, path, newDistance, limit);
                min = System.Math.Min(min, result.cost);
                path.Remove(path.Count - 1);
            }

            return (min, false);

        }
        
        
    }
}