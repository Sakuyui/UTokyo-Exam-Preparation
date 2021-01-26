using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UTokyo.Math;

namespace UTokyo
{


    public static class LinqFunctionExt
    {


        public static List<List<T>> GroupByCount<T>(this IEnumerable<T> collection, int groupSize)
        {
            var enumerable = collection as T[] ?? collection.ToArray();
            var size = enumerable.Count();
            if (groupSize >= size) return new[] {enumerable}.Select(e => e.ToList()).ToList();
            var result = enumerable.Select((c, index) => new {index, c})
                .GroupBy(e => e.index / groupSize)
                .Select(x => x.Select(p => p.c));
            return result.Select(e => e.ToList()).ToList();
        }
        public static Dictionary<TKey, TVal> CastToDictionary<T, TKey, TVal>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector,
            Func<T, TVal> valSelector
        )
        {
            if(source == null || keySelector == null || valSelector == null) throw new ArgumentException();
            return source.ToDictionary(keySelector, valSelector);
        }
        
        public static TSource ArgBy<TSource, TKey>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            Func<(TKey Current, TKey Previous), bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var value = default(TSource);
            var key = default(TKey);

            if (value == null)
            {
                foreach (var other in source)
                {
                    if (other == null) continue;
                    var otherKey = keySelector(other);
                    if (otherKey == null) continue;
                    if (value == null || predicate((otherKey, key)))
                    {
                        value = other;
                        key = otherKey;
                    }

                }
                return value;
            }
            else
            {
                bool hasValue = false;
                foreach (var other in source)
                {
                    var otherKey = keySelector(other);
                    if (otherKey == null) continue;

                    if (hasValue)
                    {
                        if (predicate((otherKey, key))) 
                        {
                            value = other;
                            key = otherKey;
                        }
                    }
                    else
                    {
                        value = other;
                        key = otherKey;
                        hasValue = true;
                    }
                }
                if (hasValue) return value;
                throw new InvalidOperationException("Sequence contains no elements");
            }
        }
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer = null)
        {
            if (comparer == null) comparer = Comparer<TKey>.Default;
            return source.ArgBy(keySelector, lag => comparer.Compare(lag.Current, lag.Previous) < 0);
        }

        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer = null)
        {
            if (comparer == null) comparer = Comparer<TKey>.Default;
            return source.ArgBy(keySelector, lag => comparer.Compare(lag.Current, lag.Previous) > 0);
        }

        public static Matrix<TSource> ToMatrix<TSource>(
            this IEnumerable<TSource> sources,
            int rows, int cols
        )
        {
            if(sources.Count() != rows * cols)
                throw new ArithmeticException();
            var matrix = new Matrix<TSource>(CollectionHelper.CreateTwoDimensionList(
                sources.ToArray(),
                rows,cols
            ));
            return matrix;
        }
    }
    //This class contains the function extension used in this program
    public static class FunctionExt
    {

       
        public static void PrintToConsole(this object obj)
        {
            Console.WriteLine(obj.ToString());
        }
        public static void PrintEnumerationToConsole<T>(this IEnumerable<T> list)
        {
            Console.WriteLine(list.ToEnumerationString());
        }
        public static string ToEnumerationString<T>(this IEnumerable<T> list)
        {
            var res = list.Aggregate("[", (current, e) => current + (e + ","));
            
            res = res.Substring(0, res.Length - 1) + "]";
            return res;
        }
       
        public static void PrintCollectionToConsole<T>(this IEnumerable<T> enumerable)
        {
            var str = "[";
            foreach (var e in enumerable)
            {
                str += e + ",";
            }
            str = str.Substring(0, str.Length - 1) + "]";
            Console.WriteLine(str);
        }
    }
}