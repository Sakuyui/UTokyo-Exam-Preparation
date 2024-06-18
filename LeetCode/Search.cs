using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
    // public class Search
    // {
    //        public bool IsEquationSolvable(string[] words, string result) {
    //         var charsNeedMapping = words.SelectMany(e => e).Union(result).ToHashSet(null);
    //         
    //         var map = new Dictionary<char, int>() {{'*', 0}};
    //         var maxLen = words.Select(e => e.Length).Max();
    //         words = words.Select(e => e.PadLeft(maxLen, '*')).ToArray();
    //         result = result.PadLeft(maxLen, '*');
    //         int ToNumber(string str, Dictionary<char, int> map) => 
    //             int.Parse(str.Select(c => map[c]).Aggregate("", (a, b) => a + b));
    //
    //         bool Dfs(int dept = 0, int cIn = 0)
    //         {
    //             $"dept = {dept}".PrintToConsole();
    //             if (dept == maxLen)
    //                 return cIn == 0;
    //             
    //             //低位优先
    //             var curBits = words.Select(e => e[maxLen - dept - 1]).ToHashSet(null);
    //             var charStillNeedMap = curBits.Except(map.Keys).ToArray();
    //             
    //             var stillCanSel = Enumerable.Range(0, 10).Except(map.Values).ToArray();
    //             if (stillCanSel.Length < charStillNeedMap.Length)
    //                 return false;
    //             //var permutation = CollectionHelper.GetPermutation(stillCanSel, charsNeedMapping.Count);
    //             foreach (var p in permutation)
    //             {
    //                 var pairs = charsNeedMapping.Zip(p, (a, b) => (a, b)).ToArray();
    //                 foreach (var pair in pairs)
    //                 {
    //                     map[pair.a] = pair.b;
    //                 }
    //                 
    //                 var r = curBits.Aggregate(0, (a, b) => a + map[b]);
    //                 //$"cur = {map.GetMultiDimensionString()}, r = {r}".PrintToConsole();
    //                 if (!map.ContainsKey(result[maxLen - dept - 1]))
    //                 {
    //                     map[result[maxLen - dept - 1]] = (r + cIn) % 10;
    //                 }
    //                 else
    //                 {
    //                     //check
    //                     if (map[result[maxLen - dept - 1]] != (r + cIn) % 10)
    //                         continue;
    //                 }
    //
    //                 var b = Dfs(dept + 1, r + cIn >= 10 ? 1 : 0);
    //                 if (b)
    //                     return true;
    //                 foreach (var pair in pairs)
    //                 {
    //                     map.Remove(pair.a);
    //                 }
    //             }
    //
    //             return false;
    //         }
    //
    //         return Dfs();
    //     }
    //     
    // }
}