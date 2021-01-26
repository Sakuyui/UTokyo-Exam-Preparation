using System;
using System.Collections;
using System.Collections.Generic;

namespace UTokyo.Structure
{
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {

        private ComparerStrategy _comparerStrategy;
        public DuplicateKeyComparer(ComparerStrategy c = null)
        {
            _comparerStrategy = c;
        }

        public delegate int ComparerStrategy(TKey x, TKey y);
        public int Compare(TKey x, TKey y)
        {
            
            if (x == null) return 0;
            
            var cmp = _comparerStrategy?.Invoke(x, y) ?? x.CompareTo(y);
            return cmp == 0 ? 1 : cmp;
        }
    }
    
    
}