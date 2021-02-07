using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UTokyo.Structure
{
    public class DataFrame : IEnumerable<Serial>
    {
        protected Hashtable ColumIndexMap = new Hashtable();
        protected List<string> Columns;
        protected List<Serial> Data = new List<Serial>();

        public List<Serial> Lines => Data.Select(e => e).ToList();
        public void RemoveAt(int i)
        {
            Data.RemoveAt(i);
        }
        public int Length
        {
            get { return Data.Count; }
            private set { }
        }

        public int ColumnsLength
        {
            get { return Columns.Count; }
            private set { }
        }

        public DataFrame()
        {
            Columns = new List<string>();
        }

        public DataFrame(string[] columns)
        {
            Data.Clear();
            ColumIndexMap.Clear();
            for (var i = 0; i < columns.Length; i++) ColumIndexMap.Add(columns[i], i);
            Columns = new List<string>(columns);
        }

        //Get the specific row
        public Serial this[int rowIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= Data.Count) return null;
                return Data[rowIndex];
            }
            set { Data[rowIndex] = value; }
        }

        

        //get rows from specific indexes.
        public Serial[] this[int[] rowIndexes]
        {
            get
            {
                if (rowIndexes.Length == 0) return null;
                var serials = new Serial[rowIndexes.Length];
                for (var i = 0; i < rowIndexes.Length; i++)
                    if (rowIndexes[i] < Data.Count)
                        serials[i] = Data[rowIndexes[i]];
                    else
                        serials[i] = new Serial(Columns);
                return serials;
            }
            set
            {
                if (rowIndexes.Length != value.Length) throw new Exception("Length not equal.");
                //check
                for (var i = 0; i < rowIndexes.Length; i++)
                    if (rowIndexes[i] >= Data.Count)
                        throw new Exception("index out of DataFrame row's count");

                for (var i = 0; i < rowIndexes.Length; i++) Data[rowIndexes[i]] = value[i];
            }
        }

        //Set a range's rows
        public List<Serial> this[int rowFrom, int rowTo]
        {
            get { return Data.GetRange(rowFrom, rowTo); }
            set
            {
                if (rowTo - rowFrom != value.Count) throw new Exception("Length Not matching");

                Data.RemoveRange(rowFrom, rowTo - rowFrom);
                Data.InsertRange(rowFrom, value);
            }
        }

        //Return the index of a column.
        public int GetColumnIndex(string columnName)
        {
            return Columns.ToList().BinarySearch(columnName);
        }

        public void AddColumn(string name, object defaultVal = null, int pos = -1)
        {
            Columns.Insert(pos < 0 ? Columns.Count : pos, name);
            var p = -1;
            for (var i = 0; i < Columns.Count; i++)
                if (Columns[i].Equals(name))
                {
                    p = i;
                    break;
                }

            if (p < 0) return;
            ColumIndexMap.Add(name, p);
            foreach (var s in Data) s.AddConlumn(pos < 0 ? Columns.Count : pos, name, defaultVal);
        }

        public void AddRow(int pos, object[] vals)
        {
            if (vals == null) return;
            var serial = new Serial(Columns);
            for (var i = 0; i < Columns.Count; i++)
                if (i < vals.Length)
                    serial.SetVal(Columns[i], vals[i]);
                else
                    serial.SetVal(Columns[i], null);
            Data.Insert(pos, serial);
        }

        public void AddRows(int pos, Hashtable[] valsMap)
        {
            foreach (var row in valsMap)
            {
                var serial = new Serial(Columns);
                foreach (var column in Columns)
                    if (row.ContainsKey(column))
                        serial.SetVal(column, row[column]);
                    else
                        serial.SetVal(column, null);
            }
        }

        public void AddRows(int pos, List<Serial> rows)
        {
            if (rows != null) Data.InsertRange(pos, rows);
        }

        public IEnumerator<Serial> GetEnumerator()
        {
            return ((IEnumerable<Serial>) Lines).GetEnumerator();
        }

        public override string ToString()
        {
            var result = "======================================================================\n";
            //Head print
            foreach (var s in Columns) result += s + "\t | ";
            result += '\n';
            result += "-------------------------------------------------------------------------------\n";
            foreach (var serial in Data)
            {
                foreach (var s in Columns) result += serial.GetVal(s) + "\t | ";

                result += '\n';
            }

            result += '\n';
            result += "======================================================================\n";
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Serial
    {
        private readonly List<string> _columns;
        private readonly Hashtable _row = new Hashtable();

        public Serial(List<string> columns)
        {
            _columns = columns;
            foreach (var s in _columns) _row.Add(s, null);
        }

        public object this[int cindex]
        {
            get { return _row[_columns[cindex]]; }
            set { _row[_columns[cindex]] = value; }
        }
        public object this[string columnName]
        {
            get
            {
                if (_row.ContainsKey(columnName)) return _row[columnName];
                return null;
            }
            set
            {
                if (_row.ContainsKey(columnName)) _row[columnName] = value;
                else
                    _row.Add(columnName, value);
            }
        }

        public void AddConlumn(int pos, string name, object val)
        {
            _columns.Insert(pos, name);
            _row.Add(name, val);
        }

        public object GetVal(string columnName)
        {
            return _row[columnName];
        }

        public void SetVal(string columnName, object val)
        {
            if (_row.ContainsKey(columnName)) _row[columnName] = val;
        }
    }
}