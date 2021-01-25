using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Algorithm.Concour
{

    //回溯
    public class CollectionHelper
    {
        public static List<List<T>> GetPermutation<T>(List<T> list, int k)
        {
            var n = list.Count;
            if(k > n)
                return new List<List<T>>();
            var ans = new List<List<T>>();
            GetPermutationBackTrack(list, ans, new List<T>(), new bool[n].ToList(),0, k);
            return ans;
        }

        public static void GetPermutationBackTrack<T>(List<T> data, List<List<T>> ans,List<T> path, List<bool> visited, int d, int limit)
        {
            //leaf
            if (d == limit)
            {
                ans.Add(path);
                return;
            }

            for (var i = 0; i < data.Count && !visited[i]; i++)
            {
                path.Add(data[i]);
                visited[i] = true;
                GetPermutationBackTrack<T>(data, ans, path,visited, d + 1, limit);
                visited[i] = false;
                path.RemoveAt(path.Count - 1);
            }
            
        }
        public static List<List<T>> GetCombination<T>(List<T> list, int k)
        {
            var n = list.Count;
            if(k > n)
                return new List<List<T>>();
            var ans = new List<List<T>>();
            GetCombinationBackTract(list, ans, new List<T>(), 0, k);
            return ans;
        }

        public static void GetCombinationBackTract<T>(List<T> data, List<List<T>> ans,List<T> path, int d, int limit)
        {
            //leaf
            if (d == limit)
            {
                ans.Add(path);
                return;
            }
          
            for (var i = d; i < data.Count; i++)
            {
                path.Add(data[i]);
                GetCombinationBackTract<T>(data, ans, path, d + 1, limit);
                path.RemoveAt(path.Count - 1);
            }
            
        }
    }
    public class Algorithm
    {
        //抽签问题
        /*
         * 4 Sum问题
         */
        public bool CanSum4(int[] nums, int target)
        {
            var n = nums.Length;
            //求所有排列
            var combination = new SortedSet<int>();
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    combination.Add(nums[i] + nums[j]);
                }
            }

            return combination.
                Any(c => combination.Contains(target - c));
        }
        
        public List<(int, int, int, int)> AllSum4(int[] nums, int target)
        {
            var n = nums.Length;
            //求所有排列 O(n^2)
            var combination = new List<(int sum,int n1, int n2)>();
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    combination.Add((nums[i] + nums[j], nums[i], nums[j]));
                }
            }

            //排序 O(n^2log n)
            combination = combination.OrderBy(e => e.sum).ToList();

            var res =
                (from x in combination
                    let set2 = combination.Where(e => e != x && target == e.sum + x.sum) 
                    from y in set2
                    select (x.n1, x.n2, y.n1, y.n2)).ToList();
            return res;
        }
        
        //最大三角形周长
        /*
         * 方法1.枚举O（n^3)
         * 方法2. 排序，判断相邻三角形 O(nlog n + n)
         */
        public static int MaxTrianglePerimeter(int[] lens)
        {
            if (lens.Length < 3)
                return -1;
            //sort
            var t = lens.OrderBy(e => -e).ToList();
            //前3个和
            var max = int.MinValue;
            if (t[1] + t[2] > t[0])
                max = t.Take(3).Sum();
            //双指针
            int left = 0, right = 2;
            while (right < lens.Length - 1)
            {
                if (t[right] + t[right - 1] < t[right - 2])
                {
                    left++;
                    right++;
                    continue;
                }
                max = Math.Max(max, max - t[left++] + t[right++]);
            }

            return max;

        }
        
        
        //nSum input: a sorted nums array
        public List<List<int>> NSum(int[] nums, int target, int n, int s)
        {
            var res = new List<List<int>>();
            var size = nums.Length;
            if (size < 2 || size < n)
                return res;
            if (n == 2)
            {
                //双指针搜索
                int lo = s, hi = size - 1;
                while (lo < hi)
                {
                    var sum = nums[lo] + nums[hi];
                    int left = nums[lo];
                    int right = nums[hi];
                    if (sum < target)
                    {
                        while (lo < hi && nums[lo] == left) lo++;
                    }else if (sum > target)
                    {
                        while (lo < hi && nums[hi] == right) hi--;
                    }
                    else
                    {
                        res.Add(new int[]{left, right}.ToList());
                        while (lo < hi && nums[lo] == left) lo++;
                        while (lo < hi && nums[hi] == right) hi--;
                    }
                    
                }
            }
            else
            {
                for (var i = s; i < size; i++)
                {
                    var sub = NSum(nums, target - nums[i], n - 1, i + 1);
                    foreach (var e in sub)
                    {
                        e.Add(nums[i]);
                        res.Add(e);
                    }

                    while (i < size - 1 && nums[i] == nums[i + 1]) i++;
                }
            }

            return res;
        }
    }
}