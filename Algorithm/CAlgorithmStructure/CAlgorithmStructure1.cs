using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using C5;

namespace Algorithm
{
    public class  CAlgorithmStructure1
    {
        
        /*
         * Top K 使用堆的方法  O(nlog k)
         */
        public List<T> TopK<T>(IEnumerable<T> data, int k) where T : IComparable
        {
            //使用堆
            var heap = new IntervalHeap<T>();
            foreach (var e in data)
            {
                if (heap.Count < k)
                    heap.Add(e);
                else
                {
                    if (e.CompareTo(heap.Min()) > 0)
                    {
                        heap.DeleteMin();
                        heap.Add(e);
                    }
                }
            }
            return heap.ToList();
        }
        
        
        /*
         * Swap
         */
        public void Swap<T>(T[] collection, int i, int j)
        {
            var t = collection[i];
            collection[i] = collection[j];
            collection[j] = t;
        }


  
        /*
         * QuickSort
         */
        public void QuickSort(int[] nums, int start, int end)
        {
            if (end - start <= 0)
                return;
            var pIndex = new Random().Next(start, end);
            var p = nums[pIndex];
            Swap(nums, pIndex, end);
            var l = start;
            var r = end - 1;
            while (l < r)
            {
                if (nums[l] <= p)
                    l++;
                else
                {
                    Swap(nums,l,r--);
                }
            }

            if (nums[l] < p)
                l++;
            Swap(nums,end,l);
            QuickSort(nums, start, l - 1);
            QuickSort(nums, l + 1, end);
        }
        /*
         * QuickSort Single Pointer
         */
        public void QuickSortSinglePointer(int[] nums, int start, int end)
        {
            if(end >= start)
                return;
            var n = nums.Length;
            var pt = start;
            //基准是left元素
            //pt永远指向比p小的,指向的是最近的比nums[left]小的元素
            for (var i = pt + 1; i <= end; i++)
            {
                if (nums[i] < nums[start])
                {
                    //把i指向的元素扔过来。这样[l, pt + 1]肯定是 <= pivot的
                    Swap(nums, ++pt, i);
                }
            }
            Swap(nums, pt, start);
            QuickSort(nums, start, pt - 1);
            QuickSort(nums, pt + 1, end);
        }
        
        
        
        
        /*
         * 顺序统计量的办法
         * O(k * log n)
         */
        public List<T> TopKOrderStatistic<T>(List<T> data, int k) where T : IComparable
        {
            var tmp = 
                new T[k].Select((e, index) => index + 1).ToDictionary(ks=> ks, vs => default(T));
            var visited = new bool[k];
            for (var i = 0; i < k; i++)
            {
                if(visited[i]) continue;
                FindIOrder(data, i, 0, data.Count - 1, tmp, visited);
            }
            return tmp.Values.ToList();
        }

        public void FindIOrder<T>(List<T> data, int i, int start, int end, Dictionary<int, T> record, bool[] vis)
            where T : IComparable
        {
            //寻找第i顺序统计量
            var pivotIndex = new Random().Next(0, data.Count - 1);
            var p = data[pivotIndex];
            //swap
            var t = data[end];
            data[end] = data[pivotIndex];
            data[pivotIndex] = t;
            //类似快排
            var l = start;
            var r = end - 1;
            while (l < r)
            {
                if (data[l].CompareTo(p) <= 0)
                    l++;
                else
                {
                    //swap
                    t = data[r];
                    data[r] = data[l];
                    data[l] = t;
                    r--;
                }
            }
            if (data[l].CompareTo(p) < 0)
                l++;
            //swap
            t = data[l];
            data[l] = data[end];
            data[end] = t;
            if (record.ContainsKey(l))
            {
                record[l] = p;
                vis[l] = true;
            }
            if (l == i)
                return;
            if (i < l)
                FindIOrder<T>(data, i, start,l - 1, record, vis);
            else
                FindIOrder<T>(data, i, l + 1, end, record, vis);
        }



        /*
         * (dp) Maximum Profit
         */
        public int MaximumProfit(int[] nums)
        {
            //最大最小值法 O(n)
            var n = nums.Length;
            var max = int.MaxValue;
            var min = nums[0];
            for (var i = 1; i < n; i++)
            {
                max = Math.Max(max, min);
                min = Math.Min(min, nums[i]);
            }
        
            //DP方法

            return max - min;
        }
        
        /*
         * 最大子数组和
         */
        public int MaxSubArraySum(int[] nums)
        {
            var dp = new int[nums.Length];
            dp[0] = nums[0];
            for (var i = 1; i < nums.Length; i++)
            {
                dp[i] = Math.Max(dp[i - 1] + nums[i], nums[i]);
            }
            return dp[nums.Length - 1];
        }
        
