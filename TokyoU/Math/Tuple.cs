using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace TokyoU.Math
{
    public class Tuple<T1,T2> : ICloneable,IComparable<Tuple<Object,Object>>
    {
        public T1 Key;
        public T2 Val;

        public Tuple(T1 key, T2 val)
        {
            this.Key = key;
            this.Val = val;
        }

        public Tuple()
        {
            Key = default(T1);
            Val = default(T2);
        }

        public Object this[int index]
        {
            get
            {
                if (index == 0) return Key;
                return Val;
            }
            set
            {
                if (index == 0) this.Key = (T1)value;
                else
                {
                    this.Val = (T2)value;
                }
            }
        }

        public Tuple<T3, T4> ConvertTo<T3, T4>()
        {
            Tuple<T3,T4> tuple = new Tuple<T3, T4>();
            tuple.Key = (T3)((dynamic)this.Key);
            tuple.Val = (T4) ((dynamic) this.Val);
            return tuple;
        }

        public override string ToString()
        {
            return "(" + Key.ToString() + "," + Val.ToString() + ")";
        }

        public Object Clone()
        {
            Tuple<T1,T2> tuple = new Tuple<T1, T2>();
            if (this.Key.GetType().IsSubclassOf(typeof(ICloneable)))
            {
                tuple.Key = (T1)((ICloneable) Key).Clone();
            }
            else
            {
                tuple.Key = Key;
            }

            if (this.Val.GetType().IsSubclassOf(typeof(ICloneable)))
            {
                tuple.Val = (T2) ((ICloneable) Val).Clone();
            }
            else
            {
                tuple.Val = Val;
            }

            return tuple;
        }


        public delegate int TupleCompareStrategy(Tuple<T1,T2> tuple1,Tuple<Object, Object> tuple2);
        public static TupleCompareStrategy DefaultTupleCompStrategy = delegate(Tuple<T1, T2> tuple1,Tuple<object, object> tuple2)
        {
            if ((dynamic)tuple1.Key < (dynamic)tuple2.Key)
            {
                return -1;
            }
            if((dynamic)tuple1.Key > (dynamic)tuple2.Key)
            {
                return 1;
            }

            if ((dynamic) tuple1.Val < (dynamic) tuple2.Val)
            {
                return -1;
            }
            if ((dynamic) tuple1.Val > (dynamic) tuple2.Val)
            {
                return 1;
            }

            return 0;
        };

        public static TupleCompareStrategy CompareStrategy = DefaultTupleCompStrategy; 
        public int CompareTo(Tuple<object, object> other)
        {
            return CompareStrategy(this, other);
        }
        
        public static implicit operator Tuple<Object,Object>(Tuple<T1, T2> tuple)
        {
            Tuple<Object,Object> newTuple = new Tuple<object, object>();
            newTuple.Key = tuple.Key;
            newTuple.Val = tuple.Val;
            return newTuple;
        }

        public static explicit operator Vector<T1>(Tuple<T1, T2> tuple)
        {
            T1[] arr = new[] {(T1)tuple.Key, (T1)(dynamic)tuple.Val};
            return new Vector<T1>(arr);
        }
        
        //为Tuple生成Hash表
        public static Hashtable CreateHashMap(List<Tuple<Object, Object>> tuples)
        {
            Hashtable hashtable = new Hashtable();
            foreach (var t in tuples)
            {
                hashtable.Add(t.Key,t);
            }
            
            return hashtable;
        }
    }
}