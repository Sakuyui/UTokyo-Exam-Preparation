using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Algorithm
{
    public class Other
    {

        public int BinarySearch(int[] nums, int t)
        {
            var n = nums.Length;
            var left = 0;
            var right = n;
            while (left < right)
            {
                var mid = (left + right) / 2;
                if (nums[mid] == t)
                    return mid;
                if (nums[mid] < t)
                    left = mid + 1;
                else
                    right = mid;
            }
            return -1;
        }

        
        
        
        //往后冒
        public void BubleSort<T>(T[] nums) where T : IComparable
        {
            var n = nums.Length;
            for (var i = n - 2; i >= 0; i++)
            {
                var sorted = true;
                for (var j = 0; j <= i; j++)
                {
                    if (nums[j].CompareTo(nums[j + 1]) <= 0) continue;
                    var t = nums[j];
                    nums[j] = nums[j + 1];
                    nums[j + 1] = t;
                    sorted = false;
                }
                if(sorted)
                    break;
            }
        }

        public void BubleSort2<T>(T[] nums) where T : IComparable
        {
            var n = nums.Length;
            for (var i = 0; i < n - 1; i++)
            {
                var sorted = true;
                for (var j = 0; j < n - 1 - i; j++)
                {
                    if (nums[j].CompareTo(nums[j + 1]) <= 0) continue;
                    var t = nums[j];
                    nums[j] = nums[j + 1];
                    nums[j + 1] = t;
                    sorted = false;
                }
                if(sorted)
                    break;
            }
        }

        public void QuickSort<T>(T[] nums, int left, int right) where T : IComparable
        {
            var n = right - left + 1;
            if (n <= 1) return;
            var p = new Random().Next(0, n - 1);
            p = left + p;
            //pivot换到末尾
            var t = nums[right];
            nums[right] = nums[p];
            nums[p] = t;
            var l = left;
            var r = right - 1;
            T tmp;
            while (l < r)
            {
                if (nums[l].CompareTo(nums[right]) < 0)
                {
                    tmp = nums[l];
                    nums[l] = nums[r];
                    nums[r] = tmp;
                    r--;
                }
                else
                {
                    l++;
                }
            }

            if (nums[l].CompareTo(nums[right]) < 0)
                l++;

            tmp = nums[right];
            nums[right] = nums[l];
            nums[l] = tmp;
            
            QuickSort<T>(nums,0 , l - 1);
            QuickSort<T>(nums,l + 1,right);
            //l为第k顺序统计量
        }


        public void Swap<T>(T[] nums, int i, int j)
        {
            if(i >= nums.Length || j >= nums.Length || i < 0 || j < 0)
                return;
            var t = nums[i];
            nums[j] = nums[i];
            nums[i] = t;
        }
        
        public void QuickSortSinglePoint<T>(T[] nums, int left, int right) where T: IComparable
        {
            if(right <= left)
                return;
            var pivot = nums[left]; //定基准
            var sp = left;
            
            //j其实就是虚假的第二个指针,注意j要扫描完数组
            for (var j = left + 1; j <= right; j++)
            {
                if (nums[j].CompareTo(pivot) < 0)
                {
                    sp++;  //sp总是比pivot小。因此sp++再交换
                    Swap(nums,  sp,j);
                }
            }
            Swap(nums, sp, left);
            QuickSort(nums, left, sp - 1 );
            QuickSort(nums, sp + 1, right);
        }

        /*
         * LIS(n^2解法)
         */
        public int Lis1(int[] nums)
        {
            var dp = new int[nums.Length + 1];
            for (var i = 1; i <= nums.Length; i++)
            {
                var max = 0;
                for (var j = 0; j < i; j++)
                {
                    if (nums[i] > nums[j])
                    {
                        max = Math.Max(max, dp[j] + 1);
                    }
                }
                dp[i] = max;
            }

            return dp[nums.Length];
        }



        
        public int Lis2D(List<Tuple<int, int>> tuples)
        {
            if (tuples == null) return 0;
            tuples.Sort((e1, e2) =>
            {
                if (e1.Item1 != e2.Item1)
                    return e1.Item1 - e2.Item1;
                else
                    return e2.Item1 - e1.Item1;
            });
            var dp = new int[tuples.Count + 1];
            var s = new Tuple<int,int>[tuples.Count];
            var cur = 0;
            for (var i = 0; i < dp.Length; i++)
            {
                var l = 0;
                var r = cur;
                while (l < r)
                {
                    var mid = (l + r) / 2;
                    var cmp = 0;
                    cmp = s[mid].Item1 - tuples[i].Item1;
                    if (cmp == 0)
                        cmp = tuples[i].Item2 - s[mid].Item2;
                    if (cmp < 0)
                    {
                        l = mid + 1;
                    }
                    else
                    {
                        r = mid - 1;
                    }
                }

                if (l >= cur)
                {
                    s[cur++] = tuples[i];
                }
                else
                {
                    s[l] = tuples[i];
                }
            }

            return dp[tuples.Count];

        }
        
        
        /*
         * LIS(nlogn解法)
         */
        public int Lis2(int[] nums)
        {
            //大 -> 小
            var s = new int[nums.Length];
            var curLen = 0;
            //寻找左边界
          
            foreach (var t in nums)
            {
                var l = 0;
                var r = curLen;
                //[l, r]
                while (l < r)
                {
                    var mid = (l + r) / 2;
                    if (nums[mid] <= t)
                        r = mid + 1;
                    else
                        l = mid - 1;
                }

                if (r >= curLen)
                {
                    s[curLen++] = t;
                }
            }

            return curLen;
        }
        
        /*
         * 最大子数组和
         */
        public int MinSubArraySum(int[] nums)
        {
            var n = nums.Length;
            var dp = new int[n + 1];
            for (var i = 1; i <= n; i++)
            {
                //dp[i]以i结尾的最大子数组
                dp[i] = Math.Max(dp[i - 1] + nums[i], nums[i]);
            }
            return dp[n];
        }
        
        /*
         * 最长公共子序列 LCS
         */
        public int Lcs(String str)
        {
            var n = str.Length;
            var dp = new int[n + 1, n + 1];
            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    if (str[i - 1] == str[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }
            return dp[n, n];
        }
        
        /*
         * 编辑距离
         */
        public int EditDistance(string str)
        {
            var n = str.Length;
            var dp = new int[n + 1, n + 1];
            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    dp[i, j] = Math.Min(dp[i - 1, j - 1],
                        Math.Min(dp[i - 1, j], dp[i, j - 1])) + 1;
                }
            }
            return dp[n, n];
        }

        
        /*
         * 树状数组构建
         */
        public int[] TreeArrayBuild(int[] nums)
        {
            var n = nums.Length;
            var msb = n;
            var c = new int[n + 1];
            //获取最高位的1
            while (true)
            {
                if((msb & (msb - 1)) == 0)
                    break;
                msb &= (1 - msb);
            }

            for (var i = 1; i <= n; i++)
            {
                var t = 0;
                var lowBit = i & (-i);
                for (var j = 0; j < lowBit; j++)
                {
                    t += nums[i - j];
                }
                c[i] = t;
            }
            return c;
        }
        /*
         * 通过树状数组求前缀和
         */
        public int SuffixesSumWithTreeArray(int[] c, int k)
        {
            var x = k;
            var sum = 0;
            while (x != 0)
            {
                sum += c[x & (-x)];
                x &= (x - 1);
            }
            return sum;
        }
        
        /*
         * 位运算加法
         */
        public int Add(int a, int b)
        {
            if (a == 0)
                return b;
            if (b == 0)
                return a;
            //进位
            var c = a ^ b;
            var s = a & b;
            return Add(s, c << 1);
        }

        /*
         * TSP
         */
        public int Tsp(int[,] w, int n)
        {
            var dp = new int[(1 << n) + 1, n + 1];
            dp[1, 0] = 0; //00001遍历过
            //填列
            for (var s = 2; s < (1 << n); s++)
            {
                //填行
                for (var u = 0; u < n; u++)
                {
                    if(((s >> u) & 1) == 0)
                        continue; //未被遍历
                    for (var v = 0; v < n; v++)
                    {
                        //两个点都是必须遍历过的
                        if(((s >> v) & 1) ==0)
                            continue;
                        dp[s, v] = Math.Min(dp[s, v], dp[s - (1 << v), u] + w[u, v]); //经过u会不会更小?
                    }
                }
            }

            var ans = int.MaxValue;
            for (var i = 0; i < n; i++)
            {
                ans = Math.Min(ans, dp[(1 << n) - 1, i] + w[i, 0]);
            }
            return ans;
        }

        

    }
}