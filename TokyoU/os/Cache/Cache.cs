using System.Collections.Specialized;

namespace TokyoU.os.Cache
{
    public class Page
    {
        
    }
    
    public delegate object CacheEvent(object source, object key, object content);

    public abstract class AbstractCache<TK,TV>
    {
        //<key,val> val可以是page

        protected int Size => CacheLines.Count;
        protected readonly int Capacity;
        protected readonly CacheEvent LineReplaced = null;
        protected readonly CacheEvent LineMiss = null;
        protected readonly CacheEvent LineAccess = null;

        protected delegate bool ContentJudgeStrategy(object cacheContent, object writeContent);
        
        //用于判断取出的内容是否是想要的。因为可能key相同。
        protected ContentJudgeStrategy JudgeContent = (c,w) => true;
        
        //OrderedDictionary
        protected readonly OrderedDictionary CacheLines = new OrderedDictionary();  
        protected AbstractCache(int capacity, CacheEvent lineMiss = null, CacheEvent lineReplaced = null, CacheEvent lineAccess = null)
        {
            Capacity = capacity;
            LineMiss = lineMiss;
            LineReplaced = lineReplaced;
            LineAccess = lineAccess;
        }

        public abstract TV Access(TK key);   //访问某个页
        public abstract void Write(TK key, TV val);  //加入一个新的页面 相当于提供 （页号，对应页内容）
    }

    
   
    
}