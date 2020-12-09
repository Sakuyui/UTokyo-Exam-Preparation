using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace TokyoU.Math
{
    public class DataFrame
    {
        
        private List<string> _columnNames;
        public List<string> ColumnNames
        {
            get => _columnNames;
        }
        public List<Serial> Serials = new List<Serial>();

        public Dictionary<string, object> MappingDict
        {
            get
            {
                var map = new Dictionary<string, object>();
                foreach (var k in _columnNames)
                {
                    map[k] = null;
                }
                return map;
            }
        }
        
        //index
        public DataFrame(IEnumerable<string> columnNames)
        {
            var enumerable = columnNames as string[] ?? columnNames.ToArray();
            if (enumerable.GroupBy(g => g).Where(s => s.Count() > 1).ToList().Count > 0)
            {
                throw new Exception("DataFrame can't have more than one columns with the same name.");
            }
            _columnNames = new List<string>(enumerable);
        }


        public void AddNewRow(List<object> data)
        {
            if(data.Count != _columnNames.Count) throw new ArithmeticException();
            var m = MappingDict;
            var index = 0;
            foreach (var c in _columnNames)
            {
                m[c] = data[index++];
            }
            Serials.Add(new Serial(m));
        }

        public void AddNewRow(Dictionary<string, object> map)
        {
            var set = new HashSet<string>(_columnNames);
            var s = new Serial(map.Keys);
            
            foreach (var k in map.Keys)
            {
                if (set.Contains(k))
                {
                    s[k] = map[k];
                }
                else
                {
                    //出现新列
                    s[k] = map[k];
                    AddNewColumn(k);
                }
            }
            Serials.Add(s);
        }

        public void AddNewRow(Serial s)
        {
            var d = s.Tuples;
            var map = new Dictionary<string, object>();
            foreach (var t in d)
            {
                map[t.Key] = t.Val;
            }
        }
        public void AddNewColumn(string cName, object fill = null)
        {
            foreach (var s in Serials)
            {
                s[cName] = fill;
            }
        }

        public Serial this[int index]
        {
            get => (index < Serials.Count) ? Serials[index] : null;
            set
            {
                var keys1 = new HashSet<string>(value.ColumnNames);
                var keys2 = new HashSet<string>(_columnNames);
                if (keys1.SetEquals(keys2))
                {
                    Serials[index] = value;
                }else{throw new ArithmeticException();}
            }
        }

        public DataFrame this[IEnumerable<string> columns]
        {
            get
            {
                IEnumerable<string> collection = columns as string[] ?? columns.ToArray();
                var keys1 = new HashSet<string>(collection);
                var keys2 = new HashSet<string>(_columnNames);
                var finalKeys = keys1.Intersect(keys2);
                var df = new DataFrame(finalKeys);
                foreach (var s in Serials)
                {
                    df.AddNewRow(s[collection]);
                }

                return df;
            }
        }
    }

    public class Serial
    {
        
        public Dictionary<string, object> DataMap {get; private set; } = new Dictionary<string, object>();
        public List<string> ColumnNames => DataMap.Keys.ToList();
        public List<object> Values => DataMap.Values.ToList();

        public List<Tuple<String, object>> Tuples => 
            (from e in DataMap.Keys select new Tuple<string, object>(e, DataMap[e])).ToList();

        public Serial(IEnumerable<string> columnNames)
        {
            foreach (var s in columnNames)
            {
                DataMap[s] = null;
            }
        }
        public Serial() { }
        public Serial(Dictionary<string, object> dictionary)
        {
            DataMap = dictionary;
        }
        public object this[string columnName]
        {
            get => DataMap[columnName];
            set => DataMap[columnName] = value;
        }

        public Serial this[IEnumerable<string> columns]
        {
            get
            {
                var s = new Serial();
                foreach (var k in columns)
                {
                    if (DataMap.Keys.Contains(k)) s[k] = DataMap[k];
                }

                return s;
            }
        }
        

    }
}