        /*
         * 插入排序惹
         */
        public void InsertSort(int[] nums)
        {
            var n = nums.Length;
            for (var i = 1; i < n; i++)
            {
                var p = nums[i];
                for (var j = i - 1; j >= 0; j--)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        nums[j + 1] = nums[j];
                    }
                    else
                    {
                        nums[j + 1] = p;
                        break;
                    }
                }
            }
        }
        
        /*
         * 冒泡: left forward
         */
        public void BubleSortLeftForwardVersion(int[] nums)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                for (var j = i + 1; j < nums.Length - 1; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        var x = nums[j];
                        nums[j] = nums[j + 1];
                        nums[j + 1] = x;
                    }
                }
            }
        }
        /*
         * 冒泡： RightForward
         */
        public void BubleSortRightForwardVersion(int[] nums)
        {
            var flag = false;
            for (var i = nums.Length - 1; i > 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (nums[j] <= nums[j + 1]) continue;
                    var t = nums[j];
                    nums[j] = nums[j + 1];
                    nums[j + 1] = t;
                    flag = true;
                }
                if(flag) break;
            }
        }
        
        /*
         * 逆序数对。就是冒泡惹。但是可以用归并求
         */
        public int InversePairs(int[] nums)
        {
            if (nums.Length == 0)
                return 0;
            var cnt = 0;
            MergeSortP(nums,0 , nums.Length - 1, ref cnt);
            return cnt;
        }

        public void MergeSortP(int[] nums, int l, int r, ref int cnt)
        {
            if (l < r)
            {
                var mid = (l + r) >> 1;
                MergeSortP(nums, l ,mid, ref cnt);
                MergeSortP(nums, mid + 1, r, ref cnt);
                MergeP(nums, l, mid, r, ref cnt);
            }
        }

        public void MergeP(int[] nums, int l, int m, int r, ref int cnt)
        {
            //归并结果
            var list = new List<int>();
            //归并
            var i = l;
            var j = m + 1;
            while (i <= m && j <= r)
            {
                if (nums[i] > nums[j])
                {
                    //碰到逆序对
                    list.Add(nums[j++]);
                    cnt += (m - i + 1);
                }
                else
                {
                    list.Add(nums[i++]);
                }
            }
            while(i <= m)
                list.Add(nums[i++]);
            while(j <= r)
                list.Add(nums[j++]);
            //copy
            for (i = 0; i < list.Count; i++)
            {
                nums[l + i] = list[i];
            }
        }
        
        /*
         * 选择排序
         */
        public void SelectionSort(int[] nums)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                var minIndex = i;
                for (var j = i + 1; j < nums.Length; i++)
                {
                    if (nums[j] < nums[minIndex])
                        minIndex = j;
                }
                //swap
                var x = nums[i];
                nums[i] = nums[minIndex];
                nums[minIndex] = x;
            }
        }
        
        /*
         * ShellSort
         */
        public void ShellSort(int[] nums)
        {
            var n = nums.Length;
            var step = n / 2;
            while (n > 1)
            {
                for (var i = nums.Length - 1; i >= 0; i -= step)
                {
                    for (var j = i - step; j >= 0; j -= step)
                    {
                        var t = nums[i];
                        if (nums[j] > nums[j + step])
                        {
                            nums[j + step] = nums[j];
                        }
                        else
                        {
                            nums[j + step] = t;
                        }
                    }
                }
                n /= 2;
            }
            InsertSort(nums);
        }


  
        
        
        public void MergeSort(int[] nums, int s, int e)
        {
            if (e >= s)
                return;
            var m = (s + 1) >> 1;
            MergeSort(nums, s, m); // [s,m]
            MergeSort(nums, m + 1, e);
            Merge(nums, s, m , e);
        }

        public void Merge(int[] nums, int s, int m, int e)
        {
            var i = s;
            var j = m + 1;
            var tmp = new List<int>();
            while (i <= m && j <= e)
            {
                tmp.Add(nums[i] < nums[j] ? nums[i++] : nums[j++]);
            }
            while(i <= m)
                tmp.Add(nums[i++]);
            while(j <= e)
                tmp.Add(nums[j++]);
            for (i = 0; i < tmp.Count; i++)
            {
                nums[s + i] = tmp[i];
            }
        }
        
        /*
         * CountSort
         */
        public void CountSort(int[] nums)
        {
            var m = nums.Max();
            if(m < 0)
                throw new ArithmeticException();
            var c = new int[m + 1];
            foreach (var e in nums)
            {
                c[e]++; //count
            }
            //求和
            for (var i = 1; i < nums.Length; i++)
            {
                c[i] = c[i] + c[i - 1];
            }
            //排序惹
            var result = new int[nums.Length];
            for (var i = nums.Length - 1; i >= 0; i--)
            {
                result[c[nums[i]]] = nums[i];
                c[nums[i]]--;
            }
            //copy
            for (var i = 0; i < nums.Length; i++)
            {
                nums[i] = result[i];
            }
        }
        
        //数据结构部分
        /*
         * 计算逆波兰表达式
         */
        public double CalcExpressionFromReversedPoLan(string e)
        {
            var elements = e.Split(' ');
            var stack = new Stack<double>();
            foreach (var o in elements)
            {
                double t = -1;
                if (double.TryParse(o, out t))
                {
                    stack.Push(t);
                }
                else
                {
                    if ("+".Equals(e))
                    {
                        if(stack.Count < 2) throw new ArithmeticException();
                        var op1 = stack.Pop();
                        var op2 = stack.Pop();
                        stack.Push(op1 + op2);
                    }else if ("-".Equals(e)) {
                        if(stack.Count < 2) throw new ArithmeticException();
                        var op1 = stack.Pop();
                        var op2 = stack.Pop();
                        stack.Push(op2 - op1);
                    }else if ("*".Equals(e)) {
                        if(stack.Count < 2) throw new ArithmeticException();
                        var op1 = stack.Pop();
                        var op2 = stack.Pop();
                        stack.Push(op1 * op2);
                    }else if ("/".Equals(e)) {
                        if(stack.Count < 2) throw new ArithmeticException();
                        var op1 = stack.Pop();
                        var op2 = stack.Pop();
                        stack.Push(op2 / op1);
                    }
                }
            }
            if(stack.Count != 1) throw new ArithmeticException();
            return stack.Pop();
        }
        
        /*
         * 中缀转后缀
         */
        public string InfixExpressionToSuffix(string infixExp)
        {
            var elements = infixExp.Split(' ');
            var stack = new Stack<string>();
            var priorities = new Dictionary<string, int>();
            var res = "";
            priorities.Add("+", 2);
            priorities.Add("-", 2);
            priorities.Add("*", 1);
            priorities.Add("/", 1);
            foreach (var e in elements)
            {
                double t = -1;
                if ("(".Equals(e))
                {
                    stack.Push("(");
                }else if(double.TryParse(e,out t))
                {
                    res += e + " ";
                }else if (")".Equals(e))
                {
                    if(stack.Count == 0) throw new ArithmeticException();
                    var s = stack.Pop();
                    while (!")".Equals(s))
                    {
                        res += s + " ";
                        if(stack.Count == 0) throw new ArithmeticException();
                        s = stack.Pop();
                    }
                }
                else if (!priorities.Keys.Contains(e))
                {
                    throw new ArithmeticException();
                }
                else
                {
                    var p = priorities[e];
                    if(stack.Count == 0) throw new ArithmeticException();
                    var s = stack.Pop();
                    double x = -1;
                    while ((double.TryParse(s ,out x) || priorities[s] >= p) && stack.Count != 0)
                    {
                        res += s + " ";
                        s = stack.Pop();
                    }
                }
            }

            if (stack.Count != 0)
                throw new ArithmeticException();
            return res;
        }
        
        /*
         * 水坑面积
         */

        public int WaterArea(string surfaces)
        {
            var res = 0;
            var stack = new Stack<int>();
            for (int i = 0; i < surfaces.Length; i++)
            {
                var e = surfaces[i];
                if ('_'.Equals(e))
                    continue;
                if ('\\'.Equals(e))
                {
                    stack.Push(i); //吧位置放入栈的中间
                }
                else if ('/'.Equals(e))
                {
                    if(stack.Count == 0)
                        continue;
                    var p = stack.Pop();
                    res += (i - p);
                }else
                {
                    throw new ArithmeticException();
                }
            }

            return res;
        } 
        
        
        /*
         * 二分搜索: 装载货物的最优解
         * 输入w[n]代表n个货物。装到k个卡车上。每辆最大运载量为P。求最小的P
         */
        public int MinLoadCapacityP(int[] w, int k)
        {
            var n = w.Length;
            if (n == 0 || k <= 0) return 0;
            var left = 0;
            var right = w.Sum();
            while (left < right)
            {
                var mid = (right - right) / 2;
                //判断是否可以
                if (CheckLoad(k, mid, w) == w.Length)
                {
                    right = mid;
                }
                else if(CheckLoad(k,mid,w) < w.Length)
                {
                    left = mid;
                }
            }
            return left;
        }

        public int CheckLoad(int k, int P, int[] w)
        {
            int i = 0;
            for (var j = 0; j < k; j++)
            {
                int s = 0;
                while (s + w[i] <= P)
                {
                    s += w[i];
                    i++;
                    if (i == w.Length)
                        return w.Length;
                }
            }

            return i;
        }

        
        

    }
}