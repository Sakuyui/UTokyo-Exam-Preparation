using System;
using System.Collections;
using System.Collections.Generic;

namespace UTokyo
{
    public class Utils
    {
        public static List<T> CreateListWithDefault<T>(int k, T data = default)
        {
            if (k < 0) return null;
            var list = new List<T>();
            for (var i = 0; i < k; i++)
            {
                list.Add(data);
            }
            return list;
        }

        public static List<List<T>> CreateTwoDimensionList<T>(T[] data, int row, int columns)
        {
            var k = data.Length;
            if(row * columns != k) throw new ArithmeticException();
            var list = new List<List<T>>();
            for (var i = 0; i < row; i++)
            {
                var l = new List<T>();
                for (var j = 0; j < columns; j++)
                {
                    l.Add(data[i * columns + j]);
                }
                list.Add(l);
            }
            return list;
        }
        public static List<List<T>> CreateTowDimensionList<T>(int rows, int columns, T data = default)
        {
            var list = new List<List<T>>();
            if (rows == 0 || columns == 0) return list;
            for (var i = 0; i < rows; i++)
            {
                list.Add(CreateListWithDefault(columns,data));
            }
            return list;
        }
        public static String HashMapToString(Hashtable hashtable)
        {
            var str = "{";
            foreach (var a in hashtable.Keys)
            {
                str += "(" + a + "," + hashtable[a] + ")" + " ";
            }

            return str +"}";
    }
        public static String ListToString<T>(List<T> list)
        {
            var str = "[";
            foreach (var VARIABLE in list)
            {
                str += VARIABLE + ",";
            }
            
            str = str.Substring(0, str.Length - 1);
            return str + "]";
        }
    }
}
