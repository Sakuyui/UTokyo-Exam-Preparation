using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoU.Structure
{
    public static class CollectionHelper
    {
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
    }
    
    
}