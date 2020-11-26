using System.Linq;

namespace TokyoU.os.Cache
{
    public class LruCache<TK,TV>: AbstractCache<TK,TV>
    {

        public override TV Access(TK key)
        {
            if (CacheLines.Contains(key))
            {
                //处理
                var e = CacheLines[key];
                CacheLines.Remove(key);
                CacheLines.Insert(0,key,e);
                return (TV)e;
            }
            else
            {
                return default;
            }
        }

        public override void Write(TK key, TV val)
        {
            //缓存满
            if (Size == Capacity)
            {
                if (CacheLines.Contains(key))
                {
                    CacheLines.Remove(key);
                    CacheLines.Insert(0,key,val);
                    return;
                }
                
                //替换策略
                LineReplaced?.Invoke(this, key, val);
                CacheLines.RemoveAt(Size - 1);
                CacheLines.Insert(0, key, val);
            }
            else
            {
                if (CacheLines.Contains(key))
                {
                    CacheLines.Remove(key);
                    CacheLines.Insert(0, key,val);
                    return;
                }

                LineMiss?.Invoke(this, key, val);
                CacheLines.Insert(0, key,val);
            }
        }

        public LruCache(int capacity, CacheEvent lineMiss = null, CacheEvent lineReplaced = null, CacheEvent lineAccess = null) : 
            base(capacity, lineMiss, lineReplaced, lineAccess)
        {
        }


        public override string ToString()
        {
            var str = "LruCache: [Key:";
            str += typeof(TK) + ", Content:" + typeof(TV)+"] Capacity "+CacheLines.Count+"/"+Capacity+"\n";
            //Linq
            return CacheLines.Keys.Cast<object>()
                .Aggregate(str, 
                    (current, key) => current + ("(" + key + ", " + CacheLines[key] + ")\n"));
        }
    }

}