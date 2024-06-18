using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode.Math
{
    public class Math1
    {
        /*不用乘除 mod实现除法*/
        public static int DivideByZeroException(int dividend, int divisor)
        {
            var sign = (dividend > 0) ^ (divisor > 0); //符号
            var result = 0;

            //转换为正数运算。不需要考虑边界
            dividend = dividend  > 0 ? -dividend : dividend;
            divisor = divisor > 0 ? -divisor : divisor;
            
            
            //快速幂思想
            //如果还能除就继续 
            while(dividend <= divisor) {
                var tempResult = -1;
                var tempDivisor = divisor;
                //每次扩大两倍的除数
                while(dividend <= (tempDivisor << 1)) {
                    //判断边界
                    if(tempDivisor < (int.MinValue >> 1))
                        break;
                    tempResult <<= 1;
                    tempDivisor <<= 1;
                }
                
                //更改状态。相当于递归惹
                dividend -= tempDivisor;
                result += tempResult;
            }

     
            return sign ? result : -result;
        }

        public static HashSet<(int, int)> ResolvePrimeFactors(int n)
        {
            if(n < 1)
                return new HashSet<(int, int)>();
            if(n == 1)
                return new HashSet<(int, int)>();
            var t = 2;
            var tuple = (2, 0);
            var tmpN = n;
            var e = (int) System.Math.Sqrt(n) + 1;
            var set = new HashSet<(int, int)>();
            while (t <= e)
            {
                if (tmpN % t == 0)
                {
                    tmpN /= t;
                    tuple.Item2++;
                }
                else
                {
                    set.Add(tuple);
                    t++;
                    tuple = (t, 0);
                }
            }

            return set;
        }
        
        
        /*筛法素数*/
        public static HashSet<int> GetSushuSet(int end)
        {
            var e = (int)System.Math.Sqrt(end) + 1;
            var arr = new bool[e + 1];
            for (var i = 2; i <= e; i++)
            {
                for (var j = 2; j * i <= e; j ++)
                {
                    arr[j * i] = true;
                }
            }

            return arr.Select((e, index) => (index, e)).Where(valueTuple => !valueTuple.e && valueTuple.index!= 0 && valueTuple.index!= 1).Select(valueTuple=> valueTuple.index).ToHashSet();
        }


        public static T Abs<T>(T x)
        {
            return (dynamic) x < 0 ? - (dynamic)x : x;
        }
        //牛顿迭代
        /*
         * 计算sqrt
         */
        
        /*列出 sqrt(m) = c => c^2 = m =>  c ^ 2 - m= 0,因为是要找c，所以c是x轴，也就是解 x^2 - m =0*/
        public static int MySqrt1(int x)
        {
            if (x == 0) {
                return 0;
            }

            double C = x, x0 = x;
            while (true) {
                var xi = 0.5 * (x0 + C / x0);
                if (Abs(x0 - xi) < 1e-7) {
                    break;
                }
                x0 = xi;
            }
            return (int) x0;
        }
        
        /*
         * 前n个丑数 - 堆或dp
         */

        public static List<int> UglyCount(int n)
        {
            var arr = new int[n + 1];
            int i2 = 0, i3 =0, i5 = 0, ugly;
            for (var i = 1; i <= n; i++)
            {
                ugly = System.Math.Min(System.Math.Min(arr[i2] * 2, arr[i3] * 3), arr[i5] * 5);
            }

            return arr.ToList();
        }
        
    }
}