using System;
using System.Collections.Generic;

namespace TokyoU
{
    //Counter
    //DataFrame
    //Matrix
    //Tuple
    //Vector
    //调度算法
    internal class Program
    {
        public static void Main(string[] args)
        {
            //========================Tuple============================
            Math.Tuple<int,double> tuple1 = new Math.Tuple<int, double>(11,10.1);
            Math.Tuple<double, double> tuple2 = new Math.Tuple<double, double>(28.7,9.1);
            Math.Tuple<int, double> tuple3 = new Math.Tuple<int, double>(8,9.1);
            Math.Tuple<double, double> tuple4 = new Math.Tuple<double, double>(33,9.1);
            List<Math.Tuple<Object,Object>> tuples = new List<Math.Tuple<object, object>>();
            tuples.Add(tuple1);
            tuples.Add(tuple2);
            tuples.Add(tuple3);
            tuples.Add(tuple4);
            Console.WriteLine(tuple1);
            tuple1.Key += 1;
            Console.WriteLine(tuple1.ConvertTo<double,int>());
            tuples.Sort();  //测试Sort()
            Console.WriteLine(Utils.ListToString(tuples));
        }


    }
}