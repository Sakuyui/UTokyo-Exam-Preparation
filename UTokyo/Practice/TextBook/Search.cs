using System.Collections.Generic;
using UTokyo.Geometry;
using UTokyo.Math;

namespace UTokyo.Praticle.TextBook
{
    public class Search
    {
        public int BinarySearch(int[] nums, int t)
        {
            var l = 0;
            var r = nums.Length - 1;
            while (l <= r)
            {
                var mid = (l + r) >> 1;
                if (nums[mid] == t)
                    return mid;
                if (nums[mid] < t)
                {
                    l = mid + 1;
                }
                else
                {
                    r = mid - 1;
                }
            }
            return -1;
        }

        public int BinarySearchLeftEdge(int[] nums, int t)
        {
            var l = 0;
            var r = nums.Length;
            while (l < r)
            {
                var mid = (l + r) >> 1;
                if (nums[mid] >= t)
                {
                    r = mid;
                }
                else if (nums[mid] < t)
                {
                    l = mid + 1;
                }
            }
            return l;
        }

        public int BinarySearchRightEdge(int[] nums, int t)
        {
            var l = 0;
            var r = nums.Length;
            while (l < r)
            {
                var mid = (l + r) >> 1;
                if (nums[mid] <= t)
                {
                    r = mid + 1;
                }
                else
                {
                    r = mid - 1;
                }
            }
            return r - 1;
        }
        
        //是否存在合为m的子集。回溯
        public void FindSumKSubset(int[] nums, int k,int d = 0, List<int> path = null, List<bool> used = null)
        {
            if (k == 0 || d == nums.Length)
                return;
            if (path == null)
                path = new List<int>();
            if (used == null)
                used = new List<bool>();
            var n = nums.Length;
            for (var i = d; i < n && !used[i]; i++)
            {
                path.Add(i);
                used[i] = true;
                FindSumKSubset(nums, d + 1,k - nums[i], path, used);
                used[i] = false;
                path.Remove(path.Count - 1);
            }
        }
        
        //科赫曲线
        public void Koch(int n, Point2D p1, Point2D p2)
        {
            if(n == 0)
                return;
            //找出3个点
            Point2D s1, s2, s3;
            s1 = p1 + (p2 - p1) / 3;
            s2 = p1 + (p2 - p1) * 2 / 3;
            s3 = s1 + ((s2 - s1).Insert(1) * TransFormUtil.Rotation2D(60)).Delete(2);
            
            //output
            Koch(n - 1, p1, s1);
            //output
            Koch(n - 1, s1, s3);
            //output
            Koch(n - 1, s3, s2);
            //output
            Koch(n - 1,s2 ,p2);
        }
    }
}