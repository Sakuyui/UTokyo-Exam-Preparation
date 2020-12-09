using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
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
    //位运算。比如bitmap
    //信息论相关
    //图像相关，分割区域，区域填充等。
    /// <summary>
    /// 像素、近邻、边界、区域操作
    /// </summary>
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
            Console.WriteLine((new Matrix<int>(5, 5, 1))[1,-1,1,-1]);
            Console.WriteLine(matrix1.Multiply(matrix1 + 1.2));
        }


        public static void CounterTest()
        {
            List<int> list = new List<int>(new int[]{11,22,33,44,44,11,22,44});
            
            var counter = new Counter<int[],int>(new int[]{11,22,33,44,44,11,22,44},null);
            counter.DataList.Sort((tuple, tuple1) => tuple.Val - tuple1.Val);
            Console.WriteLine(Utils.HashMapToString(counter.CountTable));
            Console.WriteLine(Utils.ListToString(counter.DataList));

            
            
            
            //快速转换!!
            var a = counter.DataList.ConvertAll((input => (Vector<int>) input));
            
           
            
            
            Console.WriteLine(Utils.ListToString(a));


            var sum = counter.DataList.Sum(p => p.Val);
            //Linq!!
            Matrix<int> matrix = new Matrix<int>((from e in counter.DataList
                    orderby e.Key
                    select ((Vector<int>) new Math.Tuple<int, int>(e.Key, e.Val / sum)).Data
                ).ToList());
             
            
            
            
            var obj1 = (object) 3.25;
            var obj2 = (object) new Matrix<int>(3,2, (i, j, e) => i + j);
            //dynamic
            var objSum = (Matrix<double>)((dynamic) obj2 + obj1);   //コンパイル可能
            Console.WriteLine(objSum);
            Console.WriteLine(matrix);
            

        }
        public static void Main(string[] args)
        {
            //AlgorithmP.GetTopK(new []{25,36,4,55,71,18,0,71,89,65},3);
            //AdvanceStructureTest.CacheTest();
            //AdvanceStructureTest.DictionaryTest();    
            //CounterTest();
            AdvanceStructureTest.IndexKeepTableTest();
            return;
            TupleTest();
            MatrixAndVectorTest();
           
        }


    }
}