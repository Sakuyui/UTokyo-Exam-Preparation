using System;
using System.Collections.Generic;
using System.Linq;
using TokyoU;

namespace Algorithm
{


   
    
    
    
    internal class Program
    {
        
        
        
        public static void Main(string[] args)
        {
            int[] arr = new[] {1, 2, 3, 4, 5};
            new int[5].Select((e, index) => index + 1).PrintEnumerationToConsole();
            var algorithm = new OfferOrientAlgorithm();
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            HashSet<List<int>> set = new HashSet<List<int>>();
            set.Add(list);
            var l2 = new List<int>(list);
            set.Add(l2);
            set.Contains(l2).PrintToConsole();
            set.Contains(list).PrintToConsole();
            List<int> l3 = new List<int>();
            l3.Add(list[0]);
            l3.Add(list[1]);
            set.Contains(l3).PrintToConsole();
            //AllPermutation(arr.ToList(), 5,new bool[5],new Stack<int>() );
            //AllCombination(arr.ToList(), 3, 0, new Stack<int>());
        }


        
        
        //搜索
        //IDDFS
        
        
        //sort

        public static void SortBuble<T>(List<T> list) where T : IComparable
        {
            for (var i = 0; i < list.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (list[i].CompareTo(list[j]) < 0) continue;
                    var t = list[i];
                    list[i] = list[j];
                    list[j] = t;
                }
            }
        }

        


        public static void SortInsert<T>(List<T> list) where T : IComparable
        {
            for (int i = 1; i < list.Count; i++)
            {
                var e = list[i];
                for (int j = i - 1; j > 0; j--)
                {
                    if (list[j].CompareTo(e) < 0)
                    {
                        list[j + 1] = list[j];
                    }
                    else
                    {
                        list[j] = e;
                        break;
                    }
                }
                
            }
        }
        
        
        
        //经典DP

        //编辑距离
        public static int EditCost(string s1, string s2)
        {
            var path = new List<int>();
            if (s1 == s2) return 0;
            if (s1 == null || s2 == null) return -1;
            var dp = new int[s1.Length + 1, s2.Length + 1];
            //dp [i,j] = dp[i-1][j-1] (s1[i] == s2[j]), 否则删除添加替换选一个
            //迭代
            for (var i = 1; i <= s1.Length; i++)
            {
                for (var j = 1; j <= s2.Length; j++)
                {
                    if (s1[i] == s2[j]) dp[i, j] = dp[i - 1, j - 1];
                    else
                    {
                        dp[i, j] = Math.Min(dp[i - 1, j - 1] + 1, Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1));
                    }
                }
            }

            return dp[s1.Length, s2.Length];

        }
        public static int Lcs(string s1, string s2)
        {
            List<int> path = new List<int>();
            
            if(s1.Length == 0 && s2.Length == 0) return 0;
            var dp = new int[s1.Length + 1, s2.Length + 1];
            for (var i = 1; i <= s1.Length; i++)
            {
                for (var j = 1; j <= s2.Length; j++)
                {
                    if (s1 == s2)
                    {
                        dp[i,j] = dp[i - 1, j - 1];
                        path.Add(s1[i - 1]);
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }
            path.PrintEnumerationToConsole();
            return dp[s1.Length, s2.Length];
        }
        
        
        //回溯法

        public static void AllCombination<T>(List<T> list, int k, int start, Stack<T> path)
        {
            if (start > list.Count || k > list.Count) return;
            //判断叶子
            if (path.Count == k)
            {
                path.PrintEnumerationToConsole();
                return;
            }
            
         
            
            //非叶子
            //所有未遍历的节点
            for (var i = start; i < list.Count; i++)
            {
                path.Push(list[i]);
                AllCombination<T>(list, k, i + 1, path);
                path.Pop();
            }

        }

        private static void AllPermutation<T>(List<T> list, int k, bool[] vis, Stack<T> path) 
        {
            if(vis.Length != list.Count) throw new ArithmeticException();
            //判断叶子
            if (path.Count == k) //所有节点已经访问过
            {
                //Output
                path.PrintEnumerationToConsole();
                return;
            }
            
            //非叶子
            //所有未遍历的节点
            for (var i = 0; i < vis.Length; i++)
            {
                if(vis[i]) continue;
                path.Push(list[i]);
                vis[i] = true;
                AllPermutation<T>(list, k, vis, path);
                vis[i] = false;
                path.Pop();
            }
        }

        //KMerge
        public static void KMerge(List<List<int>> source, List<int> des)
        {
            if(source.Count <= 1) return;
            var k = source.Count;
            if(des.Count != k) throw new ArithmeticException();
            int[] pos = new int[k];
            int cur = 0;
            bool[] vis = new bool[k];
            int rest = k;
            while (rest > 1)
            {
                var min = Int32.MaxValue;
                var minI = -1;
                for (int i = 0; !vis[i] && pos[i] <= source[i].Count - 1; i++)
                {
                   if(source[i][pos[i]] <= min)
                   {
                       min = source[i][pos[i]];
                       minI = i;
                   }
                }

                des[cur++] = pos[minI]++;
                if (pos[minI] < source[minI].Count) continue;
                vis[minI] = true;
                
                rest = vis.Count(e => !e);
            }

            var r = -1;
            for (var i = 0; i < source.Count && !vis[i]; i++)
            {
                r = i;
                break;
            }

            while (pos[r] < source[r].Count)
            {
                des[cur++] = source[r][pos[r]++];
            }
        }
        
        
        
        
        //merge sort 如何不用辅助空间完成归并排序
        public static void MergeSort<T>(List<T> es, List<T> dec, int begin, int end) where T : IComparable
        {
            if(end - begin <= 1 || es.Count <= 1 || end >= es.Count) return;
            var midIndex = (begin + end) / 2;
            MergeSort<T>(es, dec, begin, midIndex);
            MergeSort<T>(es, dec,midIndex + 1, end);
            Merge(es, dec, begin, midIndex, end);
        }

        
        public static void Merge<T>(List<T> list,List<T> dec, int begin, int mid, int end) where T : IComparable
        {
            //[begin, mid] [mid + 1, end]
            int i = begin, j = mid + 1, pos = 0;
            while (i <= mid && j <= end)
            {
                if (list[i].CompareTo(list[j]) <= 0)
                {
                    dec[pos++] = list[i++];
                }
                else
                {
                    dec[pos++] = list[j++];
                }
            }

            while (i <= mid)
            {
                dec[pos++] = list[i++];
            }

            while (j <= end)
            {
                dec[pos++] = list[j++];
            }
        }
        
   
        
        //二分探査
        public static int BinarySearch<T>(List<T> elements, T target) where T: IComparable
        {
            foreach (var variable in elements)
            {
                Console.WriteLine(variable);
            }
            var left = 0;
            var right = elements.Count() - 1;
            while (left <= right)
            {
                var mid = (right + left) / 2;
                Console.WriteLine("find" + elements[mid]);
                if (elements[mid].Equals(target))
                {
                    return mid;
                }
                if (elements[mid].CompareTo(target) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return -1;
        }
    }
}