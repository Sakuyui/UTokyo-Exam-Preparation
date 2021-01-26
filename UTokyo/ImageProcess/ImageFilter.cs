using System.Collections.Generic;
using System.Linq;
using UTokyo.Math;

namespace UTokyo.ImageProcess
{
    public class ImageFilter
    {
        
        public static Matrix<int> ApplyEqualWithConvolutionToImage(Matrix<int> image, Matrix<int> filter)
        {
            var imgH = image.RowsCount;
            var imgW = image.ColumnsCount;
            var fH = filter.RowsCount;
            var fW = filter.ColumnsCount;
            var fillZeroH = fH >> 1;
            var fillZeroW = fW >> 1;
            var imgCopy = (Matrix<int>)image.Clone();
            for (var i = 0; i < fillZeroH; i++)
            {
                imgCopy.AddARow(0);
                imgCopy.AddARow(imgCopy.RowsCount);
            }
            for (var i = 0; i < fillZeroW; i++)
            {
                imgCopy.AddColumn(0);
                imgCopy.AddColumn(imgCopy.ColumnsCount);
            }
            filter.PrintToConsole();
            imgCopy.PrintToConsole();
            var newH = imgH + 2 * fillZeroH - fH + 1;
            var newW = imgW + 2 * fillZeroW - fW + 1;
            var res = new Matrix<int>(newH, newW);
            for (var i = 0; i < newH; i++)
            {
                for (var j = 0; j < newW; j++)
                {
                    var sum = 0;
                    var beginX = i;
                    var beginY = j;
                    for (var u = 0; u < fH; u++)
                    {
                        for (var v = 0; v < fW; v++)
                        {
                            sum += filter[u, v] * imgCopy[beginX + u,beginY + v];
                        }
                    }
                    res[i, j] = sum;
                }
            }

            var t = (Matrix<double>)(res / filter.ElementEnumerator.Select(e => (double)e).Sum());
            res.PrintToConsole();
            t.PrintToConsole();
            return res;
        }
    }
}