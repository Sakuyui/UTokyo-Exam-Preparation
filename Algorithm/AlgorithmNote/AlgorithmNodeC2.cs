using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.AlgorithmNote
{
    public class AlgorithmNodeC2
    {
        /*
         * Lis
         */
        public int Lis(int[] nums)
        {
            var n = nums.Length;
            var dp = new int[n].Select(e => 1).ToList();
            for (var i = 0; i <= n; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (nums[i] > nums[j])
                        dp[i] = Math.Max(dp[i], dp[j] + i);
                }
            }

            return dp.Max();
        }

        /*
         * LIS - BinarySearch
         */
        public int LisBinarySearch(int[] nums)
        {
            var s = new int[nums.Length];
            var piles = 0;
            for (var i = 0; i < nums.Length; i++)
            {
                var cur = nums[i];

                //binary search
                var l = 0;
                var r = piles;
                while (l < r)
                {
                    var mid = (l + r) / 2;
                    if (s[mid] >= cur)
                    {
                        r = mid; // [l, mid)
                    }
                    else if (s[mid] < cur)
                    {
                        l = mid + 1;
                    }
                }

                //binary search end
                if (l == piles)
                    s[piles++] = cur;
            }

            return piles;
        }

        /*
         * MaxSubArray
         */
        public int MaxSubArray(int[] nums)
        {
            var n = nums.Length;
            if (n <= 0) return 0;
            var dp0 = nums[0];
            var dp1 = 0;
            var res = dp0;
            for (var i = 1; i < n; i++)
            {
                dp1 = Math.Max(nums[i], dp0 + nums[i]);
                dp0 = dp1;
                res = Math.Max(res, dp1);
            }

            return res;
        }

        /*
         * 最长回文
         */
        public int LongestPalindromeSubsequence(string s)
        {
            var n = s.Length;
            var dp = new int[n, n];
            for (var i = 0; i < n; i++)
            {
                dp[i, i] = 1;
            }

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


        /*
         * 插入最小次数构造回文串
         */
        public int MinInsertToPalindrome(string s)
        {
            var n = s.Length;
            var dp = new int[n, n]; // dp[i,j] i~j构造回文串需要插入多少
            /*转移若 si == sj,就是dp[i + 1,j - 1]惹。否则dp[i + 1,j] + 1, dp[i, j - 1] + 1最小的*/
            for (var i = n - 2; i >= 0; i--)
            {
                for (var j = i + 1; j < n; j++)
                {
                    if (s[i] == s[j])
                    {
                        dp[i, j] = dp[i + 1, j - 1];
                    }
                    else
                    {
                        dp[i, j] = Math.Min(dp[i + 1, j], dp[i, j - 1]) + 1;
                    }
                }
            }

            return dp[0, n - 1];
        }

        public int MaxACounts(int operationChances)
        {
            /*
             * 选择 - 这次按A、这次按C-V
             */
            var dp = new int[operationChances + 1];
            dp[0] = 0;
            for (var i = 1; i <= operationChances; i++)
            {
                dp[i] = dp[i - 1] + 1; //按A
                /*连续粘贴的情况*/
                for (var j = 2; j < i; j++)
                {
                    dp[i] = Math.Max(dp[i], dp[i - 2] * (i - j + 1));
                }
            }

            return dp[operationChances];
        }

        /*
         * 回溯，每个加正负号，运算得到target
         */
        public int FindTargetSumWaysBacktrack(int[] nums, int target, List<List<int>> path, int d = 0)
        {
            if (nums.Length == 0)
                return 0;
            var rest = target;
            if (d == nums.Length)
            {
                if (rest == 0)
                    path.Add(new List<int>());
                return 1;
            }

            rest += nums[d];
            var p = FindTargetSumWaysBacktrack(nums, rest, path, d + 1);
            rest -= nums[d];
            rest -= nums[d];
            var m = FindTargetSumWaysBacktrack(nums, rest, path, d + 1);
            rest += nums[d];
            return p + m;
        }

        /*
         * 和为K的子集/子集背包
         */
        public int FindTargetSumWaysDP(IEnumerable<int> nums, int target)
        {
            //!!!!!!!!
            var array = nums.SelectMany(e => new int[2] {e, -e}).ToList();
            var n = array.Count;
            var dp = new int[n + 1, target + 1];
            //base case
            for (var i = 0; i <= n; i++)
            {
                dp[i, 0] = 1;
            }

            for (var i = 1; i <= n; i++)
            {
                for (var j = 0; j <= target; j++)
                {
                    if (j >= array[i - 1])
                        dp[i, j] = dp[i - 1, j] + dp[i - 1, j - array[i - 1]];
                    else
                        dp[i, j] = dp[i - 1, j];
                }
            }

            return dp[n, target];
        }

        /*
         * 和为K的子数组
         * */
        public int SubArraySumK(int[] nums, int k)
        {
            /*
             * 用pre[i]表示从0到i序列的和,
             * [j,i]序列的和为pre[i]-pre[j]，
             * 题目意思即是要找到pre[i]-pre[j]=k的序列，
             * 即pre[j]=pre[i]-k的序列，可以从0到n-1遍历数组，
             * 将sum[i]保存在map中，形式为<sum[i]，次数>,表示从0到i的序列中，
             * 和为sum[i]的序列出现过多少次，对于sum[i]，如果前面出现过pre[i]-k，
             * 则count+=次数
             */
            var n = nums.Length;
            var dict = new Dictionary<int, int>(); // sum[i],次数
            dict[0] = 1;
            var count = 0;
            var sum = 0;
            for (var i = 0; i < n; i++)
            {
                sum += nums[i];
                var pre = sum - k;
                if (dict.ContainsKey(pre))
                    count += dict[pre];
                if (dict.ContainsKey(sum))
                    dict[sum]++;
                else
                    dict[sum] = 1;
            }

            return count;
        }


        public void PancakeSort(int[] cakes, int n, List<int> res)
        {
            //寻找最大的饼
            if (n <= 1)
                return;
            var maxCake = 0;
            var maxCakeIndex = 0;
            for (var i = 0; i < n; i++)
            {
                if (cakes[i] > maxCake)
                {
                    maxCake = cakes[i];
                    maxCakeIndex = i;
                }
            }

            //把最大的转上来
            Reverse(cakes, 0, maxCakeIndex);
            res.Add(maxCakeIndex + 1); //记录答案
            Reverse(cakes, 0, n - 1);
            res.Add(n);
            PancakeSort(cakes, n - 1, res);
        }

        public void Reverse(int[] arr, int i, int j)
        {
            while (i < j)
            {
                var t = arr[i];
                arr[i] = arr[j];
                arr[j] = t;
                i++;
                j--;
            }
        }

        /*
         * 回溯 - 括号生成
         */
        public int BacktrackGenerateParenthesis(int pairCounts, List<string> path = null, int lc = 0, int rc = 0)
        {
            if (lc + rc >= pairCounts * 2)
                return (lc == rc) ? 1 : 0;
            if (path == null)
                path = new List<string>();
            if (lc == rc)
            {
                path.Add("(");
                var a1 = BacktrackGenerateParenthesis(pairCounts, path, lc + 1, rc);
                return a1;
            }
            else if (lc > rc)
            {
                //剪枝
                if (lc - rc > pairCounts - lc - rc)
                    return 0;
                path.Add("(");
                var a2 = BacktrackGenerateParenthesis(pairCounts, path, lc + 1, rc);
                path.RemoveAt(path.Count - 1);
                path.Add(")");
                var a3 = BacktrackGenerateParenthesis(pairCounts, path, lc, rc + 1);
                return a2 + a3;
            }

            return 0;
        }

        public int LeftRangeBinarySearch(int[] nums, int target)
        {
            var l = 0;
            var r = nums.Length;
            while (l < r)
            {
                var mid = (l + r) / 2;
                if (nums[mid] == target)
                {
                    r = mid; //[left, mid)
                }
                else if (nums[mid] < target)
                {
                    l = mid + 1;
                }
                else if (nums[mid] > target)
                {
                    r = mid;
                }
            }

            return l;
        }

        public int RightRangeBinarySearch(int[] nums, int target)
        {
            var l = 0;
            var r = nums.Length;
            while (l < r)
            {
                var mid = (l + r) / 2;
                if (nums[mid] <= target)
                {
                    l = mid + 1;
                }
                else
                {
                    r = mid;
                }

            }

            return l - 1;
        }


        /*
         * 扔鸡蛋，记忆化搜索法
         * 首先函数本身循环复杂的是O(N)
         * 子问题个数是不同状态的组合数也就是 O(KN)
         * 总体就是 O(KN^2)
         */
        public int SupperEggDrop(int k, int n, Dictionary<(int,int),int> memo = null)
        {
            if(memo == null)
                memo = new Dictionary<(int,int), int>();
            if (k == 1)
                return n; //只剩下一个鸡蛋只能线性扫描
            if (n == 0)
                return 0;
            //记忆化搜索
            if (memo.ContainsKey((k, n)))
                return memo[(k, n)];
            var res = int.MaxValue;
            
            //列举所有可能性
            for (var i = 1; i <= n; i++)
            {
                //在i层扔
                res = Math.Min(
                    res,
                    //碎和没碎下的最坏情况
                    Math.Max(
                        SupperEggDrop(k - 1, i - 1, memo), //碎了
                        SupperEggDrop( k,n - i, memo) //没碎,楼层数更新，鸡蛋个数不变
                    )
                );
            }

            memo[(k, n)] = res;
            return res;
        }
        
        /*
         * 高楼扔鸡蛋，二分搜索法。
         * 因为dp(k - 1, i - 1) dp(k, n - i)中 i是递增的。看作是i的函数。其实最低点就是求两个函数的交点
         */
        public int SupperEggDropBinarySearch(int k, int n, Dictionary<(int, int), int> memo = null)
        {
            if (memo == null)
                memo = new Dictionary<(int, int), int>();
            if (k == 1)
                return n;
            if (n == 0)
                return 0;
            if (memo.ContainsKey((k, n)))
                return memo[(k, n)];
            var res = int.MaxValue;
            //二分搜索代替 [l,r]
            var l = 1;
            var r = n;
            while (l <= r)
            {
                var mid = (l + r) / 2;
                var broken = SupperEggDropBinarySearch(k - 1, mid - 1);
                var nonBroken = SupperEggDropBinarySearch(k, n - mid); //没碎，在上面楼层数抛
                if (broken > nonBroken)
                {
                    r = mid - 1;
                    res = Math.Min(res, broken + 1);
                }
                else
                {
                    l = mid + 1;
                    res = Math.Min(res, nonBroken + 1);
                }
            }

            memo[(k, n)] = res;
            return res;
        }
        
        /*
         * 高楼扔鸡蛋，状态转移优化
         * dp[k,m] 给k个鸡蛋。测试m次最坏情况下能测试多少层楼
         * dp[k,m] = dp[k - 1, m - 1] + dp[k, m - 1] + 1 楼下+楼上的层数+1
         * O(KN)
         */
        public int SupperEggDrop3(int k, int n)
        {
            var dp = new int[k + 1, n + 1];
            var m = 0;
            while (dp[k, m] < n) //终止条件是能够测试n楼
            {
                m++;
                for (var i = 1; i <= k; i++)
                {
                    dp[i, m] = dp[i, m - 1] + dp[i - 1, m - 1] + 1;
                }
            }
            return m;
        }
        
        
        /*搓气球，DP：
         * 每个气球nums[i]分。。搓破时可以获得nums[left] * nums[i] * nums[right]的分数
         * 怎样搓破能分数最大
         * 全排列惹
         */
        public int BrokeSphereDP(int[] nums)
        {
            var n = nums.Length;
            var points = new int[n + 2];
            points[0] = 1;
            points[n + 1] = 1;
            for (var i = 1; i <= n; i++)
            {
                points[i] = nums[i - 1];
            }

            //搓破i ~ j之间的气球最大分数
            var dp = new int[n + 2, n + 2];
            //最后搓破的气球是哪个？
            //dp[i,j] =max(res, dp[i,k] + dp[k,j] + nums[k] * nums[i] * nums[j]);
            //下到上，左到右便利
            for (var i = n; i >= 0; i--)
            {
                for (var j = i + 1; j < n + 2; j++)
                {
                    //遍历所有气球
                    for (var k = i + 1; k < j; k++)
                    {
                        dp[i, j] = Math.Max(
                            dp[i, j],
                            dp[i, k] + dp[k,j] + nums[k] * nums[i] * nums[j] 
                        );
                    }
                }
            }

            return dp[0, n + 1];
        }
        
        /*
         * 子集背包。能否划分成两个集合，和相等。
         */
        public bool CanPartition(int[] nums)
        {
            var n = nums.Length;
            var sum = nums.Sum();
            if ((sum & 1) == 1) //和为奇数不可能
                return false;
            sum /= 2;
            
            
            var dp = new bool[n + 1, sum + 1];
            //base
            for (var i = 0; i <= n; i++)
            {
                dp[i, 0] = true;
            }

            for (var i = 0; i <= n; i++)
            {
                for (var j = 0; j <= sum; j++)
                {
                    //放不下惹
                    if (j - nums[i - 1] < 0)
                        dp[i, j] = dp[i - 1, j];
                    else
                    //能放下那就尝试两种情况
                        dp[i, j] = dp[i - 1, j] || dp[i - 1, j - nums[i - 1]];
                }
            }
            return dp[n, sum];
        }
        
        /*
         * 子集背包2 - 有多少种操作为nums的数加上正负号和为k
         */
        public int SubSetSumKCount(int[] nums, int k)
        {
            var n = nums.Length;
            var dp = new int[n + 1, k + 1];
            for (var i = 0; i <= n; i++)
            {
                dp[i, 0] = 1;
            }

            for (var i = 0; i <= n; i++)
            {
                for (var j = 0; j <= k; j++)
                {
                    if (j >= nums[i - 1])
                        dp[i, j] = dp[i - 1, j] + dp[i - 1, j - nums[i - 1]];
                    else
                        dp[i, j] = dp[i - 1, j];
                }
            }
            
            return dp[n, k];
        }
        
        /*
         * 完全背包/零钱兑换
         * 给定不同面额的硬币，无限。多少种方法能够凑出总金额
         */
        public int CoinChange(int amount, int[] coins)
        {
            var n = coins.Length;
            var dp = new int[n + 1,amount + 1];
            for (var i = 0; i <= n; i++)
            {
                dp[i, 0] = 1;
            }
            for (var i = 0; i <= n; i++)
            {
                for (var j = 0; j <= amount; j++)
                {
                    if (j - coins[i] >= 0)
                        dp[i, j] = dp[i - 1, j] + dp[i, j - coins[i]];
                    else
                        dp[i, j] = dp[i - 1, j];
                }
            }

            return dp[n, amount];
        }
        
        
        
    }
} 