using System.Collections.Generic;
using System.Linq;

namespace LeetCode.Array
{
    public class ArrayBfs
    {
        public int Jump(int[] nums) {
            var n = nums.Length;
            var q = new Queue<(int index, int most)>();
            q.Enqueue((0, nums[0]));
            var cnt = 0;
            var memo = new HashSet<int>();
            if(nums.Length == 1)
                return 0;
            while(q.Any()){
                var c = q.Count;
                for(var i = 0; i < c; i++){
                    var f = q.Dequeue();
                    if(f.most >= n - 1)
                        return cnt + 1;
                    if(memo.Contains(f.index))
                        continue;
                    memo.Add(f.index);
                    for(var j = f.index; j <= f.most && j < n ; j++){
                        if(!memo.Contains(j)){
                            q.Enqueue((j, j + nums[j]));
                        }
                    }
                }
                cnt++;
            }
            return 0;
        }
    }
}