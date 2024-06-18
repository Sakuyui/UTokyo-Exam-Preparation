using System.Collections.Generic;

namespace Algorithm
{
    public class C8Structure
    {
        public void GraphDfs(int[,] matrix, int n)
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(0);
            var visited = new bool[n];
            while (stack.Count != 0)
            {
                var p = stack.Peek();
                visited[p] = true;
                //access
                var l = ExtendGraphNode(matrix, n, p);
                for (var i = 0; i < l.Count; i++)
                {
                    if (!visited[i])
                    {
                        stack.Push(i);
                        continue;
                    }
                }

                stack.Pop();
            }
        }

        public List<int> ExtendGraphNode(int[,] matrix, int n, int nodeIndex)
        {
            var res = new List<int>();
            for (var i = 0; i < n; i++)
            {
                if (matrix[nodeIndex, i] != int.MaxValue && i != nodeIndex)
                {
                    res.Add(i);
                }
            }

            return res;
        }
    }
    
    

    
}