using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Algorithm.AlgorithmNote
{
    public class AlgorithmNoteC4
    {
        /*
         * 排列
         */
        public List<List<T>> GetPermutation<T>(List<T> data, int k, 
            List<List<T>> paths = null, List<T> path = null, bool[] visited = null)
        {
            if (path == null)
                path = new List<T>();
            if (paths == null) 
                paths = new List<List<T>>();
            if (visited == null)
                visited = new bool[data.Count];
            if(path.Count == k)
                paths.Add(path);
            for (var i = 0; i < data.Count && !visited[i]; i++)
            {
                visited[i] = true;
                GetPermutation<T>(data, k, paths, path, visited);
                visited[i] = false;
            }
            return paths;
        }
        
        
        /*
         * 组合
         */
        public List<List<T>> GetCombinations<T>(List<T> data,int k, List<List<T>> paths = null, List<T> path = null, int from = 0)
        {
            if (path == null)
                path = new List<T>();
            if (paths == null)
                paths = new List<List<T>>();
            if(path.Count == k)
                paths.Add(path);
            for (var i = from; i < data.Count; i++)
            {
                path.Add(data[i]);
                GetCombinations<T>(data, k, paths, path, from + 1);
                path.RemoveAt(path.Count - 1);
            }

            return paths;
        }
        
        
        
        //子集
        public List<List<T>> GetAllSubset<T>(List<T> data,List<List<T>> paths = null, List<T> path = null, int d = 0)
        {
            if (data.Count == 0) return paths;
            if (paths == null)
            {
                paths = new List<List<T>>();
                path = new List<T>();
            }
            
            if(path == null) path = new List<T>();
            paths.Add(path);
            //未遍历节点
            for (var i = d; i < data.Count; i++)
            {
                path.Add(data[i]);
                GetAllSubset<T>(data, paths, path, d + 1);
                path.RemoveAt(path.Count - 1);
            }
            
            return paths;
        }
        
        /*
         * 二分法
         * 猴子吃香蕉。最小的每小时速度
         */
        public bool CanFinish(int[] piles, int speed, int h)
        {
            //TODO
            return true;
        }
        public int MinEatingSpeed(int[] piles, int h)
        {
            var l = 1;
            var r = piles.Max() + 1;
            while (l < r)
            {
                var mid = (l + r) / 2;
                if (CanFinish(piles, mid, h))
                    r = mid;
                else
                    l = mid + 1;
            }

            return l;
        }
        
        
        /*接雨水*/
        public int StorageRain(int[] height)
        {
            var stack = new Stack<(int,int)>();
            var sum = 0;
            for (var i = 0; i < height.Length; i++)
            {
                if(stack.Count == 0 && height[i] == 0)
                    continue;
                if (stack.Count == 0)
                {
                    stack.Push((i,height[i]));
                }
                else
                {
                    if (stack.Peek().Item2 >= height[i])
                    {
                        stack.Push((i, height[i]));
                    }
                    else
                    {
                        
                    }
                }
            }

            return sum;
        }
        
        
        
    }
}