using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace TokyoU.Math
{
    public class Matrix<T> : ICloneable
    {
        public List<List<T>> Datas;
        public int RowsCount => Datas?.Count ?? 0;

        public int ColumnsSize = 0; //列数
        public int RowSize = 0; //行数

        public Tuple<int, int> Shape => new Tuple<int, int>(RowsCount,ColumnsCount);

        public int ColumnsCount
        {
            get
            {
                if (Datas == null) return 0;
                return Datas.Count != 0 ? Datas[0].Count : ColumnsSize;
            }
            set => ColumnsSize = value;
        }

        public Matrix(IReadOnlyList<T[]> datas)
        {
            if(datas == null) throw new Exception();
            RowSize = datas.Count;
            if (RowSize == 0) ColumnsSize = 0;
            Datas = new List<List<T>>();
            for (int i = 0; i < datas.Count; i++)
            {
                ColumnsSize = datas[i].Length;
                Datas.Add(new List<T>(datas[i]));
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

            var x = vectors[0].Count;
            var vectorType = vectors[0].IsColumnMatrix;
            for (var i = 1; i < vectors.Count; i++)
            {
                if(x != vectors[i].Count || vectorType!= vectors[i].IsColumnMatrix)  throw new Exception();;
               
            }
            //初始化
            this.ColumnsSize = vectors[0].Count;
            this.RowSize = vectors.Count;
            Datas = new List<List<T>>();
          
            
            //如果是行矩阵，将所有行添加
            for (int i = 0; i < RowSize; i++)
            {
                Datas.Add(new List<T>(vectors[i].Data.ToArray())); 
            }

            if (vectors[0].IsColumnMatrix)
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

        public delegate T ElementProcess(int i, int j, int e);
        public Matrix(int rows, int columns, ElementProcess initAction)
        {
            RowSize = rows;
            ColumnsSize = columns;
            List<List<T>> list = new List<List<T>>();
            for (int i = 0; i < rows; i++)
            {
                List<T> clist = new List<T>();
                for (int j = 0; j < columns; j++)
                {
                    clist.Add(initAction(i, j, 0));
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

        public Matrix<T> this[int rowFrom, int rowTo, int columnFrom, int columnTo]
        {
            get
            {
                int rf = rowFrom < 0 ? 0 : rowFrom;
                int rt = (rowTo < 0 || rowTo >= RowsCount) ? RowsCount - 1 : rowTo;
                int cf = columnFrom < 0 ? 0 : columnFrom;
                int ct = (columnTo < 0 || columnTo >= ColumnsCount) ? ColumnsCount - 1 : columnTo;
                return SubMatrix(rf, rt,cf,ct);
            }
            set
            {
                int rf = rowFrom < 0 ? 0 : rowFrom;
                int rt = (rowTo < 0 || rowTo >= RowsCount) ? RowsCount - 1 : rowTo;
                int cf = columnFrom < 0 ? 0 : columnFrom;
                int ct = (columnTo < 0 || columnTo >= ColumnsCount) ? ColumnsCount - 1 : columnTo;
                Matrix<T> mat = value;
                if (mat.RowsCount != rt - rf + 1 || mat.ColumnsCount != ct - cf + 1)
                {
                    throw new ArithmeticException(rf+","+rt+","+cf+","+ct+" not match "+mat.Shape);
                }

                for (int i = rf; i <= rt; i++)
                {
                    for (int j = cf; j <= ct; j++)
                    {
                        Datas[i][j] = mat[i - rf, j - cf];
                    }
                }
            }
        }
        //选择某些行以及某些列，索引可重复
        public Matrix<T> this[int[] rowsIndexes, int[] columnsIndexes]
        {
            get
            {
                List<Vector<T>> vectors = new List<Vector<T>>();
                for (int i = 0; i < rowsIndexes.Length; i++)
                {
                    Vector<T> vector = iloc(rowsIndexes[i], rowsIndexes[i])[0];
                    vectors.Add(vector);
                }
                //选择需要的列
                for (int i = 0; i < vectors.Count; i++)
                {
                    vectors[i] = vectors[i][columnsIndexes];
                }

                return new Matrix<T>(vectors);
            }
        }
  
        public void AddARow(T[] row, int index = -1)
        {
            if (row.Length != this.Shape.Val)
            {
                throw new ArithmeticException();
            }
            else
            {
                List<T> newRow = new List<T>(row);
                if (index < 0)
                {
                    //默认插在最尾
                    this.Datas.Add(newRow);
                }
                else
                {
                    this.Datas.Insert(index,newRow);
                }
            }
        }

        public void AddColumn(T[] column, int index = -1)
        {
            if (column.Length != this.Shape.Key)
            {
                throw new ArithmeticException();
            }
            else
            {
                List<T> newColumn = new List<T>(column);
                if (index < 0)
                {
                    //默认插在最尾
                    for (int i = 0; i < RowsCount; i++)
                    {
                        Datas[i].Add(newColumn[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < RowsCount; i++)
                    {
                        Datas[i].Insert(index,newColumn[i]);
                    }
                }
            }
        }


        public Matrix<T> SubMatrix(int rowTop, int rowBottom, int columnLeft, int columnRight)
        {
            List<Vector<T>> vectors = iloc(rowTop, rowBottom);
            
            List<int> indexes = new List<int>();
            for (int i = columnLeft; i <= columnRight; i++)
            {
                indexes.Add(i);
            }

            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = vectors[i][indexes.ToArray()];
                
            }

      
            return new Matrix<T>(vectors);
        }
        public Vector<T> Dense()
        {
            Vector<T> vector = new Vector<T>(RowsCount*ColumnsCount);
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    vector[i * ColumnsSize + j] = this[i, j];
                }
            }

            
            vector.IsColumnMatrix = false;
            return vector;
        }


        
        
        /*选择行/列*/
        //默认选择行
        public List<Vector<T>> iloc(int from, int to, int axis = 0)
        {
            List<Vector<T>> vectors = new List<Vector<T>>();
            if(axis!=0 && axis!=1) 
                throw new ArithmeticException("Axis should be 1 or 0");
            //行选择
            if (axis == 0)
            {
                for (int i = from; i <= to; i++) {
                    //Extract a row
                    Vector<T> vector = new Vector<T>(Datas[i].ToArray());
                    vector.IsColumnMatrix = false;
                    vectors.Add(vector);
                   
                }
            }
            else
            {
                for (int i = from; i <= to; i++)
                {
                    Vector<T> v = new Vector<T>(RowsCount);
                    v.IsColumnMatrix = true;
                    vectors.Add(v);
                }

               
                for (int i = 0; i < RowsCount; i++) {
                    for (int j = from; j <= to; j++)
                    {
                        vectors[j-from][i] = Datas[i][j];
                    }
                }
            }

            return vectors;
        }
        /*类型转换*/
        public static  explicit operator T(Matrix<T> matrix)
        {
            if (matrix.RowsCount != 1 || matrix.ColumnsCount != 1 ) 
                throw new ArithmeticException("Matrix shape != (1,1) when trying to covert to a scalar");
            return matrix[0,0];
        }

        public static explicit operator Vector<T>(Matrix<T> matrix)
        {
            if (matrix.RowsCount != 1 && matrix.ColumnsCount != 1)
            {
                throw new ArithmeticException("Matrix shape is" + matrix.Shape +" which shold be (N,1) or (1,N)");
            }
            if(matrix.RowsCount == 1) return  matrix.iloc(0, 0)[0];
            return matrix.iloc(0, 0, 1)[0];
        }


        public object Clone()
        {
            Matrix<T> matrix = new Matrix<T>(Datas);
            matrix.RowSize = RowSize;
            matrix.ColumnsSize = ColumnsSize;
            return matrix;
        }

        public delegate Object MatrixMapFunction(int indexR,int indexC,object x);

        public Matrix<Object> Map(MatrixMapFunction mapFunction)
        {
            Matrix<Object> matrix = (Matrix<T>)Clone();
            for (int i = 0; i < matrix.RowsCount; i++)
            {
                for (int j = 0; j < matrix.ColumnsCount; j++)
                {
                    matrix[i, j] = mapFunction(i,j,matrix[i, j]);
                }
            }
            return matrix;
        }

        
        public static implicit operator Matrix<Object>(Matrix<T> matrix)
        {
            Matrix<Object> m = new Matrix<object>(matrix.RowsCount,matrix.ColumnsCount);
            for (int i = 0; i < (int)m.Shape[0]; i++)
            {
                for (int j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = matrix[i, j];
                }
            }

            return m;
        }
       
        public static explicit operator Matrix<T>(Matrix<Object> matrix)
        {
            Matrix<T> m = new Matrix<T>(matrix.RowsCount,matrix.ColumnsCount);
            for (int i = 0; i < (int)m.Shape[0]; i++)
            {
                for (int j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (T)matrix[i, j];
                }
            }

            return m;
        }

        
        public static Matrix<Object> operator + (Matrix<T> matrix1, Matrix<Object> matrix2)
        {
            if (!matrix1.Shape.Equals(matrix2.Shape))
            {
                throw new ArithmeticException();
            }
            Matrix<Object> m = new Matrix<Object>(matrix1.RowsCount,matrix1.ColumnsCount);
            for (int i = 0; i < (int)m.Shape[0]; i++)
            {
                for (int j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)matrix1[i, j] + (dynamic)matrix2[i,j];
                }
            }

            return m;
        }
        public static Matrix<Object> operator + (Matrix<T> matrix1, Object val)
        {
            Matrix<Object> mat = new Matrix<object>(matrix1.Shape.Key,matrix1.Shape.Val,val);
            
            return matrix1 + mat;
        }
        public static Matrix<Object> operator - (Matrix<T> matrix1, Matrix<Object> matrix2)
        {
            if (!matrix1.Shape.Equals(matrix2.Shape))
            {
                throw new ArithmeticException();
            }
            Matrix<Object> m = new Matrix<Object>(matrix1.RowsCount,matrix1.ColumnsCount);
            for (int i = 0; i < (int)m.Shape[0]; i++)
            {
                for (int j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)matrix1[i, j] - (dynamic)matrix2[i,j];
                }
            }

            return m;
        }
        
        
        public static Matrix<Object> operator * (Matrix<T> matrix1, Matrix<Object> matrix2)
        {
            if (!matrix1.Shape.Val.Equals(matrix2.Shape.Key))
            {
                throw new ArithmeticException(""+matrix1.Shape+"*"+matrix2.Shape);
            }
           
            Matrix<T> m = new Matrix<T>(matrix1.RowsCount,matrix2.ColumnsCount);
            for (int i = 0; i < (int)matrix1.Shape[0]; i++)
            {
                Vector<Object> vec = new Vector<T>(matrix2.ColumnsCount); 
                for (int j = 0; j < (int) matrix1.Shape[1]; j++)
                {
                    vec = vec + matrix2.iloc(j,j , 0)[0] * matrix1[i,j] ;
                }

                m[i, i, -1, -1] = ((Matrix<T>)(Matrix<Object>)vec)._T();
            }

            return m;
        }
        
        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    hash += this[i, j].GetHashCode() + i + j;
                }
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Matrix<T>))
            {
                Matrix<T> matrix = (Matrix<T>) obj;
                if (matrix.ColumnsCount != ColumnsCount || matrix.RowsCount != RowsCount) return false;
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if (!this[i, j].Equals(matrix[i, j])) return false;
                    }
                }
                return true;
            }
            
            return false;
        }

        //默认在整个矩阵范围找最大值
        // -1:整个矩阵 0:行 1:列
        public Vector<T> Max(int axis = -1)
        {
            if (ColumnsCount == 0 || RowsCount == 0) return null;
            if (axis == -1)
            {
                T max = this[0, 0];
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if ((dynamic)this[i, j] > (dynamic)max)
                        {
                            max = this[i, j];
                        }
                    }
                }

                return (Vector<T>)max;
            }
            if(axis == 0) //行上寻找最大
            {
                Vector<T> maxes = new Vector<T>(ColumnsCount);
                maxes.IsColumnMatrix = true;
                for (int i = 0; i < RowsCount; i++)
                {
                    T max = this[i, 0];
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if ((dynamic)this[i, j] > (dynamic)max)
                        {
                            max = this[i, j];
                        }
                    }

                    maxes[i] = max;
                }

                return maxes;
            }
            else
            {
                Vector<T> maxes = new Vector<T>(ColumnsCount);
                maxes.IsColumnMatrix = false;
                for (int j = 0; j < ColumnsCount; j++)
                {
                    maxes[j] = this[0, j];
                }
                for (int i = 1; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if ((dynamic)this[i, j] > (dynamic)maxes[j])
                        {
                            maxes[j] = this[i, j];
                        }
                    }
                }
                return maxes;
            }
        }

        public Matrix<Object> Multiply(Matrix<Object> matrix)
        {
            if (!matrix.Shape.Equals(Shape))
            {
                throw new ArithmeticException();
            }
            Matrix<Object> m = new Matrix<Object>(RowsCount,ColumnsCount);
            for (int i = 0; i < (int)m.Shape[0]; i++)
            {
                for (int j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)this[i, j] * (dynamic)matrix[i,j];
                }
            }

            return m;
        }
        public Vector<T> Min(int axis = -1)
        {
            if (ColumnsCount == 0 || RowsCount == 0) return null;
            if (axis == -1)
            {
                T max = this[0, 0];
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if ((dynamic)this[i, j] < (dynamic)max)
                        {
                            max = this[i, j];
                        }
                    }
                }

                return (Vector<T>)max;
            }
            if(axis == 0) //行上寻找最大
            {
                Vector<T> maxes = new Vector<T>(ColumnsCount);
                maxes.IsColumnMatrix = true;
                for (int i = 0; i < RowsCount; i++)
                {
                    T max = this[i, 0];
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if ((dynamic)this[i, j] < (dynamic)max)
                        {
                            max = this[i, j];
                        }
                    }

                    maxes[i] = max;
                }

                return maxes;
            }
            else
            {
                Vector<T> maxes = new Vector<T>(ColumnsCount);
                maxes.IsColumnMatrix = false;
                for (int j = 0; j < ColumnsCount; j++)
                {
                    maxes[j] = this[0, j];
                }
                for (int i = 1; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if ((dynamic)this[i, j] < (dynamic)maxes[j])
                        {
                            maxes[j] = this[i, j];
                        }
                    }
                }
                return maxes;
            }
        }
        
        public Vector<Object> Avg(int axis = -1)
        {
            if (ColumnsCount == 0 || RowsCount == 0) return null;
            if (axis == -1)
            {
                dynamic sum = this[0, 0];
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        sum = sum + (dynamic) this[i, j];
                    }
                }

                return (Vector<Object>)(sum/((float)(ColumnsCount*RowsCount)));
            }
            if(axis == 0) 
            {
                Vector<Object> avgs = new Vector<T>(ColumnsCount);
                List<Vector<T>> list = iloc(0, RowsCount - 1);
                int i = 0;
                foreach (var v in list)
                {
                    avgs[i] = v.Avg();
                    i++;
                }
                avgs.IsColumnMatrix = true;
                return avgs;
            }
            else
            {
                Vector<Object> avgs = new Vector<T>(ColumnsCount);
                List<Vector<T>> list = iloc(0, RowsCount - 1,1);
                int i = 0;
                foreach (var v in list)
                {
                    avgs[i] = v.Avg();
                    i++;
                }

                avgs.IsColumnMatrix = false;
                return avgs;
            }
        }

        public Matrix<T> Reshape(Tuple<int, int> shape)
        {
            //为了效率更高可以用内存操作
            if (shape != this.Shape) throw new ArithmeticException();
            int rs = (int) shape[0];
            int cs = (int) shape[1];
            Matrix<T> matrix = new Matrix<T>(rs,cs);
            for (int i = 0; i < rs; i++)
            {
                for (int j = 0; j < cs; j++)
                {
                    int pos = i * rs + j;
                    matrix[i, j] = this[pos / ColumnsCount, pos % ColumnsCount];
                }
            }

            return matrix;
        }
    }
}