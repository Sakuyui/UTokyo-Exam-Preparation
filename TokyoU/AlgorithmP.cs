using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using C5;
using TokyoU.Structure;

//实现堆，优先队列

namespace TokyoU
{
    public class AlgorithmP
    {
        public static void GetTopK<T>(T[] arr, int k) where T:IComparable
        {
            IntervalHeap<T> heap = new IntervalHeap<T>();
            foreach (var e in arr)
            {
                if (heap.Count < k)
                {
                    heap.Add(e);
                }
                else
                {
                    if (e.CompareTo(heap.FindMin()) > 0)
                    {
                        heap.DeleteMin();
                        heap.Add(e);
                    }
                }
            }

            //Linq排序~!!!!
            var list = (from c in heap orderby c descending select c).ToList();
            Console.WriteLine(Utils.ListToString(list));
            
        }

        

   
        
        //背包问题的三种解法

        public static int BagDynamicProgramming (int[] w, int[] v, int capacity)
        {
            //dp[i][w]表示前i个物品在w的情况下能够获得的最大价值
            var dp = new int[w.Length+ 1, capacity + 1];
            //dp[i,j] = max{dp[i-1, j], dp[j, j - w[i]] + v[i]}
            var ans = Int32.MinValue;
            for (var i = 1; i <= w.Length; i++)
            {
                for (var j = 1; j <= capacity; j++){
                    dp[i, j] = ( j - w[i] < 0 ? dp[i - 1, j] : 
                        System.Math.Max(dp[i - 1, j], dp[i, j - w[i]] + v[i]));
                    ans = (dp[i, j] > ans ? dp[i, j] : ans);
                }
            }

            return ans;
        }

        public static int BagReturn(int[] w, int[] v, int capacity, bool[] vis, int curMax)
        {
            //回溯法第一步：判断是否到叶子节点
            var max = curMax;
            var flag = w.Where((t, i) => !vis[i] && t < capacity).Any();
            if (!flag) return max; //到达叶子
            
            //遍历未遍历的子树
            for (int i = 0; i < w.Length; i++)
            {
                if (vis[i] || w[i] > capacity) continue;
                //找到一个未遍历的子节点
                vis[i] = true; //标记
                max =  System.Math.Max(BagReturn(w, v, capacity - w[i], vis, max), max);
                vis[i] = false; //清除标记
            }

            return max;
        }
        
        public static int BagBranchLimit(int[] w, int[] v, int capacity, int curMax)
        {
            int[] indexes = new int[w.Length];
            
            for (int i = 0; i < w.Length; i++)
            {
                //对于每个物品有选择或者不选择
                
            }

            return 0;
        }
    }
}