using System;
using System.Collections.Generic;
using System.Linq;
using TokyoU;

namespace Algorithm.BeautyProgmming
{
    public class C1
    {

        public void Swap<T>(T[] values, int i, int j)
        {
            var t = values[i];
            values[i] = values[j];
            values[j] = t;
        }
        public void Swap<T>(List<T> values, int i, int j)
        {
            var t = values[i];
            values[i] = values[j];
            values[j] = t;
        }
        /*
         * 翻烙饼
         */
        public void ReversePan(int[] pans, int bottom)
        {
            var t = 0;
            while (t <= bottom)
            {
                Swap(pans, t++, bottom--);
            }
        }
        public List<int> ReversePan2(List<int> pans, int bottom)
        {
            var list = new List<int>(pans);
            var t = 0;
            while (t <= bottom)
            {
                Swap(list, t++, bottom--);
            }

            return list;
        }
        public bool IsSorted(List<int> nums)
        {
            if(nums == null || nums.Count == 0)
                return false;
            var t = nums[0];
            foreach (var e in nums)
            {
                if (e < t)
                    return false;
                t = e;
            }

            return true;
        }

        //最多 2 (n - 1)次翻转
        public void ReversePanCount(int[] pans)
        {
            while (true)
            {
                var maxI = pans.MaxBy(e => e);
                ReversePan(pans, maxI);
                ReversePan(pans, pans.Length - 1);
                if (!IsSorted(pans.ToList())) continue;
                break;
            }
        }
        
        /*
         * 搜索翻转烙饼的最小次数
         */
        public C5.HashSet<List<int>> VisitedPanState = new C5.HashSet<List<int>>();
        public int ReversePanSearchMinBFS(List<int> pans)
        {
            var limit = 2 * (pans.Count - 1);
            if(IsSorted(pans))
                return 0;
            var states = new Queue<List<int>>();
            states.Enqueue(pans);
            var d = 0;
            while (states.Count != 0)
            {
                //某层
                for(var i = 0; i < states.Count; i++)
                {
                    var s = states.Dequeue();
                    for (var k = 0; k < pans.Count; k++)
                    {
                        var tmp = ReversePan2(pans, k);
                        if (VisitedPanState.Contains(tmp)) continue;
                        if (IsSorted(tmp))
                            return d + 1;
                        states.Enqueue(s);
                        VisitedPanState.Add(s);
                    }
                }

                d++;
                if(d >= limit)
                    break;
            }
            return limit;
        }
        
        public int ReversePanSearchMinDFS(List<int> pans, int depth = 0)
        {
            
            var limit = 2 * (pans.Count - 1);
            if (depth >= limit)
                return limit;
            if(IsSorted(pans))
                return depth;

            //某层
            var min = int.MaxValue;
            for (var k = 0; k < pans.Count; k++)
            { 
                var tmp = ReversePan2(pans, k);
                if (VisitedPanState.Contains(tmp)) continue;
                min = Math.Min(min, ReversePanSearchMinDFS(pans, depth + 1));
            }
            
            return min;

        }
        
        
        /*
         * 被翻转的饼的数量最小
         */
        public int ReversePansSearchBFSWithLeast(List<int> pans)
        {
            var limit = 0;
            if (IsSorted(pans))
                return 0;
            //使用正常方法时的代价（作为初始上界）
            while (true)
            {
                if(IsSorted(pans))
                    break;
                var mIndex = pans.MaxBy(e => e);
                pans = ReversePan2(pans, mIndex);
                limit += mIndex + pans.Count;
            }
            
            //bfs模板惹
            var queue = new Queue<(int, List<int>)>();
            queue.Enqueue((0,pans));
            var min = limit;
            
            while (queue.Count > 0)
            {
                var c = queue.Count;
               
                for (var i = 0; i < c; i++)
                {
                    var tmp = queue.Dequeue();
                   
                    //遍历子节点
                    for (var k = 1; k < pans.Count; k++)
                    {
                        var cost = tmp.Item1 + k + 1 + pans.Count;
                        if(cost > min)
                            continue;
                        var newPans = ReversePan2(tmp.Item2, k);
                        if(VisitedPanState.Contains(newPans)) //已经经过这个状态惹
                            continue;
                        if (IsSorted(newPans))
                            min = Math.Min(min, cost);
                        queue.Enqueue((cost, newPans));
                        VisitedPanState.Add(newPans); //记忆化
                    }
                    //层结束
                }
            }
         

            return min;
        }
        
              
        /*
         * 买书最大折扣
         * 2本：5%, 3本10%,4本 15%, 5本20%
         * DP: 
         */
        public double MaxDeCostBuyBooks(int y1, int y2, int y3, int y4, int y5)
        {
            const int singlePrice = 8;
            var dp = new double[y1, y2, y3, y4, y5];
            for (var i = 1; i <= y1; i++)
            {
                for (var j = 1; j <= y2; j++)
                {
                    for (var k = 1; k <= y3; k++)
                    {
                        for (var w = 1; w <= y4; w++)
                        {
                            for (var v = 1; v <= y5; v++)
                            {
                                dp[i, j, k, w, v] = Math.Min(
                                    5 * singlePrice * 0.75 + dp[i - 1, j - 1, k - 1, w - 1, v - 1],
                                    Math.Min(
                                        4 * singlePrice * 0.8 + dp[i - 1, j - 1, k - 1, w - 1, v],
                                        Math.Min(
                                            3 * singlePrice * 0.9 + dp[i - 1, j - 1, k - 1, w, v],
                                            2 * singlePrice * 0.95 + dp[i - 1, j - 1, k, w, v]
                                        )
                                    ));
                                dp[i, j, k, w, v] = Math.Min(dp[i, j, k, w, v], 8 + dp[i - 1, j, k, w, v]);
                            }
                        }
                    }
                }
            }

            return dp[y1, y2, y3, y4, y5];
        }


