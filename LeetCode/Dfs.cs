using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
    public class Dfs
    {

        public List<List<string>> KQueenSolves(int n)
        {
            var result = new List<List<int>>();
            void Dfs(IEnumerable<int> path, IEnumerable<int> xyDif, IEnumerable<int> xySum)
            {
                var p = path.Count();
                if (p == n)
                {
                    result.Add(new List<int>(path));
                    return;
                }

                foreach (var q in Enumerable.Range(0, n))
                {
                    if (!path.Contains(q) && !xyDif.Contains(p - q) && !xySum.Contains(p + q))
                    {
                        Dfs(path.Append(q), xyDif.Append(p - q), xySum.Append(p + q));
                    }
                }
            }
            Dfs(new List<int>(), new List<int>(), new List<int>());
            var t =(from sol in result
                    select (from i in sol 
                            select Enumerable.Repeat('.', i).Aggregate("", (a, b) => a + b)
                                   + 'Q' + Enumerable.Repeat('.', n - i - 1).Aggregate("", (a, b) => a + b)).ToList()
                ).ToList();
            return t;
        }
        public int KQueen(int k)
        {
            if (k >= 64)
                return -1;
            //检测冲突用
            var tate = 0L;
            var nanameL = 0L;
            var nanameR = 0L;
            //经典位运算
            // void SetBit(ref long num, int k) => num |= 1L << k;
            // void CleanBit(ref long num, int k) => num &= ~(1L << k);
            // void CleanLowBit(ref long num) => num ^= (num & -num);
            // byte GetIBit(long num, int i) => (byte) ((num >> i) & 1);
            // void ReverseIBit(ref long num, int i) => num ^= 1 << i;
            var cnt = 0;
            
            void Dfs(int row)
            {
                if (row >= k)
                {
                    cnt++;
                    return;
                }
                //求差集 with 竖 斜
                var available = ((1L << k) - 1) & ~(tate | (nanameR >> row) | (nanameL >> (k - 1 - row)));

                while (available > 0)
                {
                    var lowBit = available & -available;
                    available ^= lowBit;
                    tate |= lowBit; // set 
                    nanameR |= (lowBit << row);
                    nanameL |= lowBit << (k - 1 - row);
                    Dfs(row + 1);
                    //clean
                    tate ^= lowBit;
                    nanameR ^= (lowBit << row);
                    nanameL ^= lowBit << (k - 1 - row);
                }
            }
            Dfs(0);
            return cnt;

        }
    }
}