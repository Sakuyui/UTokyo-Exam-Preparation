using System.Collections.Generic;
using System.Linq;
using UTokyo.Math;

namespace UTokyo.ImageProcess
{
    public enum ConnectedType
    {
        EightConnect,
        FourConnectLrud
    }
    public static class ImageProcessUtils
    {
        public static List<List<(int x, int y)>> GetConnectedArea(GreyImage image, double threshHold)
        {
            var mat = (Matrix<double>)image.PixelMatrix.Clone();
            var zeroOneImage =
                mat.ElementEnumerator.Select(e => e >= threshHold ? 1 : 0)
                    .ToArray().ToMatrix(mat.RowsCount, mat.ColumnsCount);
            zeroOneImage.PrintToConsole();
            var ans = new List<List<(int x, int y)>>();
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    if (zeroOneImage[i, j] != 1)
                    {
                        var tmpArea = new HashSet<Vector<int>>();
                        GetConnectedAreaDfs(zeroOneImage, i, j, tmpArea);
                        ans.Add(tmpArea.Select(e => (e[1], e[2])).ToList());
                        foreach (var a in ans)
                        {
                            a.PrintEnumerationToConsole();
                        }
                    }
                }
            }
            return ans;
        }

        private static void GetConnectedAreaDfs(Matrix<int> matrix, int x, int y, ISet<Vector<int>> path)
        {
            //默认上下左右dfs
            var h = matrix.RowsCount;
            var w = matrix.ColumnsCount;
            var direction = new[]
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };
            
            if(y >= w || x >= h || x < 0 || y < 0)
                return;
            if (matrix[x, y] != 1)
            {
                matrix[x, y] = 1;
                path.Add(new Vector<int>(2, x, y));
                foreach (var d in direction)
                {
                    GetConnectedAreaDfs(matrix, x + d.Item1, y + d.Item2, path);
                }
            }
        }
       
    }
}