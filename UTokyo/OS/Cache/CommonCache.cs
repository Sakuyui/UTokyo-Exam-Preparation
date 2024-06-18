using UTokyo.os.Cache.ReplaceStrategies;

namespace UTokyo.os.Cache
{
    public class CommonCache<TK,TV> : AbstractCache<TK,TV>
    {
        public CommonCache(int capacity, BaseCacheStrategy<TK, TV> strategy) : base(capacity, strategy)
        {
        }

        public override TV Access(TK key)
        {
            return (TV) CacheStrategy.DoAccess(key, this, CacheLines);
        }

        public override void Write(TK key, TV val)
        {
            CacheStrategy.DoWrite(key, val, this, CacheLines);
        }
    }
}