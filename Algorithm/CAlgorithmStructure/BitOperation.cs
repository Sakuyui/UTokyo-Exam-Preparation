using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TokyoU;

namespace Algorithm
{
    public class BitOperation
    {
        public int CountOneWithForce(int num)
        {
            var res = 0;
            for (var i = 0; i < 32; i++)
            {
                res += (num >> i) & 1;
            }

            return res;
        }

        public int CountOneWithLowBit(int num)
        {
            var t = num;
            var i = 0;
            for (; t != 0; i++, t &= t - 1);
            return i;
        }

        int[] oneCountTable = new int[1 << 4]
        {
            0, 1, 1, 2,
            1, 2, 2, 3,
            1, 2, 2, 3, 
            2, 3, 3, 4
        }; 
        public int CountOneLookTable(int num)
        {
            var res = 0;
            var t = num;
            const int mask = 0xF;
            while (t != 0)
            {
                res += oneCountTable[t & mask];
                t >>= 4;
            }
            return res;
        }


        public int CoinChange(int[] c, int k)
        {
            var dp = new int[c.Length + 1].Select(_ => int.MinValue).ToList(); //支付i元的最少枚数
            for (var i = 1; i <= c.Length; i++)
            {
                for (var j = 0; j + c[i] <= k; j++)
                {
                    dp[j] = Math.Min(dp[j - c[i]], dp[j] + 1);
                }

            }

            return dp[k];
        }


        public void CountSort(int[] nums, out int[] res)
        {
            if(nums.Min() < 0)
            {
                res = null;
                return;
            }

            var c = new int[nums.Max() + 1];
            for (var i = 0; i < nums.Length; i++)
            {
                c[nums[i]]++;
            }
            for (var i = 1; i <= c.Length; i++)
            {
                c[i] = c[i - 1] + c[i];
            }

            res = new int[nums.Length];
            for (var i = nums.Length - 1; i >= 0; i--)
            {
                res[c[nums[i]]--] = nums[i];
            }
        }



        public void OutPutHaCHIQueencePath(List<int> path)
        {
            path.PrintEnumerationToConsole();
        }
        public void HachiQueence()
        {
            //init
            var visited = new bool[8, 8];
            BackTractHachiQueence(ref visited);
        }

        public void BackTractHachiQueence( ref bool[,] vis,List<int> path = null, int d = 1)
        {
            if (d == 8)
            {
                OutPutHaCHIQueencePath(path);
                return;
                //到达叶子
            }
            if(d == 1)
                path = new List<int>();
            for (var i = 0; i < 8; i++)
            {
                if (!vis[i, d - 1] && CanPutQueence(vis, i, d - 1))
                {
                    vis[i, d - 1] = true;
                    path.Add(i);
                    BackTractHachiQueence(ref vis, path, d + 1);
                    path.RemoveAt(path.Count - 1);
                    vis[i, d - 1] = false;
                }
            }
        }

        public bool CanPutQueence(bool[,] vis, int r, int c)
        {
            //row,col
            for (var i = 0; i < 8; i++)
            {
                if (vis[r, i] && i != c)
                    return false;
                if (vis[i, c] && i != r)
                    return false;
            }

            for (var i = 1; r - i >= 0 && c - i >= 0; i++)
            {
                if (vis[r - i, c - i])
                    return false;
            }
            for (var i = 1; r + i < 8 && c + i < 8; i++)
            {
                if (vis[r + i, c + i])
                    return false;
            }

            return true;
        }
        
    }
    
    
   
    
}