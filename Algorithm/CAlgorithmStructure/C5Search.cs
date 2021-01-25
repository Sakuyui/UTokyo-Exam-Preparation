using System;
using System.Collections.Generic;

namespace Algorithm
{
    public class C5Search
    {

        public void QuickSortSinglePointer(int[] nums, int s, int e)
        {
            if(e >= s)
                return;
            var pt = s;
            var p = nums[s];
            for (var i = pt + 1; i <= e; i++)
            {
                if (nums[i] >= p) continue;
                var t = nums[i];
                nums[i] = nums[++pt];
                nums[pt] = t;
            }

            var tmp = nums[s];
            nums[s] = nums[pt];
            nums[pt] = tmp;
            QuickSortSinglePointer(nums, s, pt - 1);
            QuickSortSinglePointer(nums, pt + 1, e);
        }
        /*
         * BinarySearch
        */
        public int BinarySearchOneElement<T>(T[] elements, T t) where T : IComparable
        {
            var l = 0;
            var r = elements.Length - 1;
            while (l <= r)
            {
                var mid = (l + r) >> 1;
                if (elements[mid].Equals(t))
                    return mid;
                if (elements[mid].CompareTo(t) < 0)
                    l = mid + 1;
                else
                    r = mid - 1;
            }
            return -1;
        }

        public int BinarySearchLeftEdge<T>(T[] elements, T t) where T : IComparable
        {
            var l = 0;
            var r = elements.Length;
            while (l < r)
            {
                var mid = (l + r) >> 1;
                if (elements[mid].Equals(t))
                    r = mid;
                else if (elements[mid].CompareTo(t) < 0)
                    l = mid + 1;
                else
                    r = mid;
            }
            return l;
        }

        public int BinarySearchRightEdge<T>(T[] elements, T t) where T : IComparable
        {
            var l = 0;
            var r = elements.Length;
            while (l < r)
            {
                var mid = (l + r) >> 1;
                if (elements[mid].Equals(t))
                    l = mid + 1;
                else if (elements[mid].CompareTo(t) < 0)
                    l = mid + 1;
                else
                    r = mid;
            }
            return l - 1;
        }
        
        /*
         * 和为K的子集
         */
        public bool SubsetSumKSubSetBagSolve(int[] nums, int k)
        {
            var n = nums.Length;
            var dp = new bool[n + 1, k + 1];
            for (var i = 0; i <= k; i++)
            {
                dp[i, 0] = true;
            }

            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    if (j - nums[i - 1] < 0)
                    {
                        dp[i, j] = dp[i - 1, j];
                    }
                    else
                    {
                        dp[i, j] = dp[i - 1, j] || dp[i, j - nums[i - 1]];
                    }
                }
            }
            return dp[n, k];
        }


        public void MergeSort(int[] nums, int start, int end)
        {
            if(end >= start)
                return;
            var m = (start + end) >> 1;
            MergeSort(nums, start, m); //[start, m]
            MergeSort(nums, m + 1, end); //[m + 1, end]
            Merge(nums,start, m, end);
        }

        public void Merge(int[] nums, int s, int m, int e)
        {
            var list = new List<int>();
            var lp1 = s; //第一部分数组
            var lp2 = m + 1; //第二部分数组
            while (lp1 <= m && lp2 <= e)
                list.Add(nums[lp1] < nums[lp2] ? nums[lp1++] : nums[lp2++]);
            while(lp1 <= m)
                list.Add(nums[lp1++]);
            while(lp2 <= m)
                list.Add(nums[lp2++]);
            //copy
            for (var i = 0; i < list.Count; i++)
            {
                nums[s + i] = list[i];
            }
        }
        
    }
}