using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TokyoU.os.Cache;
using TokyoU.Structure;

namespace TokyoU.Test
{
    public static class AdvanceStructureTest
    {
        public static void DictionaryTest()
        {
            OrderedDictionary dictionary = new OrderedDictionary {{"a", 13}, {"b", 1}, {"+12s", 113}, {"324", 1232}};
            for (int i = 0; i < dictionary.Count; i++)
            {
                Console.WriteLine(dictionary[i]);
            }

            
        }


        public static void StructureTest()
        {
           
        }


        public static void IndexKeepTableTest()
        {
            var list = new List<int>(new int[]{4,11,4,2,1,7,5,91,23,5,44});
            var table = new IndexKeepTable<int>(list);
            var stable = table.SortData(true);
            foreach (var x in stable.nodes)
            {
                Console.Write(x.Data + "[" + x.SourceIndex + "] ");
            }
        }
        
        public static void CacheTest()
        {
            
            SortedSet<int> set = new SortedSet<int>(new DuplicateKeyComparer<int>());
            set.Add(1 );
            set.Add(4);
            set.Add(2);
            set.Add(0);
            set.Add(4);
            foreach (var e in set)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("\n");
            CommonCache<int, int> lruCache = CacheBuilder.BuildLruCommonCache<int, int>(5);
                // new LruCache<int, int>(5,null,
              //  delegate(object source, object key, object content) { Console.WriteLine(key + " be replaced");return null; });
            lruCache.Write(4,0);
            lruCache.Write(7,0);
            lruCache.Write(0,0);
            lruCache.Write(7,0);
            lruCache.Write(1,0);
            lruCache.Write(0,0);
            lruCache.Write(1,0);
            lruCache.Write(2,0);
            lruCache.Write(1,0);
            lruCache.Write(2,0);
            lruCache.Write(6,0);
            Console.WriteLine(lruCache);
        }
    }
}