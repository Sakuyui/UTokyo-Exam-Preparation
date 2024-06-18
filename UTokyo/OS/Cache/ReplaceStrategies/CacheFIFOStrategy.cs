using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace UTokyo.os.Cache.ReplaceStrategies
{
    public class CacheFifoStrategy<TK,TV> : BaseCacheStrategy<TK,TV>
    {
        private readonly Queue<TK> _queue = new Queue<TK>();
        

        public override object DoAccess(TK key, AbstractCache<TK, TV> cache, OrderedDictionary cacheLines)
        {
            return (cacheLines.Contains(key) ? cacheLines[key] : null);
        }

        public override object DoWrite(TK key, TV val, AbstractCache<TK,TV> cache, OrderedDictionary cacheLines)
        {
            if (cacheLines.Contains(key))
            {
                if (!JudgeContent(cacheLines[key],val))
                {
                    cacheLines[key] = val;
                }
            }
            else
            {
                if (cacheLines.Count >= cache.Capacity)
                {
                    //替换
                    var e = _queue.Dequeue();
                    cacheLines.Remove(e);
                    _queue.Enqueue(key);
                    cacheLines.Add(key,val);
                }
                else
                {
                    _queue.Enqueue(key);
                    cacheLines.Add(key,val);
                }
            }
            return true;
        }

    }
}