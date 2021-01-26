using System;
using System.Collections.Generic;
using System.Linq;

namespace UTokyo
{
    
    //HashBag: 能够存储的同时记录次数
    public static class CollectionHelper
    {
        
        //组合
        public static void Combination<T>(List<T> list, int k, int start, Stack<T> path)
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
                Combination<T>(list, k, i + 1, path);
                path.Pop();
            }

        }
        
        
        //排列
        public static void Permutation<T>(List<T> list, int k, bool[] vis, Stack<T> path) 
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
                Permutation<T>(list, k, vis, path);
                vis[i] = false;
                path.Pop();
            }
        }
        
        public static IEnumerable<IEnumerable<T>> SplitCollection<T>(IEnumerable<T> collection, int sizePerGroup)
        {
            var enumerable = collection as T[] ?? collection.ToArray();
            var size = enumerable.Count();
            if (sizePerGroup >= size) return new[] {enumerable};
            var result = enumerable.Select((c, index) => new {index, c})
                .GroupBy(e => e.index /sizePerGroup)
                .Select(x => x.Select(p => p.c));
            return result;
        }
        
        //linq排序  var list = (from c in heap orderby c descending select c).ToList();
        
        public static void Test()
        {
            int[] nums = new []{1,2,3,4,5,6,7,8};
            var s = CollectionHelper.SplitCollection(nums,3);
            foreach (var t in s)
            {
                foreach(var e in t)
                {
                    Console.Write(e +" ");
                }
                Console.WriteLine();
            }
            
        }
        
        public static List<T> CreateListWithDefault<T>(int k, T data = default)
        {
            if (k < 0) return null;
            var list = new List<T>();
            for (var i = 0; i < k; i++)
            {
                list.Add(data);
            }
            return list;
        }

        public static List<List<T>> CreateTwoDimensionList<T>(T[] data, int row, int columns)
        {
            var k = data.Length;
            if(row * columns != k) throw new ArithmeticException();
            var list = new List<List<T>>();
            for (var i = 0; i < row; i++)
            {
                var l = new List<T>();
                for (var j = 0; j < columns; j++)
                {
                    l.Add(data[i * columns + j]);
                }
                list.Add(l);
            }
            return list;
        }
        public static List<List<T>> CreateTowDimensionList<T>(int rows, int columns, T data = default)
        {
            var list = new List<List<T>>();
            if (rows == 0 || columns == 0) return list;
            for (var i = 0; i < rows; i++)
            {
                list.Add(CreateListWithDefault(columns,data));
            }
            return list;
        }
    }
    
    
}