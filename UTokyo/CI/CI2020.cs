using System.Linq;
using UTokyo.Math;

namespace UTokyo.CI
{
    public class CI2020
    {
        public static void Test()
        {
            //输入: 正方形方块大小集合
            var input = "113142421231".Select(e => e - '0').ToList();
            //输入 masu：图形大小
            const int masu = 10;
            
            var matrix = new Matrix<int>(10, 10);
            foreach (var c in input)
            {
                //Linq寻找第一个合法行
                var (px, firstLine) 
                    = matrix.FindFirst((e ,i) => {
                        var l = e.IndexOf(0);
                        var r = l + c;
                        
                        //不能超出边界，并且指定大小子矩阵内元素都要是0
                        var isInEdge = r <= 10 && l >= 0;
                        var isNoCollision = matrix[i, i + c - 1, l, r - 1].
                            ElementEnumerator.All(element => element == 0);
                        return isInEdge && isNoCollision; 
                    });
                
                var py = firstLine.IndexOf(0);
                
                //合法的能够开始放砖块的x, y坐标
                (px, py).PrintToConsole();
                //放置砖块，填充子矩阵
                matrix[px, px + c - 1, py, py + c - 1] = new Matrix<int>(c, c , 1);
            }
            var ans = matrix.FindFirst((line, i) => line.All(e => e == 0));
            (input.ToEnumerationString() + "深度为 " + ans.Item1).PrintToConsole();
        }
    }
}