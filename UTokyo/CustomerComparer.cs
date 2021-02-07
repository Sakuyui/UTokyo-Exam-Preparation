using System;
using System.Collections.Generic;
using System.Linq;

namespace UTokyo
{
    public class CustomerComparer<T> : Comparer<T>
    {
        public delegate int CompareStrategy(T t1, T t2);

        private CompareStrategy _compareStrategy = null;
     
        public CustomerComparer(CompareStrategy compareStrategy)
        {
            _compareStrategy = compareStrategy;
        }
        public override int Compare(T x, T y)
        {
            return _compareStrategy.Invoke(x, y); ;
        }

        
        //后缀数组
        public static void Test()
        {
            const string tmp = "abracadabra";
            var s = tmp.Select((t, i) 
                => tmp.Substring(i, tmp.Length - i)).ToList();
            s.Add("");
            s.PrintCollectionToConsole();
            var orderedEnumerable = 
                s.OrderBy(e => e, new CustomerComparer<string>((t1, t2) =>
                    string.Compare(t1, t2, StringComparison.Ordinal)
                ));
            orderedEnumerable.PrintCollectionToConsole();
        }
    }
 
}