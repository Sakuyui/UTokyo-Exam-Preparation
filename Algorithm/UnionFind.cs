using System.Collections.Generic;

namespace Algorithm
{
    public class UnionFind
    {
        private int[] Father;
        private int[] Weight;
        public UnionFind(int size)
        {
            Father = new int[size];
            Weight = new int[size];
        }
        
        public int Find(int u)
        {
            return u == Father[u] ? u : Find(Father[u]);
        }
        public void Join(int u, int v, int w = 0)
        {
            var u1 = Find(u);
            var v1 = Find(v);
            if(u1 == v1)
                return;
            Father[v] = u;
            Weight[v] = w;
        }

        public bool Same(int u, int v)
        {
            return Find(u) == Find(v);
        }
    }
}