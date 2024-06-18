namespace UTokyo.Praticle
{
    public class DP
    {
        /*
         * 矩阵乘法最少次数 DP[i, j] 表示M_i * M_j需要最小次数
         * d是维度矩阵 M_i 为(d[i - 1], d[i])
         */
        public static int MinMatrixMultiplyCount(int[] d)
        {
            var n = d.Length - 1;
            if (d.Length < 2)
                return 0;
            var dp = new int[n + 1, n + 1];
            for (var i = 1; i <= n; i++)
            {
                dp[i, i] = 0;
            }

            
            //上到下，左到右
            for (var l = 2; l <= n; l++)
            {
                for (var i = 1; i < n - l; i++)
                {
                    var j = i + l - 1;
                    dp[i, j] = int.MaxValue;
                    for(var k = i; k < j; k ++)
                    {
                        dp[i, j] = System.Math.Min(
                            dp[i, j],
                            dp[i, k] + dp[k + 1, j] + d[i - 1] * d[k] * d[j]
                        );
                    }
                }
            }
            return dp[n,n];
        }
    }
}