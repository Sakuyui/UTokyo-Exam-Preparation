using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace TokyoU.Math
{
    public class Vector<T>
    {
        public bool IsColumnMatrix = false;

        public List<T> Data
        {
            get;
            private set;
        }
        
        public Vector(T[] data)
        {
            this.Data = new List<T>(data);
        }

        public void Transpose()
        {
            IsColumnMatrix = !IsColumnMatrix;
        }

      
        public void DeleteAt(T val , int index = -1)
        {
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
        
        
        /* 索引器使用 */
        public T this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
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
        
    }
}