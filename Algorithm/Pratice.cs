using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using C5;

namespace Algorithm
{
    public class Pratice
    {
        public static int Fib(int n)
        {
            var mem = new int[n + 1];
            if (n == 0)
                return 0;
            var am2 = 1;
            var am1 = 1;
            var am = 1;
            for (var i = 3; i <= n; i++)
            {
                am = am2 + am1;
                am2 = am1;
                am1 = am;
            }
            return am;
        }

        public int CoinChange(int[] coins, int amount)
        {
            var n = coins.Length;
            var dp = new int[amount + 1].Select(e => int.MaxValue).ToList();
            dp[0] = 0;
            for (var i = 0; i <= n; i++)
            {
                for (var j = 0; j <= n; j++)
                {
                    if(i - coins[j] < 0)
                        continue;
                    dp[i] = Math.Min(dp[i], dp[i - coins[j] + 1]);
                }
            }

            return dp[amount] != int.MaxValue ? dp[amount] : -1;
        }


        public void AllPermutation(int[] nums, int c, List<List<int>> paths = null, List<int> curPath = null)
        {
            var n = nums.Length;
            if (c > n)
                return;
            if(paths == null)
                paths = new List<List<int>>();
            if(curPath == null)
                curPath = new List<int>();
            //是否到达叶子
            if(curPath.Count == c)
                paths.Add(curPath);
            foreach (var node in nums.Where(e => !curPath.Contains(e)))
            {
                curPath.Add(node);
                AllPermutation(nums, c, paths, curPath);
                curPath.RemoveAt(curPath.Count - 1);
            }
        }


        public void NQueenceBacktract(int n, int depth = 0, bool[,] board = null, List<List<int>> possiblePaths = null,
            List<int> curPath = null)
        {
            if (possiblePaths == null)
                possiblePaths = new List<List<int>>();
            if (curPath == null)
                curPath = new List<int>();
            if (board == null)
                board = new bool[n, n];
            if (depth == n - 1)
            {
                possiblePaths.Add(curPath);
                return;
            }

            //遍历行

            for (var i = 0; i < n; i++)
            {
                if (NQueenceCheck(board, n, i, depth))
                {
                    board[i, depth] = true;
                    curPath.Add(i);
                    NQueenceBacktract(n, depth + 1, board, possiblePaths, curPath);
                    board[i, depth] = false;
                    curPath.RemoveAt(curPath.Count - 1);
                }
            }
        }
    

        public bool NQueenceCheck(bool[,] board, int n, int r, int c)
        {
            //Check
            return true;
        }


        public static int BinarySearchLeftRange<T>(T[] values, T t, int s, int e) where T : IComparable
        {
            if (e > s)
                return e;
            var l = s;
            var r = e;
            while (l <= r)
            {
                var m = (l + r) >> 1;
                if (values[m].Equals(t) || values[m].CompareTo(t) > 0)
                {
                    r = m - 1;
                }else if (values[m].CompareTo(t) < 0)
                {
                    l = m + 1;
                }
            }
            return l;
        }

        public static int Lis(int[] nums)
        {
            var n = nums.Length;
            var s = new int[n];
            var curLen = 0;
            foreach (var e in nums)
            {
                var i = BinarySearchLeftRange(nums, e, 0, curLen);
                if (i == curLen)
                {
                    s[curLen++] = e;
                }
                else
                {
                    s[i] = e;
                }
            }

            return curLen;
        }


        public static int MergeSortForReversedPairs(int[] nums, int s, int e)
        {
            if (e >= s)
                return 0;
            var m = (s + e) >> 1;
            var lc = MergeSortForReversedPairs(nums, s, m);
            var rc = MergeSortForReversedPairs(nums, m + 1, e);
            var mc = MergeForReversedPairs(nums, s, m, e);
            return lc + rc + mc;
        }


        public static int MergeForReversedPairs(int[] nums, int s, int m, int e)
        {
            var list = new List<int>();
            var res = 0;
            var lp = s;
            var rp = m + 1;
            while (lp <= m && rp <= e)
            {
                if (nums[lp] < nums[rp])
                {
                    list.Add(nums[lp++]);
                }
                else
                {
                    list.Add(nums[rp]);
                    res += e - rp + 1;
                    rp++;
                }
            }
            while(lp <= m)
                list.Add(lp++);
            while(rp <= e)
                list.Add(rp++);

            return res;
        }


        public static int LongestPalindromeSubseq(String s)
        {
            var n = s.Length;
            var dp = new int[n, n];
            for (var i = 0; i < n; i++)
                dp[i, i] = 1;
            for (var i = n - 2; i >= 0; i--)
            {
                for (var j = i + 1; j < n; j++)
                {
                    if (s[i] == s[j])
                        dp[i, j] = dp[i + 1, j - 1] + 2;
                    else
                        dp[i, j] = Math.Max(dp[i + 1, j], dp[i, j - 1]);
                }
            }
            return dp[0, n - 1];
        }

        public static int MinInsertions(string s)
        {
            var n = s.Length;
            var dp = new int[n, n];

            for (var i = n - 2; i >= 0; i--)
            {
                for (var j = i + 1; j < n; j++)
                {
                    if (s[i] == s[j])
                        dp[i, j] = dp[i + 1, j - 1];
                    else
                        dp[i, j] = Math.Min(dp[i + 1, j], dp[i, j - 1]) + 1;
                }
            }

            return dp[0, n - 1];
        }


        public static int SupperEggDrop(int k, int n,  HashDictionary<(int, int), int> memo = null)
        {
            memo = memo ?? new HashDictionary<(int, int), int>(); 
            if (k == 1)
                return n; //只能线性扫描
            if (n == 0)
                return 0;
            if (memo.Contains((k, n)))
                return memo[(k, n)];

            var res = int.MaxValue;

            for (var i = 1; i <= n + 1; i++)
            {
                res = Math.Min(res,
                    Math.Max(
                        SupperEggDrop(k, n - i, memo), //没碎
                        SupperEggDrop(k - 1, i - 1) // 碎了
                    )
                );
            }

            memo[(k, n)] = res;
            return res;
        }
    }
}