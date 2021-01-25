using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using TokyoU.ImageProcess;

namespace TokyoU.Math
{


    public class MatrixTest
    {
        public static void Test()
        {
            
            var matrix = new Matrix<int>(Utils.CreateTwoDimensionList(new int[12]
            {
                1,2,3,4,
                5,6,7,8,
                9,10,11,12
            },4,3));
            matrix[1][0].PrintToConsole();
            
            ("max = " + matrix.Max(e => e.Max(p => p))).PrintToConsole();
            
            //dense
            matrix.Aggregate((e1, e2) => e1.Concat(e2).ToList()).PrintEnumerationToConsole();
            
            matrix.PrintToConsole();
            matrix.Iloc(0, 1).PrintEnumerationToConsole();
            //matrix[-1,-1,0,1].PrintToConsole();
            matrix[2] -= (dynamic) new Vector<int>(1,6,2);
            matrix[1] += (dynamic) 10;
            matrix.PrintToConsole();
            matrix.Reverse180();
            matrix.PrintToConsole();
           // matrix.ColumnsEnumerator.Select(e => e.Max()).PrintEnumerationToConsole();
            //matrix.RowsEnumerator.Select(e => e.Max()).PrintEnumerationToConsole();
            
            Matrix<int> filter = new Matrix<int>(Utils.CreateTwoDimensionList(new []
            {
                1, 0, 1,
                0, 5, 0,
                1, 0, 1
            },3,3));
            //平均
           // matrix.RowsEnumerator.Select(e => e.Sum() / e.Count).PrintEnumerationToConsole();
           ImageFilter.ApplyEqualWithConvolutionToImage(matrix, filter);
        }
    }
    public class Matrix<T> : ICloneable, IEnumerable<List<T>>
    {
        public List<List<T>> Datas;
        public int RowsCount => Datas?.Count ?? 0;

        public int ColumnsSize = 0; //列数
        public int RowSize = 0; //行数

        private Tuple<int, int> Shape => new Tuple<int, int>(RowsCount,ColumnsCount);


        public IEnumerable<List<T>> ColumnsEnumerator
        {
            get
            {
                for(var i = 0; i <  ColumnsSize; i++)
                {
                    var col = Datas.Select(e => e[i]).ToList();
                    yield return col;
                }
            }
        }
        public IEnumerable<List<T>> RowsEnumerator
        {
            get
            {
                for(var i = 0; i <  RowSize; i++)
                {
                    yield return Datas[i];
                }
            }
        }

        public IEnumerable<T> ElementEnumerator
        {
            get { return Datas.SelectMany(r => r); }
        }
        
        
        public int ColumnsCount
        {
            get
            {
                if (Datas == null) return 0;
                return Datas.Count != 0 ? Datas[0].Count : ColumnsSize;
            }
            set => ColumnsSize = value;
        }

        public Matrix(IReadOnlyCollection<T[]> datas)
        {
            if(datas == null) throw new Exception();
            RowSize = datas.Count;
            if (RowSize == 0) ColumnsSize = 0;
            Datas = new List<List<T>>();
            foreach (var t in datas)
            {
                ColumnsSize = t.Length;
                Datas.Add(new List<T>(t));
            }
        }
        public Matrix(IReadOnlyList<List<T>> datas)
        {
            if(datas == null) throw new Exception();
            RowSize = datas.Count;
            if (RowSize == 0) ColumnsSize = 0;
            Datas = new List<List<T>>();
            foreach (var t in datas)
            {
                ColumnsSize = t.Count;
                Datas.Add(new List<T>(t.ToArray()));
            }
        }

        //从向量创建矩阵
        public Matrix(IReadOnlyList<Vector<T>> vectors)
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

