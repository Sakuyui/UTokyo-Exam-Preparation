using System.Collections.Generic;
using System.Linq;
using TokyoU.os.Cache;

namespace TokyoU.os
{
    public class FifoCache<TK, TV> : AbstractCache<TK, TV>
    {
        private readonly Queue<TK> _queue = new Queue<TK>();
        public override TV Access(TK key)
        {
            return (TV)(CacheLines.Contains(key) ? CacheLines[key] : null);
        }

        public override void Write(TK key, TV val)
        {
            if (CacheLines.Contains(key))
            {
                if (!JudgeContent(CacheLines[key],val))
                {
                    CacheLines[key] = val;
                }
            }
            else
            {
                if (Size >= Capacity)
                {
                    //替换
                    var e = _queue.Dequeue();
                    CacheLines.Remove(e);
                    _queue.Enqueue(key);
                    CacheLines.Add(key,val);
                }
                else
                {
                    _queue.Enqueue(key);
                    CacheLines.Add(key,val);
                }
            }
        }

        public FifoCache(int capacity, CacheEvent lineMiss = null, CacheEvent lineReplaced = null, CacheEvent lineAccess = null) : 
            base(capacity, lineMiss, lineReplaced, lineAccess)
        {
            
        }
        
        public override string ToString()
        {
            var str = "FIFOCache: [Key:";
            str += typeof(TK) + ", Content:" + typeof(TV)+"] Capacity "+CacheLines.Count+"/"+Capacity+"\n";


            return CacheLines.Keys.Cast<object>().
                Aggregate(str, (current, key) => current + ("(" + key + ", " + CacheLines[key] + ")\n"));
        }
    }
}