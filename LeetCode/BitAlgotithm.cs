using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
    public class BitAlgotithm
    {
        //下一个数
        IEnumerable<int> FindClosedNumbers(int num) {
            static int NextOne(int x){
                long lowBit = x & (-x);
                var toZero = x + lowBit;
                return (int)((x & ~toZero) / lowBit >> 1 | toZero);
            }
            int mx = NextOne(num), mi = ~NextOne(~num);
            return new int[]{mx > 0 ? mx : -1, mi > 0 ? mi : -1};
        }

        public bool IsValidSudoku(char[][] board)
        {
            // 记录某行，某位数字是否已经被摆放
            var row = new bool[9].Select(_ => new bool[9]).ToArray(); //行，数字 = 是否
            // 记录某列，某位数字是否已经被摆放
            var col = new bool[9].Select(_ => new bool[9]).ToArray();
            // 记录某 3x3 宫格内，某位数字是否已经被摆放
            var block = new bool[9].Select(_ => new bool[9]).ToArray();

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (board[i][j] == '.') 
                        continue;
                    //是数字的情况
                    var num = board[i][j] - '1';
                    var blockIndex = i / 3 * 3 + j / 3;
                    if (row[i][num] || col[j][num] || block[blockIndex][num])
                    {
                        return false;
                    }

                    row[i][num] = true;
                    col[j][num] = true;
                    block[blockIndex][num] = true;
                }
            }

            return true;
        }
    }
}