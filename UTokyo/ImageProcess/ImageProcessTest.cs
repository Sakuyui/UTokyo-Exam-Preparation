using System;
using UTokyo.Math;

namespace UTokyo.ImageProcess
{
    public class ImageProcessTest
    {
        public static void FilterTest()
        {

            var mat1 = new double[]
            {
                56, 192, 45,
                1, 122, 44.0,
                11, 112, 45,
                125, 156, 222
            };
            var mat2 = mat1.ToMatrix(4,3);
            ImageProcessUtils.GetConnectedArea(mat2, 100);
            return;
            var matrix = new Matrix<int>(Utils.CreateTwoDimensionList(new int[12]
            {
                1,2,3,4,
                5,6,7,8,
                9,10,11,12
            },4,3));
            
            
            
            var filter = new Matrix<int>(Utils.CreateTwoDimensionList(new []
            {
                1, 0, 1,
                0, 5, 0,
                1, 0, 1
            },3,3));
            ImageFilter.ApplyCustomConvolutionToImage(matrix, filter);
            
           // ImageFilter.ApplyEqualWithConvolutionToImage(matrix, filter);
        }
    }
}