using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoU.Math
{
    public class Matrix<T>
    {
        public List<List<T>> Datas;
        public int RowsCount { 
            get {
                if (Datas == null) return 0;
                return Datas.Count;
            }
        }

        public int ColumnsSize = 0; //列数
        public int RowSize = 0; //行数

        public Tuple<int, int> Shape
        {
            get{return new Tuple<int, int>(RowsCount,ColumnsCount);}
        }
        public int ColumnsCount
        {
            get
            {
                if (Datas == null) return 0;
                if (Datas.Count != 0) return Datas[0].Count;
                return ColumnsSize;
            }
            set
            {
                ColumnsSize = value;
            }
        }

        public Matrix(List<List<T>> datas)
        {
            if(datas == null) throw new Exception();
            RowSize = datas.Count;
            if (RowSize == 0) ColumnsSize = 0;
            Datas = new List<List<T>>();
            for (int i = 0; i < datas.Count; i++)
            {
                ColumnsSize = datas[i].Count;
                Datas.Add(new List<T>(datas[i].ToArray()));
            }
        }

        //从向量创建矩阵
        public Matrix(List<Vector<T>> vectors)
        {
            //Check
            //If all the shape of the vectors are the same. And if all vector is row/column vector.
            if (vectors == null) throw new Exception();
            if (vectors.Count == 0)
            {
                RowSize = 0;
                ColumnsSize = 0;
                Datas = new List<List<T>>();
                return;
            }

            if (vectors.Count == 1)
            {
                Matrix<T> m = (Matrix<T>) vectors[0];
                this.Datas = m.Datas;
                this.ColumnsSize = m.ColumnsSize;
                this.RowSize = m.RowSize;
                return;
            }

            int x = vectors[0].Count;
            bool sameVectorType = vectors[0].IsColumnMatrix;
            for (int i = 1; i < vectors.Count; i++)
            {
                x = x ^ vectors[i].Count;
                sameVectorType = sameVectorType ^ vectors[i].IsColumnMatrix;
                if (x != 0 || sameVectorType) throw new Exception();
            }
            //初始化
            this.ColumnsSize = vectors[0].Count;
            this.RowSize = vectors.Count;
            Datas = new List<List<T>>();
          
            for (int i = 0; i < RowSize; i++)
            {
                Datas.Add(new List<T>(vectors[i].Data.ToArray())); 
            }

            if (!vectors[0].IsColumnMatrix)
            {
                Matrix<T> matrix = this._T();
                this.Datas = matrix.Datas;
                this.ColumnsSize = matrix.ColumnsSize;
                this.RowSize = matrix.RowSize;
            }
            
            
        }
        public Matrix<T> _T()
        {
            Matrix<T> result = new Matrix<T>(ColumnsSize, RowSize);
            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColumnsSize; j++)
                {
                    //应该按行读取，效率高,写入不走cache无所谓
                    result[j, i] = Datas[i][j];
                }
            }

            return result;
        }
        public Matrix(int rows, int columns, T val = default(T))
        {
            RowSize = rows;
            ColumnsSize = columns;
            List<List<T>> list = new List<List<T>>();
            for (int i = 0; i < rows; i++)
            {
                List<T> clist = new List<T>();
                for (int j = 0; j < columns; j++)
                {
                    clist.Add(val);
                }
                list.Add(clist);
            }

            Datas = list;
        }

        public override string ToString()
        {
            var str = "|";
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    str += " " + Datas[i][j] + "\t , ";
                }

                str = str.Substring(0, str.Length - 2) +"|\n|";
            }

            str = str.Substring(0, str.Length - 1) + Shape +'\n';
            return str;
        }
        
        
        //索引器
        public T this[int r, int c]
        {
            get { return Datas[r][c]; }
            set { Datas[r][c] = value; }
        }
        //选择一列，选择一行等..

        public void AddARow(T[] row, int index = -1)
        {
            
        }
       
    }
}