using System.Collections.Generic;
using System.Linq;
using UTokyo.Structure.Graph;

namespace UTokyo.Praticle
{
    public class Test1
    {
        /*Maximum profit*/
        public static int MaximumProfit(int[] r)
        {
            if(r.Length < 2)
                return 0;
            var min = r[0];
            var max = 0;
            for (var i = 1; i < r.Length; i++)
            {
                max = System.Math.Max(max, r[i] - min);
                min = System.Math.Min(min, r[i]);
            }

            return max;
        }

        public void Swap<T>(T[] values, int i, int j)
        {
            var t = values[i];
            values[i] = values[j];
            values[j] = t;
        }







        public void Merge(int[] nums, int s, int m, int e)
        {
            var tmp = new List<int>();
            var l = s;
            var r = m + 1;
            while (l <= m && r <= e)
            {
                if(nums[l] <= nums[r])
                    tmp.Add(nums[l++]);
                else
                    tmp.Add(nums[r++]);
            }
            while(l <= m)
                tmp.Add(nums[l++]);
            while(r <= e)
                tmp.Add(nums[r++]);
            for (var i = 0; i < tmp.Count; i++)
            {
                nums[s + i] = tmp[i];
            }
            
        }
    }

    public class Test2
    {

        // ax + by === gcd(x , y) , b = p的话能求a mod p的逆元
        public int ExtGcd(int a, int b, out int outX, out int outY)
        {
            if (b == 0)
            {
                outX = 1;
                outY = 0;
                return a;
            }

            var ans = ExtGcd(b, a % b, out outX, out outY);
            var tmp = outX;
            //新解
            outX = outY;
            outY = tmp - a / b * outY;
            return ans;
        }

        
        
        
        

    }


   
}