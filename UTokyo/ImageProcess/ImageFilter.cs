using System.Collections.Generic;
using System.Linq;
using UTokyo.Math;

namespace UTokyo.ImageProcess
{
    public class ImageFilter
    {

        
        public static Matrix<int> ApplyCustomConvolutionToImage(Matrix<int> image, Matrix<int> filter, 
            (int p0, int p1) padding = default, (int s0, int s1) stride = default)
        {
            
            var n = image.RowsCount;
            var m = image.ColumnsCount;
            var fh = filter.RowsCount;
            var fw = filter.ColumnsCount;
            if (stride == default)
            {
                stride = (1, 1);
            }

            if (padding == default)
            {
                padding = ((n - 1) / 2, (m - 1) / 2);
            }

            var (newH, newW) = (
                (n + 2 * padding.p0 - fh) / stride.s0 + 1,
                (m + 2 * padding.p1 - fw) / stride.s1 + 1);
            
            var matrix = new Matrix<int>(newH, newW);
            
            var matrixPadded = (Matrix<int>)image.Clone();
            matrixPadded.PrintToConsole();
            //0填充
            for (var i = 0; i < padding.p0; i++)
            {
                matrixPadded.AddARow( 0);
                matrixPadded.AddARow(matrixPadded.RowsCount);
            }
            for (var i = 0; i < padding.p1; i++)
            {
                matrixPadded.AddColumn( 0);
                matrixPadded.AddColumn(matrixPadded.ColumnsCount);
            }
            matrixPadded.PrintToConsole();
            for (var i = 0; i < newH; i += stride.s0)
            {
                for (var j = 0; j < newW; j += stride.s1)
                {
                    var sum = 0;
                    var beginX = i;
                    var beginY = j;
                    for (var u = 0; u < fh; u++)
                    {
                        for (var v = 0; v < fw; v++)
                        {
                            sum += filter[u, v] * matrixPadded[beginX + u,beginY + v];
                        }
                    }
                    matrix[i, j] = sum;
                }
            }
            var t = (Matrix<double>)(matrix / filter.ElementEnumerator.Select(e => (double)e).Sum());
            
            return matrix;
        }
        
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