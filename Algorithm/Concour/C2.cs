using System;
using System.Collections.Generic;
using System.Linq;
using C5;

namespace Algorithm.Concour
{
    public class C2
    {
        //子集背包
        /*
         * 寻找和为k的组合可重复
         */
        //DFS方式 O(2^n)
        public static void SumKSelectDFS(int[] nums, int d = 0, int sum = 0,  List<int> res = null)
        {
            if (res == null)
                res = new List<int>();
            if (sum < 0 || d == nums.Length)
                return;
            //分支
            SumKSelectDFS(nums, d + 1, sum, res);
            SumKSelectDFS(nums, d + 1, sum - nums[d], res);
        }
        
        //子集部分背包 O(nKW)
        public static int SumKSelectBag(int[] nums, int sum)
        {
            var n = nums.Length;
            var dp = new int[n + 1, sum + 1];
            //init
            for (var i = 1; i <= n; i++)
            {
                dp[n, 0] = 1;
            }

            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= sum; j++)
                {
                    var max = dp[i - 1, j]; // 不装
                    for (var k = 1; j - k * nums[i] >= 0; i++)
                    {
                        max = Math.Max(max, dp[i, j - k * nums[i]]);
                    }

                    dp[i, j] = max;
                }
            }

            return dp[n, sum];
        }
        
        
        //Lake数（8连通区域个数）
        /*DFS*/
        public void LakeCountDFS(char[,] lake, int n, int m, int pi, int pj)
        {
            //visit 当前
            lake[pi, pj] = '.';
            
            //8联通遍历
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    int nx = pi + i;
                    int ny = pj + j;
                    if (nx >= 0 && nx < m && ny >= 0 && ny < n && lake[nx, ny] == 'W')
                    {
                        LakeCountDFS(lake, n , m, nx, ny);
                    } 
                }
            }
        }

        public int LakeCount(char[,] lake, int n, int m)
        {
            var res = 0;
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    if (lake[i, j] == 'W')
                    {
                        LakeCountDFS(lake, n, m, i, j);
                        res++;
                    }
                }
            }
            return res;
        }
        
        
        /*区间调度*/
        public int MaxParallelTasksCount(List<(int s, int e)> schedule)
        {
            if (schedule.Count == 0)
                return 0;
            var tmp = schedule.OrderBy(e => -e.e).ToList();
            var stack = new Stack<(int s, int e)>();
            stack.Push(tmp[0]);
            tmp.RemoveAt(0);
            while (tmp.Count > 0)
            {
                if(tmp[0].e > stack.Peek().s)
                    tmp.RemoveAt(0);
                else
                {
                    stack.Push(tmp[0]);
                    tmp.RemoveAt(0);
                }
            }
            return stack.Count;
        }
        
        /*
         * Fence Repair
         * 类似哈夫曼编码
         */
        public static int FenceRepairCost(int[] requires)
        {
            if (requires.Length < 2)
                return 0;
            var q = new IntervalHeap<int>();
            foreach (var e in requires)
            {
                q.Add(e);
            }
            
            while (q.Count > 1)
            {
                var m1 = q.DeleteMin();
                var m2 = q.DeleteMin();
                q.Add(m1 + m2);
            }

            return q.DeleteMin();
        }
        
        /*
         * 多重部分和
         */
        public bool IsSumK(int[] nums,int[] max, int sum)
        {
            var n = nums.Length;
            var dp = new int[sum + 1];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j <= sum; j++)
                {
                    if (dp[j] >= 0)
                    {
                        dp[j] = max[i];  //前面的数就能算出来
                    }else if (j < nums[i] || dp[j - nums[i]] <= 0)
                    {
                        dp[j] = -1;
                    }
                    else
                    {
                        dp[j] = dp[j - nums[i]] - 1;
                    }
                }
            }

            return dp[sum] >= 0;
        }
        
        /*
         *
         * 计数DP  
         */
        //1 把k个东西扔到n个容器里，有多少种方式。
        public int PartitionCount(int n, int k)
        {
            var dp = new int[n + 1, k + 1];
            dp[0, 0] = 1;
            for (var i = 0; i <= n; i++)
            {
                for (var j = 0; j <= k; j++)
                {
                    if (j - i >= 0)
                    {
                        dp[i, j] = dp[i - 1, j] //总数不变，多一个盘
                                   + dp[i, j - i]; //每个盘扔一个的情况
                    }
                    else
                    {
                        dp[i, j] = dp[i - 1, j];
                    }
                   

                }
            }

            return dp[n, k];
        }


        
    }
}