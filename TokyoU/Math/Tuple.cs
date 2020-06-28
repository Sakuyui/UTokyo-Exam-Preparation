using System;
using System.ComponentModel.Design;

namespace TokyoU.Math
{
    public class Tuple<T1,T2>
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
                    this.Val = (T2) Val;
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
    }
}