        public void MatrixQuickMultiply(List<List<int>> mat1, List<List<int>> mat2)
        {
            var m = mat1.Count;
            var c = mat2.Count;
            var n = mat2[0].Count;
            var blockSize = 1 << 4;
            var result = new int[m, n];
            for (var i = 0; i < m; i += blockSize)
            {
                for (var j = 0; j < c; j += blockSize)
                {
                    for (var k = 0; k < n; k += blockSize)
                    {
                        
                        for (var u = i; u < i + blockSize; u+=2)
                        {
                            for (var v = j; v < j + blockSize; v+=2)
                            {
                                for (var w = k; w < k + blockSize; w++)
                                {
                                    //loop unrolling
                                    result[u, v] += mat1[u][v] * mat2[v][w];
                                    result[u + 1, v] += mat1[u + 1][v] * mat2[v][w];
                                    result[u, v + 1] += mat1[u][v] * mat2[v][w + 1];
                                    result[u + 1, v + 1] += mat1[u + 1][v] * mat2[v][w + 1];
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /*
         * 找出故障机器
         */
        //寻找只出现一次的数
        public int FindUnavailableMachineOne(int[] nums)
        {
            var r = nums[0];
            for (var i = 1; i < nums.Length; i++)
            {
                r ^= nums[i];
            }
            return r;
        }
        
        //寻找缺失的不同两个数
        public (int, int) FinUnavailableMachineTwo(int[] currentIDs, int[] RightIDs)
        {
            var e1 = RightIDs.Sum();
            var e2 = RightIDs[0];
            for (var i = 1; i < RightIDs.Length; i++)
            {
                e2 *= RightIDs[i];
            }

            var (d1, d2) = ((e1 + Math.Sqrt(e1 * e1 - 4 * e2)) / 2, (e1 - Math.Sqrt(e1 * e1 - 4 * e2)) / 2);
            return ((int, int)) (d1, d2);
        }
        
        
        /*
         * 饮料最大满意度 DP
         */
        public int MaxSatisfyDrink(List<(int vi, int ci, int hi, int bi)> statisticData)
        {
            //Vi 容量 ci可能的最大数量 hi满意度 bi实际购买量
            //类似部分背包问题
            //dp[v',i] 表示容量为v'时，前i个物品最大满意度
            var totalC = statisticData.Sum(e => e.bi * e.vi);
            var dp = new int[statisticData.Count + 1, totalC + 1];
            for (var i = 0; i <= statisticData.Count; i++)
            {
                
                for (var j = 1; j <= totalC; j++)
                {
                    var m = int.MinValue;
                    for (var k = 0; k <= statisticData[i].ci; k++)
                    {
                        if( j < k * statisticData[i].vi)
                            break;
                        var x = dp[j - k * statisticData[i].vi, i];
                        if (x != 0)
                            x += statisticData[i].hi * k;
                        m = Math.Max(m, x);
                    }
                }
            }

            return dp[statisticData.Count, totalC];
        }
        
        
        
        /*
         * 二分法求逆序数
         */
        public static int ReverseNumCount(int[] nums, int s, int e)
        {
            if (e >= s)
                return 0;
            var mid = (s + e) >> 1;
            var l = ReverseNumCount(nums, s, mid);
            var r = ReverseNumCount(nums, mid + 1, e);
            var m = MergeForReverseCount(nums, s, mid, e);
            return l + r + m;
        }
        
        
        private static int MergeForReverseCount(IList<int> nums, int s, int m, int e)
        {
            var lpt = s;
            var rpt = m + 1;
            var tmp = new List<int>();
            var c = 0;
            while (lpt <= m && rpt <= e)
            {
                if (nums[lpt] < nums[rpt])
                {
                    tmp.Add(nums[lpt++]);
                }
                else
                {
                    c += e - rpt + 1;
                    tmp.Add(nums[rpt ++]);
                }
            }
            while(lpt <= m)
                tmp.Add(nums[lpt ++]);
            while(rpt <= e)
                tmp.Add(nums[rpt++]);
            for (var i = s; i <= e; i++)
            {
                nums[i] = tmp[i - e];
            }
            return c;
        }




    }
}