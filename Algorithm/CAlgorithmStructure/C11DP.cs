using System;

namespace Algorithm
{
    public class C11DP
    {
        /*
         * 通过DP计算矩阵链式乘法的代价。遍历括号化数量复杂度太高
         * Input : m:矩阵的维度 M_i是 (p[i - 1,p[i])维度的)
         */
        public int MatrixMultiplyCost(int[] p)
        {
            var n = p.Length - 1;
            var dp = new int[n + 1, n + 1];
            //dp[i,j]表示M_i乘到M_j的代价
            //在i ~ j之间的k处画括号的最小代价
            //dp[i,j] = min_k{m[i, k] + m[k + 1, j] + p[i - 1]p[k]p[j]
            //从上到下从左到右
            for (var l = 2; l <= n; l++)
            {
                for (var i = 1; i <= n - l + 1; i++)
                {
                    var j = i + l - 1;
                    dp[i,j] = int.MaxValue;
                    for (var k = i; k < j; k++)
                    {
                        dp[i, j] = Math.Min(dp[i, j], dp[i, k] + dp[k + 1, j] + p[i - 1] * p[k] * p[j]);
                    }
                }
            }
            
            return dp[n, n];
        }
    }
}