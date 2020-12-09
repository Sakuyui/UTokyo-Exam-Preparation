using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace TokyoU.Structure
{
    /*一个数据结构，可以保存元素原来的索引，即使进行相关操作*/
    public class IndexKeepTable<TS> where TS : IComparable<TS>
    {
        public List<IndexKeepNode> SourceData { get; private set; }
        public List<IndexKeepNode> CurData { get; private set; } = new List<IndexKeepNode>();
        public Dictionary<int,int> SourceCurIndexMap { get; private set; } = new Dictionary<int, int>();
        public IndexKeepTable(List<TS> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                CurData.Add(new IndexKeepNode(i,collection[i]));
                SourceData.Add(new IndexKeepNode(i,collection[i]));
            }
           
        }

        /**返回排序后的数据，以及一个映射表，现在的数据在原来数据的哪**/
        public (List<IndexKeepNode> nodes, Dictionary<int,int> sourceCurIndexMap) SortData(bool isDesc = false)
        {
            var sortedNodes =
                isDesc
                    ? (from e in SourceData orderby e descending select e).ToList()
                    : (from e in SourceData orderby e select e).ToList();
            CurData = sortedNodes;
            var map = new Dictionary<int,int>();
            for (var i = 0; i < sortedNodes.Count; i++)
            {
                SourceCurIndexMap[sortedNodes[i].SourceIndex] = i;
            }

            return (CurData, SourceCurIndexMap);
        }
        
        
        //内部类
        public class IndexKeepNode : IComparable<IndexKeepNode>
        {
            public int SourceIndex { get; private set; }
            public TS Data { get; private set; }

            public IndexKeepNode(int sourceIndex, TS data)
            {
                this.SourceIndex = sourceIndex;
                this.Data = data;
            }


            public int CompareTo(IndexKeepNode other)
            {
                return this.Data.CompareTo(other.Data);
            }
        }
    }
    
    
    
    
}