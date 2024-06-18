using System;
using System.Collections.Generic;
using System.Linq;

namespace UTokyo.Praticle.TextBook
{
    public class Sort
    {
        public static void Swap<T>(T[] array, int i, int j)
        {
            var t = array[i];
            array[i] = array[j];
            array[j] = t;
        }
        public static void BubbleSortForward(int[] nums)
        {
            var n = nums.Length;
            for (var i = 0; i < n - 1; i++)
            {
                var flag = true;
                for (var j = 0; j < n - i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        Swap(nums, i, j);
                        flag = false;
                    }
                }
                if(flag)
                    break;
            }
        }

        public static void BubbleSort2(int[] nums)
        {
            var n = nums.Length;
            for (var i = n - 2; i >= 0; i --)
            {
                var flag = true;
                for (var j = 0; j <= i; j--)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        Swap(nums, j, j + 1);
                        flag = false;
                    }
                }
                if(flag)
                    break;
            }
        }

        public static void QuickSortSinglePointer(int[] nums, int s, int e)
        {
            if(e >= s)
                return;
            var n = nums.Length;
            var pt = s;
            var pivot = nums[pt];
            for (var i = pt + 1; i < n; i++)
            {
                if(nums[i] < pivot)
                    Swap(nums, ++pt, i);
            }
            Swap(nums, pt, s);
            QuickSortSinglePointer(nums, s, pt - 1);
            QuickSortSinglePointer(nums, pt + 1, e);
        }

        public static void QuickSort(int[] nums, int s, int e)
        {
            var n = nums.Length;
            if(e >= s)
                return;
            var pIndex = new Random().Next(s, e);
            var pivot = nums[pIndex];
            Swap(nums, e, pIndex);
            var l = s;
            var r = e - 1;
            while (l <= r)
            {
                if (nums[l] > pivot)
                {
                    Swap(nums, l, r--);
                }
                else
                    l++;
            }
        }

        public static void InsertSort(int[] nums)
        {
            var n = nums.Length;
            for (var i = 1; i < n - 1; i++)
            {
                var t = nums[i];
                for (var j = i; j > 0; j--)
                {
                    if (nums[j] < nums[j - 1])
                    {
                        nums[j] = nums[j - 1];
                    }
                    else
                    {
                        nums[j] = t;
                        break;
                    }
                }
            }
        }

        public static void SelectionSort(int[] nums)
        {
            var n = nums.Length;
            for(var i = 0; i < n; i++)
            {
                var maxIndex = nums.Skip(i).MinBy(e => e) + i;
                Swap(nums, maxIndex, i);
            }
        }

        public static void InsertSort(int[] nums, int g)
        {
            var n = nums.Length;
            for (var gEnd = g; gEnd < n; gEnd++) //定末尾
            {
                var p = nums[gEnd];
                var pt = gEnd - g;
                for (; pt >= 0 && nums[pt] > p; pt -= g) //每次-g地插入排序
                {
                    nums[pt + g] = nums[pt];
                }

                nums[pt + g] = p;
            }
        }

        public static void ShellSort(int[] nums)
        {
            var n = nums.Length;
            var g = new[] {1, 2, 4, 8};
            for (var i = 0; i < g.Length; i++)
            {
                InsertSort(nums, g[i]);
            }
            InsertSort(nums);
        }

        public static void CountSort(int[] nums)
        {
            var maxNum = nums.Max();
            var c = new int[maxNum + 1];
            
            foreach (var e in nums)
            {
                c[e]++;
            }

            for (var i = 1; i < c.Length; i++)
            {
                c[i] = c[i] + c[i - 1];
            }

            var n = nums.Length;
            var sort = new int[n];
            for (var i = c.Length - 1; i >= 0; i--)
            {
                sort[--c[nums[i]]] = nums[i];
            }
            
        }

        public static void MergeSort(int[] nums, int s, int e)
        {
            if(e >= s)
                return;
            var m = (s + e) >> 1;
            MergeSort(nums, s, m);
            MergeSort(nums, m + 1, e);
            MergeForSort(nums, s, m, e);
        }

        public static void MergeForSort(int[] nums, int s, int m, int e)
        {
            var l = s;
            var r = m + 1;
            var tmp = new List<int>();
            while (l <= m && r <= e)
            {
                if(nums[l] < nums[r])
                    tmp.Add(nums[l++]);
                else
                    tmp.Add(nums[r++]);
            }
            while(l <= m)
                tmp.Add(nums[l++]);
            while(r <= e)
                tmp.Add(nums[r++]);
            for (var i = 0; i < tmp.Count; i++)
            {
                nums[s + i] = tmp[i];
            }
        }
    }
}