using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TokyoU.os.Cache
{
    public class LfuCache<TK, TV> : AbstractCache<TK, TV> 
    {
        private readonly OrderedDictionary _frequentSetMap= new OrderedDictionary();
        private readonly OrderedDictionary _keyFrequentMap= new OrderedDictionary();
        
        public override TV Access(TK key)
        {
            if (!CacheLines.Contains(key))
            {
                LineMiss?.Invoke(this, key, null);
                return default;
            }
            else
            {
              
                var c = CacheLines[key]; //根据key直接获取内容
                var freq = (int) _keyFrequentMap[key];  //根据k获取频率
                ((List<TK>) _frequentSetMap[freq]).Remove(key); //从该频率集合中删除key
                //key转移到下一频率
                if (_frequentSetMap.Contains(freq + 1))
                {
                    ((List<TK>) _frequentSetMap[freq + 1]).Add(key);
                }
                else
                {
                    _frequentSetMap.Add(freq + 1, new SortedSet<TK>());
                }
                //修改频率映射
                _keyFrequentMap[key] = freq + 1;
                return (TV) c;
            }
        }

        public override void Write(TK key, TV val)
        {
            if (!CacheLines.Contains(key))
            {
                if (Size < Capacity)
                {
                    _keyFrequentMap[key] = 1;
                    CacheLines.Add(key, val);
                    //key加入频率列表
                    if (_frequentSetMap.Contains(1))
                    {
                        ((List<TK>) _frequentSetMap[1]).Add(key);
                    }
                    else
                    {
                        _frequentSetMap.Add(1, new List<TK>());
                    }
                }
                else
                {
                    //需要替换惹。
                    foreach (var f in _frequentSetMap.Keys)
                    {
                        var list = (List<TK>) _frequentSetMap[f];
                        if (list.Count <= 0) continue;
                        //替换出一个元素
                        _keyFrequentMap.Remove(list[0]);
                        CacheLines.Remove(list[0]);
                        list.RemoveAt(0);
                        //插入新元素
                        _keyFrequentMap[key] = 1;
                        CacheLines.Add(key, val);
                        //key加入频率列表
                        if (_frequentSetMap.Contains(1))
                        {
                            ((List<TK>) _frequentSetMap[1]).Add(key);
                        }
                        else
                        {
                            _frequentSetMap.Add(1, new List<TK>());
                        }
                        break;
                    }
                }
            }
            else
            {
                //存在key的情况
                Access(key);
                CacheLines[key] = val;
            }
        }


        public LfuCache(int capacity, CacheEvent lineMiss = null, CacheEvent lineReplaced = null, CacheEvent lineAccess = null) 
            : base(capacity, lineMiss, lineReplaced, lineAccess)
        {
        }
    }
}