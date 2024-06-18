using System;
using System.Linq;
using Microsoft.SolverFoundation.Solvers;

namespace LeetCode
{
    public class StringA
    {
        public static bool HasStringRepeat(string a)
        {
            return (a + a).Skip(1).SkipLast(1).Aggregate("",(a, b) => a + b).IndexOf(a, StringComparison.Ordinal) >= 0;
        }
        public bool IsInterleave(string s1, string s2, string s3) {
            int n = s1.Length, m = s2.Length, t = s3.Length;
            
            if (n + m != t) {
                return false;
            }

            var f = new bool[n + 1, m + 1];

            f[0, 0] = true;
            for (var i = 0; i <= n; ++i) {
                for (var j = 0; j <= m; ++j) {
                    var p = i + j - 1;
                    if (i > 0) {
                        f[i, j] = f[i, j] || f[i - 1, j] && s1[i - 1] == s3[p];
                    }
                    if (j > 0) {
                        f[i, j] = f[i, j] || f[i, j - 1] && s2[j - 1] == s3[p];
                    }
                }
            }

            return f[n, m];
        }

    
    }
}