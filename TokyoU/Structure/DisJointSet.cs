using System;
using System.Collections.Generic;
using C5;

namespace TokyoU.Structure
{
    public class DisJointSet <T>
    {
        private readonly HashDictionary<T, System.Collections.Generic.HashSet<T>> _dictionary =
            new HashDictionary<T, System.Collections.Generic.HashSet<T>>(); //代表点到集合的映射
        private HashDictionary<T, T> _representDictionary = new HashDictionary<T, T>(); //元素到代表点的映射
        
        public void MakeSet(T e)
        {
            if (_representDictionary[e] != null || _dictionary[e] != null) return; //已经存在了
            var s = new System.Collections.Generic.HashSet<T>();
            _representDictionary[e] = e; //新的元素自己指向自己
            _dictionary.Add(e, s); //代表点->集合
        }

        public bool IsSame(T a, T b)
        {
            return _representDictionary[a].Equals(_representDictionary[b]);
        }


        //从一个元素去找集合
        public (T, System.Collections.Generic.HashSet<T>) FindSet(T e)
        {
            var p = _representDictionary[e];
            return p == null ? (default, null) : (p, _dictionary[p]);
        }


        public bool DeleteSet(T representElement)
        {
            if (_dictionary[representElement] == null) return false;
            var s = _dictionary[representElement];
            _dictionary.Remove(representElement);
            foreach (var e in s)
            {
                _representDictionary.Remove(e);
            }
            s.Clear();
            return true;
        }
        public bool DeleteElement(T e)
        {
            if (_representDictionary[e] == null) return false;
            var p = _representDictionary[e];
            if (!p.Equals(e))
            {
                _representDictionary.Remove(e);
                _dictionary[e].Remove(e);
            }
            else
            {
                //要删除的是根。
                var s = _dictionary[e];
                if (s.Count <= 1)
                {
                    s.Clear();
                    _dictionary.Remove(p);
                    _representDictionary.Remove(e);
                }
                else
                {
                    s.Remove(e);
                    T newP = default;
                    foreach (var setElement in s)
                    {
                        newP = setElement;
                        break;
                    }

                    _dictionary[newP] = s;
                    foreach (var setElement in s){
                        _representDictionary[setElement] = newP;
                    }
                }
            }
            return true;
        }
        //将两个元素合并。
        public void Unite(T a, T b)
        {
            MakeSet(a);
            MakeSet(b);
            var (p1, s1) = FindSet(a);
            var (p2, s2) = FindSet(b);

            if (p1.Equals(p2)) return;
            //合并两个集合
            var newP = s1.Count >= s2.Count ? p1 : p2;
            s1.UnionWith(s2);
            _representDictionary[newP] = newP;
            _dictionary.Remove(p1);
            _dictionary.Remove(p2);
            _dictionary[newP] = s1;
            s2.Clear();
            
        }
    }
}