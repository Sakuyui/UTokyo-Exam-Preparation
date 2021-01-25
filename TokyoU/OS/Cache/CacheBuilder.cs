using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using TokyoU.os.Cache.ReplaceStrategies;

namespace TokyoU.os.Cache
{
    public class CacheBuilder
    {
        private CacheBuilder()
        {
            
        }
        private static CacheBuilder _instance;
        private static readonly Object InstanceLock = new object();
        public static CacheBuilder GetInstance()
        {
            if (_instance == null)
            {
                lock (InstanceLock)
                {
                    if(_instance == null) _instance = new CacheBuilder();
                }
            }

            return _instance;
        }
        public static CommonCache<TK,TV> BuildFifoCommonCache<TK, TV>(int capacity)
        {
            CommonCache<TK, TV> cache = new CommonCache<TK, TV>(capacity, new CacheFifoStrategy<TK, TV>());
            return cache;
        }
        public static CommonCache<TK,TV> BuildLruCommonCache<TK, TV>(int capacity)
        {
            CommonCache<TK, TV> cache = new CommonCache<TK, TV>(capacity, new CacheLruStrategy<TK, TV>());
            return cache;
        }
        public static CommonCache<TK,TV> BuildLfuCommonCache<TK, TV>(int capacity)
        {
            CommonCache<TK, TV> cache = new CommonCache<TK, TV>(capacity, new CacheLfuStrategy<TK, TV>());
            return cache;
        }

        public static CommonCache<TK,TV> BuildCustomeCommonCache<TK, TV>(int capacity, BaseCacheStrategy<TK,TV> strategy)
        {
            CommonCache<TK,TV> cache = new CommonCache<TK, TV>(capacity, strategy);
            return cache;
        }

       
        public static CommonCache<TK,TV> BuildCustomeCommonCache<TK, TV>(int capacity, 
            CacheCustomStrategy<TK,TV>.DoAccessDelegate<TK,TV> doAccessStrategy, CacheCustomStrategy<TK,TV>.DoWriteDelegate<TK,TV> doWriteDelegate)
        {
            BaseCacheStrategy<TK,TV> strategy =  new CacheCustomStrategy<TK, TV>(doAccessStrategy, doWriteDelegate);
            CommonCache<TK,TV> cache = new CommonCache<TK, TV>(capacity, strategy);
            return cache;
        }
    }
}