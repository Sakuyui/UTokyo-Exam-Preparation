using System;
using System.Collections.Generic;

namespace TokyoU
{
    public class Utils
    {
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