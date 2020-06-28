using System;

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
            Math.Tuple<int,double> tuple1 = new Math.Tuple<int, double>(10,10.1);
            Console.WriteLine(tuple1);
            tuple1.Key += 1;
            Console.WriteLine(tuple1.ConvertTo<double,int>());
            
        }
    }
}