            if (!vectors[0].IsColumnMatrix) return;
            var matrix = this._T();
            this.Datas = matrix.Datas;
            this.ColumnsSize = matrix.ColumnsSize;
            this.RowSize = matrix.RowSize;


        }
        public Matrix<T> _T()
        {
            var result = new Matrix<T>(ColumnsSize, RowSize);
            for (var i = 0; i < RowSize; i++)
            {
                for (var j = 0; j < ColumnsSize; j++)
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
            var list = new List<List<T>>();
            for (var i = 0; i < rows; i++)
            {
                var tmpList = new List<T>();
                for (var j = 0; j < columns; j++)
                {
                    tmpList.Add(val);
                }
                list.Add(tmpList);
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


        public IEnumerator<List<T>> GetEnumerator()
        {
            return ((IEnumerable<List<T>>) Datas).GetEnumerator();
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
            get => Datas[r][c];
            set => Datas[r][c] = value;
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
                for (var i = 0; i < rowsIndexes.Length; i++)
                {
                    Vector<T> vector = Iloc(rowsIndexes[i], rowsIndexes[i])[0];
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


        public void AddARow(int index = -1)
        {
            var newRow = new List<T>();
            for (var i = 0; i < ColumnsCount; i++)
            {
                newRow.Add(default);
            }
            AddARow(newRow.ToArray(),index);
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

        public void AddColumn(int index = -1)
        {
            var newColumn = new List<T>();
            for (var i = 0; i < RowsCount; i++)
            {
                newColumn.Add(default);
            }
            AddColumn(newColumn.ToArray(),index);
        }
        public void AddColumn(T[] column, int index = -1)
        {
            if (column.Length != this.Shape.Key)
            {
                throw new ArithmeticException();
            }
            else
            {
                var newColumn = new List<T>(column);
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
            var vectors = Iloc(rowTop, rowBottom);
            
            var indexes = new List<int>();
            for (var i = columnLeft; i <= columnRight; i++)
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
            var vector = new Vector<T>(RowsCount*ColumnsCount);
            for (var i = 0; i < RowsCount; i++)
            {
                for (var j = 0; j < ColumnsCount; j++)
                {
                    vector[i * ColumnsSize + j] = this[i, j];
                }
            }

            
            vector.IsColumnMatrix = false;
            return vector;
        }


        public void Reverse180()
        {
            var n = Shape.Key * Shape.Val;
            var mid = n >> 1;
            for (var p1 = 0; p1 < mid; p1++)
            {
                var p1X = p1 / Shape.Val;
                var p1Y = p1 % Shape.Val;
                var p2 = n - p1 - 1;
                var p2X = p2 / Shape.Val;
                var p2Y = p2 % Shape.Val;
                //swap
                var t = Datas[p1X][p1Y];
                Datas[p1X][p1Y] = Datas[p2X][p2Y];
                Datas[p2X][p2Y] = t;
            }
        }
        
        /*选择行/列*/
        //默认选择行
        public List<Vector<T>> Iloc(int from, int to, int axis = 0)
        {
            var vectors = new List<Vector<T>>();
            if(axis!=0 && axis!=1) 
                throw new ArithmeticException("Axis should be 1 or 0");
            //行选择
            if (axis == 0)
            {
                for (int i = from; i <= to; i++) {
                    //Extract a row
                    var vector = new Vector<T>(Datas[i].ToArray());
                    vector.IsColumnMatrix = false;
                    vectors.Add(vector);
                   
                }
            }
            else
            {
                for (var i = from; i <= to; i++)
                {
                    var v = new Vector<T>(RowsCount);
                    v.IsColumnMatrix = true;
                    vectors.Add(v);
                }

               
                for (var i = 0; i < RowsCount; i++) {
                    for (var j = from; j <= to; j++)
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
            if(matrix.RowsCount == 1) return  matrix.Iloc(0, 0)[0];
            return matrix.Iloc(0, 0, 1)[0];
        }


        public object Clone()
        {
            var matrix = new Matrix<T>(Datas);
            matrix.RowSize = RowSize;
            matrix.ColumnsSize = ColumnsSize;
            return matrix;
        }

        public delegate object MatrixMapFunction(int indexR,int indexC,object x);

        public Matrix<object> Map(MatrixMapFunction mapFunction)
        {
            Matrix<object> matrix = (Matrix<T>)Clone();
            for (var i = 0; i < matrix.RowsCount; i++)
            {
                for (var j = 0; j < matrix.ColumnsCount; j++)
                {
                    matrix[i, j] = mapFunction(i,j,matrix[i, j]);
                }
            }
            return matrix;
        }

        
        public static implicit operator Matrix<object>(Matrix<T> matrix)
        {
            var m = new Matrix<object>(matrix.RowsCount,matrix.ColumnsCount);
            for (var i = 0; i < (int)m.Shape[0]; i++)
            {
                for (var j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = matrix[i, j];
                }
            }

            return m;
        }
       
        public static explicit operator Matrix<T>(Matrix<object> matrix)
        {
            var m = new Matrix<T>(matrix.RowsCount,matrix.ColumnsCount);
            for (var i = 0; i < (int)m.Shape[0]; i++)
            {
                for (var j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (T)matrix[i, j];
                }
            }

            return m;
        }

        
        public static Matrix<object> operator + (Matrix<T> matrix1, Matrix<object> matrix2)
        {
            if (!matrix1.Shape.Equals(matrix2.Shape))
            {
                throw new ArithmeticException();
            }
            var m = new Matrix<object>(matrix1.RowsCount,matrix1.ColumnsCount);
            for (var i = 0; i < (int)m.Shape[0]; i++)
            {
                for (var j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)matrix1[i, j] + (dynamic)matrix2[i,j];
                }
            }

            return m;
        }
        public static Matrix<object> operator + (Matrix<T> matrix1, object val)
        {
            Matrix<Object> mat = new Matrix<object>(matrix1.Shape.Key,matrix1.Shape.Val,val);
            
            return matrix1 + mat;
        }
        public static Matrix<object> operator - (Matrix<T> matrix1, object val)
        {
            var mat = new Matrix<object>(matrix1.Shape.Key,matrix1.Shape.Val,val);
            
            return matrix1 - mat;
        }
        public static Matrix<object> operator - (Matrix<T> matrix1, Matrix<Object> matrix2)
        {
            if (!matrix1.Shape.Equals(matrix2.Shape))
            {
                throw new ArithmeticException();
            }
            var m = new Matrix<object>(matrix1.RowsCount,matrix1.ColumnsCount);
            for (int i = 0; i < (int)m.Shape[0]; i++)
            {
                for (int j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)matrix1[i, j] - (dynamic)matrix2[i,j];
                }
            }

            return m;
        }
        
        public static Matrix<object> operator * (Matrix<T> matrix1, object val)
        {
            var mat = new Matrix<object>(matrix1.Shape.Key,matrix1.Shape.Val,val);
            
            return matrix1.DotMultiply(mat);
        }
        public static Matrix<object> operator / (Matrix<T> matrix1, object val)
        {
            var m = new Matrix<Object>(matrix1.RowsCount, matrix1.ColumnsCount);
            for (var i = 0; i < (int)m.Shape[0]; i++)
            {
                for (var j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)matrix1[i, j] / (dynamic)val;
                }
            }

            return m;
        }
        public static Matrix<object> operator * (Matrix<T> matrix1, Matrix<object> matrix2)
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
                    vec = vec + matrix2.Iloc(j,j , 0)[0] * matrix1[i,j] ;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Matrix<T>))
            {
                Matrix<T> matrix = (Matrix<T>) obj;
                if (matrix.ColumnsCount != ColumnsCount || matrix.RowsCount != RowsCount) return false;
                for (var i = 0; i < RowsCount; i++)
                {
                    for (var j = 0; j < ColumnsCount; j++)
                    {
                        if (!this[i, j].Equals(matrix[i, j])) return false;
                    }
                }
                return true;
            }
            
            return false;
        }

       

        public Matrix<object> BlockMultiPly(Matrix<object> mat, int blockSize = 16)
        {
            if (!Shape[1].Equals(mat.Shape[0]) || blockSize <= 0)
            {
                throw new ArithmeticException();
            }
            var m = new Matrix<Object>(RowsCount, ColumnsCount);

            for (var i = 0; i < RowsCount; i += blockSize)
            {
                for (var j = 0; j < ColumnsSize; j += blockSize)
                {
                    for (var k = 0; k < (int) mat.Shape[1]; k += blockSize)
                    {
                        for (var x = 0; x < blockSize && x < (int) Shape[1]; x++)
                        {
                            for (var y = 0; y < blockSize && y < (int) Shape[0]; y++)
                            {
                                for (var z = 0; z < blockSize && z < (int) mat.Shape[1]; z++)
                                {
                                    var foo = (dynamic)this[x,y] * mat[y,z];
                                    m[x, y] = m[x, y] + foo;
                                }
                            }
                        }
                    }
                }
            }

            return m;
        }
        
        
        public Matrix<object> DotMultiply(Matrix<object> matrix)
        {
            if (!matrix.Shape.Equals(Shape))
            {
                throw new ArithmeticException();
            }
            var m = new Matrix<Object>(RowsCount,ColumnsCount);
            for (var i = 0; i < (int)m.Shape[0]; i++)
            {
                for (var j = 0; j < (int) m.Shape[1]; j++)
                {
                    m[i, j] = (dynamic)this[i, j] * (dynamic)matrix[i,j];
                }
            }

            return m;
        }
      

        public Matrix<T> Reshape(Tuple<int, int> shape)
        {
            //为了效率更高可以用内存操作
            if (shape != this.Shape) throw new ArithmeticException();
            var rs = (int) shape[0];
            var cs = (int) shape[1];
            var matrix = new Matrix<T>(rs,cs);
            for (var i = 0; i < rs; i++)
            {
                for (var j = 0; j < cs; j++)
                {
                    var pos = i * rs + j;
                    matrix[i, j] = this[pos / ColumnsCount, pos % ColumnsCount];
                }
            }

            return matrix;
        }


        public Vector<T> this[int row]
        {
            get
            {
                return new Vector<T>( Datas[row].ToArray());
            }
            set
            {
                Datas[row] = value.ToList();
            }
        } 
    }
}