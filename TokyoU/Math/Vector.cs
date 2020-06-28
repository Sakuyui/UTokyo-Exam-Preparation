using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace TokyoU.Math
{
    public class Vector<T> : ICloneable,IComparable<Vector<Object>>
    {
        public bool IsColumnMatrix = true;

        public List<T> Data
        {
            get;
            private set;
        }
        public int Count
        {
            get { return Data.Count;}
        }
        public Vector(T[] data)
        {
            this.Data = new List<T>(data);
        }

        public Vector()
        {
        }

        public Vector(int size,T val = default(T))
        {
            this.Data = new List<T>();
            for (int i = 0; i < size; i++)
            {
                Data.Add(val);
            }
        }
        
        public Vector<T> _T()
        {
            Vector<T> vector = (Vector<T>)this.Clone();
            vector.IsColumnMatrix = !(vector.IsColumnMatrix);
           
            return vector;
        }

      
        public void Insert(T val , int index = -1)
        {
            if (Data == null) return;
            if (index > Data.Count || index < -(Data.Count - 1))
            {
                throw new IndexOutOfRangeException();
            }
            if (index >= 0)
            {
                Data.Insert(index,val);
            }
            else
            {
                Data.Insert(Data.Count + index + 1,val);
            }
        }
        
        public void Delete(int index = -1)
        {
            if (Data == null) return;
            if (index > Data.Count || index < -(Data.Count - 1))
            {
                throw new IndexOutOfRangeException();
            }
            if (index >= 0)
            {
                Data.RemoveAt(index);
            }
            else
            {
                Data.RemoveAt(Data.Count + index );
            }
        }
        /* 索引器使用 */
        public T this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public List<T> this[int left, int right]
        {
            get
            {
                if (right <= left) return new List<T>();
                List<T> list = new List<T>();
                for (int i = left; i < right; i++)
                {
                    list.Add(Data[i]);
                }

                return list;
            }
            set
            {
                if (right <= left) return;
                List<T> list = value;
                for (int i = left; i < right; i++)
                {
                    Data[i] = list[i - left];
                }
            }
        }
        
        
        
        public override string ToString()
        {
            var result = "[";
            foreach (var d in Data)
            {
                result += d.ToString() + ",";
            }
            result = result.Substring(0, result.Length - 1) +"]";
            return IsColumnMatrix?result+"^T":result;
        }

        public object Clone()
        {
            Vector<T> result = new Vector<T>(Count);
            bool isDeepCopy = typeof(T).IsSubclassOf(typeof(ICloneable));
            for (int i = 0; i < Count; i++)
            {
                result.Data[i] = (isDeepCopy ? (T)((ICloneable) Data[i]).Clone() : Data[i]);
            }

            result.IsColumnMatrix = IsColumnMatrix;
            return result;
        }

        
        /*加减乘除*/
        public static Vector<Object> operator +(Vector<T> t1, Vector<Object> t2)
        {
            if(t1.Count != t2.Count) throw new ArithmeticException("向量长度不匹配: " + t1.Count + " != " +t2.Count);
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] + (dynamic)t2[i];
            }

            return result;
        }
        public static Vector<Object> operator +(Vector<T> t1, Object t2)
        {
            
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] + (dynamic)t2;
            }

            return result;
        }
        public static Vector<Object> operator -(Vector<T> t1, Vector<Object> t2)
        {
            if(t1.Count != t2.Count) throw new ArithmeticException("向量长度不匹配: " + t1.Count + " != " +t2.Count);
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] - (dynamic)t2[i];
            }

            return result;
        }
        public static Vector<Object> operator -(Vector<T> t1, Object t2)
        {
            
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] - (dynamic)t2;
            }

            return result;
        }
        public static Vector<Object> operator /(Vector<T> t1, Vector<Object> t2)
        {
            if(t1.Count != t2.Count) throw new ArithmeticException("向量长度不匹配: " + t1.Count + " != " +t2.Count);
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] / (dynamic)t2[i];
            }

            return result;
        }
        public static Vector<Object> operator /(Vector<T> t1, Object t2)
        {
            
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] / (dynamic)t2;
            }

            return result;
        }
        public static Matrix<Object> operator *(Vector<T> t1, Vector<Object> t2)
        {
            if(t1.Count != t2.Count) throw new ArithmeticException("向量长度不匹配: " + t1.Count + " != " +t2.Count);
            
            if (t1.IsColumnMatrix ^ t2.IsColumnMatrix)
            {
                if (!t1.IsColumnMatrix && t2.IsColumnMatrix)
                {
                    Vector<Object> result = new Vector<object>(1);
                    if(t1.Count == 0) return (Matrix<T>)(new Vector<object>());
                    dynamic sum = (dynamic)t1[0]*(dynamic)t2[0];
                    for (int i = 1; i < t1.Count; i++)
                    {
                        sum = sum +  ((dynamic)t1[i]*(dynamic)t2[i]);
                        //Console.WriteLine("k="+((dynamic)t1[i]*(dynamic)t2[i]));
                    }
                    result[0] = sum;
                    return (Matrix<Object>)result;
                }
                else
                {
                    List<Vector<Object>> vectors = new List<Vector<object>>();
                    for (int i = 0; i < t1.Count; i++)
                    {
                        Vector<Object> vector = t2 * t1[i];
                        vectors.Add(vector);
                    }

                    return new Matrix<Object>(vectors);
                }
              
            }
            else
            {
                Vector<Object> result = new Vector<object>(t1.Count);
                for (int i = 0; i < t1.Count; i++)
                {
                    result[i] = (dynamic) t1[i] * (dynamic)t2[i];
                }

                return (Matrix<Object>)result;
            }
            
        }
        public static Vector<Object> operator *(Vector<T> t1, Object t2)
        {
            
            Vector<Object> result = new Vector<object>(t1.Count);
            for (int i = 0; i < t1.Count; i++)
            {
                result[i] = (dynamic) t1[i] * (dynamic)t2;
            }

            return result;
        }
        /*比较与相等*/
        public delegate int VectorCompStrategy(Vector<Object> v1, Vector<Object> v2);
        public static VectorCompStrategy DefaultCompStrategy = delegate(Vector<object> v1, Vector<object> v2)
        {
            if (v1 == null && v2 == null) return 0;
            if (v1 == null) return -1; else if (v2 == null) return 1;
            if (v1.Count != v2.Count)
            {
                return v1.Count - v2.Count;
            }

            for (int i = 0; i < v1.Count; i++)
            {
                if(v1[i].Equals(v2[i])) continue;
                if ((dynamic) v1[i] < (dynamic)v2[i]) return -1;
                return 1;
            }

            return 0;
        };
        
        public static VectorCompStrategy CompStrategy = DefaultCompStrategy;
        public int CompareTo(Vector<object> other)
        {
            return CompStrategy(this, other);
        }

        /**Convert**/
        public static implicit operator Vector<Object>(Vector<T> vector)
        {
            if (vector == null) return null;
            Vector<Object> newVec = new Vector<Object>(vector.Count);
            newVec.IsColumnMatrix = vector.IsColumnMatrix;
            for (int i = 0; i < newVec.Count; i++)
            {
                newVec[i] = vector[i];
            }
            return newVec;
        }

        public static explicit operator Vector<T>(Vector<Object> vector)
        {
            if (vector == null) return null;
            Vector<T> newVec = new Vector<T>(vector.Count);
            newVec.IsColumnMatrix = vector.IsColumnMatrix;
            for (int i = 0; i < newVec.Count; i++)
            {
                newVec[i] = (T)vector[i];
            }
            return newVec;
        }

        public Object L2_Distance(Vector<Object> vector)
        {
            Vector<Object> v1 = this - vector;
            //Console.WriteLine(v1);
            //Console.WriteLine((v1._T() * v1)[0]);
            return v1._T() * v1;
        }
        public Object L1_Distance(Vector<Object> vector)
        {
            Vector<Object> v1 = this - vector;
            
            return v1.Map((o => System.Math.Abs((dynamic) o))).Sum();
        }
        public delegate Object VectorMapFunction(T v);

        public Vector<Object> Map(VectorMapFunction vectorMapFunction)
        {
            Vector<Object> vector = (Vector<T>)this.Clone();
            if (vector == null) return null;
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] = vectorMapFunction(Data[i]);
            }

            return vector;
        }

        
        public T Sum()
        {
            if (Data.Count == 0) return default(T);
            dynamic s = Data[0];
            for (int i = 1;i<Count;i++)
            {
                s = (dynamic)s + (dynamic)Data[i];
            }
            return s;
        }
        
        public T Avg()
        {
            if (Data.Count == 0) return default(T);
            dynamic s = Data[0];
            for (int i = 1;i<Count;i++)
            {
                s = (dynamic)s + (dynamic)Data[i];
            }
            return s/Data.Count;
        }
        public T Max()
        {
            if (Data.Count == 0) return default(T);
            T max = Data[0];
            for (int i = 1;i<Count;i++)
            {
                if ((dynamic)Data[i] > (dynamic)max) max = Data[i];
            }
            return max;
        }
        public T Min()
        {
            if (Data.Count == 0) return default(T);
            T min = Data[0];
            for (int i = 1;i<Count;i++)
            {
                if ((dynamic)Data[i] > (dynamic)min) min = Data[i];
            }
            return min;
        }

        public delegate int ElementComp(Object obj1, Object obj2); 
        public T Middle(ElementComp elementComp = null)
        {
            Vector<T> vector = this.Sort(elementComp);
            if (vector.Count % 2 != 0)
            {
                return vector[vector.Data.Count / 2];
            }
            else
            {
                return ((dynamic)vector[(vector.Data.Count+1) / 2] + (dynamic)vector[(vector.Data.Count+1) / 2 - 1])/2.0;
            }
        }

        public Vector<T> Sort(ElementComp elementComp = null)
        {
            Vector<T> vector = (Vector<T>) this.Clone();
            if (elementComp != null)
            {
                vector.Data.Sort(delegate(T x, T y)
                {
                    return elementComp(x, y);
                });
            }
            else
            {
                vector.Data.Sort();
            }

            return vector;
        }

        public static explicit operator Matrix<T>(Vector<T> vector)
        {
            List<List<T>> list = new List<List<T>>();
            if (!vector.IsColumnMatrix)
            {
                list.Add(vector.Data);
            }
            else
            {
                for (int i = 0; i < vector.Count; i++)
                {
                    List<T> l = new List<T>();
                    l.Add(vector[i]);
                    list.Add(l);
                }
            }
            return new Matrix<T>(list);
        } 
    }
}