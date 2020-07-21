using System;
using System.Collections.Generic;
using TokyoU.Math;

namespace TokyoU
{
    //Counter
    //DataFrame
    //Matrix
    //Tuple
    //Vector
    //调度算法
    //文件处理
    //压缩算法
    //信息论相关
    internal class Program
    {
        public static void TupleTest()
        {
            //========================Tuple============================
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>Tuple Test<<<<<<<<<<<<<<<<<<<<<<<<");
            Math.Tuple<int,double> tuple1 = new Math.Tuple<int, double>(11,10.1);
            Math.Tuple<double, double> tuple2 = new Math.Tuple<double, double>(28.7,9.1);
            Math.Tuple<int, double> tuple3 = new Math.Tuple<int, double>(8,9.1);
            Math.Tuple<double, double> tuple4 = new Math.Tuple<double, double>(33,9.1);
            List<Math.Tuple<Object,Object>> tuples = new List<Math.Tuple<object, object>>();
            tuples.Add(tuple1);
            tuples.Add(tuple2);
            tuples.Add(tuple3);
            tuples.Add(tuple4);
            Console.WriteLine(tuple1);
            tuple1.Key += 1;
            Console.WriteLine(tuple1.ConvertTo<double,int>());
            tuples.Sort();  //测试Sort()
            Console.WriteLine(Utils.ListToString(tuples));
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>Tuple Test End<<<<<<<<<<<<<<<<<<<<<<<<\n");
        }
        public static void MatrixAndVectorTest()
        {
            Vector<int> vector1 = new Vector<int>(new []{1,2,3,4,5,6,7,8});
            vector1.Delete(0);
            vector1.Insert(0,0);
            vector1.Insert(9,-1);
            Console.WriteLine(vector1._T());
            Console.WriteLine(vector1);
            Vector<double> vector2 = new Vector<double>(new []{1.1,2.2,3.3});
            Vector<int> vector3 = new Vector<int>(new []{1,2,3});
            Console.WriteLine(vector2*vector3);
            Console.WriteLine(vector2+vector3);
            Console.WriteLine(vector2-vector3);
            Console.WriteLine(vector2/vector3);
            Console.WriteLine(vector2._T()*vector3);
            Console.WriteLine(vector2.CompareTo(vector3));
            
            Console.WriteLine(((Vector<Object>)(vector2*vector3))
                .Map( ((i,o) => (dynamic)o+100)));
            Console.WriteLine(((Vector<Object>)(vector2*vector3)).L2_Distance(vector2+vector3));
            Console.WriteLine(((Vector<Object>)(vector2*vector3)).L1_Distance(vector2+vector3));
            Console.WriteLine(vector1*2.1);
            //向量维度排序
            Console.WriteLine(vector1.Sort(delegate(object obj1, object obj2)
            {
                if ((dynamic) obj1 < (dynamic) obj2)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }));
            
            Console.WriteLine(((Matrix<int>)vector1._T())._T());
            Console.WriteLine(new Matrix<int>(5,5,1));
            Console.WriteLine((Vector<int>)( ((Matrix<int>)vector1._T())));
            //子矩阵
            Matrix<int> matrix1 = new Matrix<int>(5, 5, 1).SubMatrix(1, 2, 1, 2);
            Console.WriteLine(matrix1);
            
            //Map测试
            Console.WriteLine(matrix1.Map((r, c, o) => o +" 惹-("+r+","+c+")" ));
            
            //索引子矩阵测试
            Console.WriteLine((new Matrix<int>(5, 5, 1))[2,-1,1,-1]);
            Console.WriteLine(matrix1.Multiply(matrix1 + 1.2));
        }
        
        public static void Main(string[] args)
        {
           TupleTest();
           MatrixAndVectorTest();
           
        }


    }
}