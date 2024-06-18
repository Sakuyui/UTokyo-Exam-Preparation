using System.Collections.Generic;
using System.Linq;

namespace Leetcode.MyMath
{
    public class Math1
    {

        public static int SolveXorEquation(int[][] a)
        {
            var n = a.Length;
            int c = 1, r = 1;
            for (; c <= n; c++)
            {
                var t = r;
                if (a[r][c] != 1)
                {
                    for (var i = r + 1; i <= n; i++)
                    {
                        if (a[i][c] != 0)
                            t = i;
                    }
                }
                
                if(a[t][c] == 0) continue;
                for (var i = c; i <= n + 1; i++)
                {
                    var tmp = a[t][i];
                    a[t][i] = a[r][i];
                    a[r][i] = tmp;
                }

                for (var i = r + 1; i <= n; i++)
                {
                    if (a[i][c] != 0)
                    {
                        for (var j = n + 1; j >= c; j--)
                            a[i][j] ^= a[r][j];
                    }
                }

                r++;
            }

            var res = 1;
            if (r < n + 1)
            {
                for (var i = r; i <= n; i++)
                {
                    if (a[i][n + 1] != 0)
                    {
                        return - 1;
                    }

                    res *= 2;
                }
            }

            return res;
        }
        
     
        /*下一个最大 - 循环版本*/
        public IEnumerable<int> NextGreaterElements(int[] nums) {
            var n = nums.Length;
            var res = Enumerable.Repeat(-1, n).ToArray();
            var stack = new Stack<int>();
            for (var i = 0; i < n * 2; i++){
                var num = nums[i % n];
                while(stack.Any() && num > nums[stack.Peek()]){
                    res[stack.Pop()] = num;
                }
                if(i < n) stack.Push(i);
            }
            
            return res;
        }
        
        /*获取所有因子*/
        public List<List<int>> GetFactors(int n)
        {
            var ans = new List<List<int>>();
            if (n == 1)
                return ans;
            Dfs(2, n, new List<int>());

            void Dfs(int index, int n, List<int> vec)
            {
                if (n == 1)
                {
                    if(vec.Count > 1)
                        ans.Add(new List<int>(vec));
                    return;
                }

                for (var i = index; i <= n; i++)
                {
                    if (n % i == 0)
                    {
                        vec.Add(i);
                        Dfs(i, n / i, vec);
                        vec.RemoveAt(vec.Count - 1);
                    }
                }
            }

            return ans;
        }
        /*数字覆盖问题,给定数组，里面的数字和覆盖指定区间至少还缺多少个数*/
        public int MinNeed(int[] arr, int range)
        {
            var needs = 0;
            var touch = 0L;
            for (var i = 0; i < arr.Length; i++)
            {
                while (arr[i] > touch + 1)
                {
                    touch += touch + 1;
                    needs++;
                    if (touch >= range)
                        return needs;
                }

                touch += arr[i];
                if (touch >= range)
                    return needs;
            }

            while (range >= touch + 1)
            {
                touch += touch + 1;
                needs++;
            }

            return needs;
        }
        /*阶乘0*/
        public int CountFactZero(int n)
        {
            var cnt = 0;
            for (var i = 5; n / i > 0; i *= 5)
            {
                cnt += n / i;
            }
            return cnt;
        }

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