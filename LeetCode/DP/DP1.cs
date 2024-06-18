using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCode.DP
{
    public class Dp1
    {
        bool CanWin(string s) {
        
            //记录每个字符串对应游戏结果的dp
            var dp = new Dictionary<string,bool>();
            bool Dfs(StringBuilder s){
                //如果当前s,之前已经操作过,则可以直接返回结果
                if(dp.ContainsKey(s.ToString())) return dp[s.ToString()];

                //当前 string 下的游戏结果 
                var res = false;

                for(var i = 1 ; i < s.Length ; ++i)
                {
                    if (s[i] != '+' || s[i - 1] != '+') continue;
                    s[i] = '-';
                    s[i - 1] = '-';
                    //当我改变这两个字符后 , 存在一种下一个先手的人必输的情况 ， 那么当前先手的人就是必胜的
                    res = res | (!Dfs(s));
                    s[i] = '+';
                    s[i - 1] = '+';
                }
                dp[s.ToString()] = res;
                return dp[s.ToString()];
            }
            //判断是否当前先手可以赢，所以return 第一级dp即可
            return Dfs(new StringBuilder(s));
        }

    

        public int ThrowEgg(int kEgg,int nLevel,  Dictionary<int,Dictionary<int, int>> memo = null)
        {
            if (nLevel < 1 || kEgg < 1)
                return 0;
            if (kEgg == 1)
                return nLevel;
            memo ??= new Dictionary<int, Dictionary<int, int>>();

            if (memo.ContainsKey(kEgg) && memo[kEgg].ContainsKey(nLevel))
                return memo[kEgg][nLevel];
            var res = int.MaxValue;
            for (var i = 1; i < nLevel + 1; i++)
            {
                //这次在i扔
                res = Math.Min(res, Math.Max(ThrowEgg(kEgg, nLevel - i, memo), ThrowEgg(kEgg - 1, i - 1, memo)) + 1);
               
            }
            memo[kEgg][nLevel] = res;
            return res;
        }

        public int EditDistance<T>(IEnumerable<T> d1, IEnumerable<T> d2)
        {
            var input1 = d1.ToArray();
            var input2 = d2.ToArray();
            var n1 = input1.Length;
            var n2 = input2.Length;
            var dp = new int[n1 + 1].Select(_ => Enumerable.Repeat(int.MaxValue, n2 + 1).ToArray()).ToArray();
            for (var i = 0; i <= n1; i++)
            {
                dp[i][0] = i + 1;
            }

            for (var i = 0; i <= n2; i++)
            {
                dp[0][i] = i;
            }

            for (var i = 1; i < n1; i++)
            {
                for (var j = 1; j < n2; j++)
                {
                    if (input1[i].Equals(input2[j]))
                    {
                        dp[i][j] = dp[i - 1][j - 1];
                    }
                    else
                    {
                        dp[i][j] = Math.Min(dp[i][j - 1] + 1, Math.Min(dp[i - 1][j - 1] + 1, dp[i - 1][j] + 1));
                    }
                }
            }

            return dp[n1][n2];
        }


        
        public IEnumerable<T> Lis<T>(IEnumerable<T> enumerable, Comparer<T> comparer = null)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            var len = ts.Length;
            var heap = new T[len];
            var pt = -1;
            
            for (var i = 0; i < len; i++)
            {
                var t = ts[i];
                if (pt < 0)
                {
                    ts[++pt] = t;
                    continue;
                }

                var l = 0;
                var r = pt;
                while(l <= r){
                    var m = l + ((r - l) >> 1);
                    if(comparer != null ? comparer.Compare(ts[m], t) < 0 : ((IComparable)(ts[m])).CompareTo(t) < 0 ){
                        l = m + 1;
                    }else{
                        r = m - 1;
                    }
                }
                var index = l;
                if (index > pt)
                {
                    ts[++pt] = t;
                }
                else
                {
                    ts[index] = t;
                }
                
            }

            return ts.Take(pt + 1);
        }
        public int Lcs(string text1, string text2) {
            var n1 = text1.Length;
            var n2 = text2.Length;
            var dp = new int[n2 + 1];
        
            for(var i = 1; i <= n1; i++){
                var pre = dp[0];

                for(var j = 1; j <= n2; j++){
                    var tmp = dp[j];
                    if(text1[i - 1] == text2[j - 1]){
                        dp[j] = pre + 1;
                    }else{
                        dp[j] = Math.Max(dp[j], dp[j - 1]);
                    }
                    pre = tmp;
                }
            }
            return dp[n2];
        }
        public int CountTwoInRange(int n)
        {
            int Count(int number, int d)
            {
                var pow10 = (int)Math.Pow(10, d);
                var pow10P = pow10 * 10;
                var right = number % pow10;

                var roundDown = number - number % pow10P;
                var roundUp = roundDown + pow10P;
                var digit = (number / pow10) % 10;
                return digit switch
                {
                    //分类讨论
                    < 2 => roundDown / 10,
                    2 => roundDown / 10 + right + 1,
                    _ => roundUp / 10
                };
            }
            //对于第i位，分析小于2，等于2以及大于2的2情况。
            var len = (n + "").Length;
            var c = 0;
            for (var i = 0; i < len; i++)
            {
                c += Count(n, i);
            }

            return c;
        }
        //切棍子最小成本 经典区间DP
        
        public int MinCost(int len, int[] cuts)
        {
            var m = cuts.Length;
            var sort = cuts.OrderBy(e => e).Prepend(0).Append(len).ToArray();

            var dp = new int[m + 2].Select(_ => new int[m + 2]).ToArray();
            //dp[i,j]代表i~j部分最小分割代价。如果i==j显然不用切割
            for (var k = 2; k <= m + 1; k++) { //区间<=1不用割。这里是选择遍历区间长度
                for (var l = 1; l <= m; l++)
                {
                    // 区间长度k固定时，r位置计算出来
                    var r = l - 1 + k - 1;
                    if(r > m)
                        break;
                    
                    for (var c = l; c <= r; c++) {
                        dp[l][r] = System.Math.Min(dp[l][r], dp[l][c - 1] + dp[c + 1][r]);
                    }
                    dp[l][r] += sort[r + 1] - sort[l - 1];
                }
            }
            return dp[1][m];
            /*
             * 也可以记忆化DFS
             * for(int i = 0; i < m; ++i){
                    ans = min(ans, help(cuts, 0, i - 1) + help(cuts, i + 1, m - 1));
                }
                
                int help(vector<int>& cuts, int left, int right){
        if(left > right) return 0;
        if(mem[left][right] != -1) return mem[left][right];
        int ans = INT_MAX;
        for(int i = left; i <= right; ++i){
            ans = min(ans, help(cuts, left, i - 1) + help(cuts, i + 1, right));
        }
        ans += (right + 1 == cuts.size() ? n : cuts[right + 1]) - (left == 0 ? 0 : cuts[left - 1]);
        return mem[left][right] = ans;
    }

             */

            List<string> FullJustify(string[] words, int maxWidth) {
                //定义0-maxWidth个空格字符串，方便之后直接调用
                var space = new string[maxWidth + 1]; 
                var s = new StringBuilder();
                for(var i = 0;i<maxWidth+1;i++){
                    space[i] = s + "";
                    s.Append(" ");
                }
                //新建List，用来存最后的结果。
                var pWords = new List<string>(); 
                //遍历整个words，一行一行的排版     
                for(var i=0; i < words.Length; i++){
                    int curlen = words[i].Length; //记录当前已读取单词的长度，当>=maxWidth时进行排版
                    var startI = i;                //记录本次读取单词的起点
                    while(i < words.Length - 1 && curlen < maxWidth){
                        i++;
                        curlen = curlen + words[i].Length + 1;  // 每多读一个单词都要加一个空格
                    }
                    if(curlen>maxWidth){       //当前长度>maxWidth，说明已经多读取了一个单词
                        curlen = curlen - words[i].Length - 1;
                        i--;
                    }   
                    //一行一行的排版
                    pWords.Add(ProcessCurline(words,startI,i,curlen,maxWidth,space));
                }
                return pWords; 
            }

            string ProcessCurline(string[] words,int si,int ei,int curLen, int maxWidth, string[] space){
                var sb = new StringBuilder();   //用来进行排版
                var map = ei-si;                   // 记录单词之间的有几个间隙
                var addSpace = maxWidth - curLen + map;   //记录这一行总共有多少个空格
                if(map==0){               //间隙为0，证明只有一个单词
                    sb.Append(words[ei]);
                    sb.Append(space[addSpace]);
                    return sb + "";
                }
                if(ei == words.Length - 1){            //证明要排版最后一行了，格式特殊
                    for(var i = si; i < ei; i++){
                        sb.Append(words[i]).Append(" ");
                    }
                    sb.Append(words[ei]);             //最后一个单词不用加空格 
                    sb.Append(space[addSpace-map]);   //如果还有多余空格，一起加上
                    return sb + "";
                }
                var allAddSpace = addSpace/map;     //所有的空格数 / 间隙 = 每个间隙必加的空格数
                var left = addSpace % map + si;     //多出来的空格要从si开始，依次加在间隙中
                for(var i = si;i<ei;i++){
                    sb.Append(words[i]).Append(space[allAddSpace]);
                    if(i < left) sb.Append(" ");     // <left就要多加一个空格
                }
                sb.Append(words[ei]);               
                return sb.ToString();
            }
        }
    }
}