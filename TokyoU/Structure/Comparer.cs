using System;
using System.Collections.Generic;

namespace TokyoU.Structure
{
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            if (x == null) return 0;
            var cmp = x.CompareTo(y);
            return cmp == 0 ? 1 : cmp;
        }
    }
}