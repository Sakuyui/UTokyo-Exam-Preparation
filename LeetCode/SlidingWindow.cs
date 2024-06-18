using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
    public class SlidingWindow
    {
        public int MedianStream(IEnumerable<int> input)
        {
         /*
          * def middle(A):
    for e in A:
        if(maxheap is empty):
            maxheap.push(e)  #先处理大堆
        else:
            if(len(maxheap) = len(minheap)):
                if(e < maxheap.top()):  #用大堆存比中位数小的元素
                	maxheap.push(e)
                else:
                    minheap.push(e)
            else if(len(maxheap) > len(minheap)): #max比min堆元素多时要调平衡。
                if(e > mapheap.top()):
                    minheap.push(e)  #此时直接扔到最小堆就平衡了
                else:
                    minheap.push(maxheap.pop()) #这样就平衡了
                    maxheap.push(e)
            else: #小堆比大堆多，同样要保持平衡、
                if(e < maxheap.top()):
                    maxheap.push(e) #这样直接就平衡了
                else:
                    maxheap.push(minheap.pop())
                    minheap.push(e)
                     
    return len(maxheap) = len(minheap) ? (maxheap.top()+minheap.top())/2 : minheap.top()
            
          */
         return 0;
        }
        public int FetchRain(int[] nums)
        {
            var leftMax = new int[nums.Length];
            var lMax = leftMax[0];
            for (var i = 0; i < nums.Length; i++)
            {
                lMax = Math.Max(lMax, leftMax[i]);
                leftMax[i] = lMax;
            }

            var sum = 0;
            var rightMax = nums[^1];
            for (var i = nums.Length - 1; i >= 0; i--)
            {
                rightMax = Math.Max(rightMax, nums[i]);
                var secondTallest = Math.Min(rightMax, leftMax[i]);
                if (secondTallest > nums[i])
                    sum += secondTallest - nums[i];
            }

            return sum;
        }
        //424. 替换后的最长重复字符。替换k个字符后，最长连续字符串是多长?k=0d的话就退化为求最长连续字符串问题了。
            //任何时刻map数组的总和一定就是区间大小
        /*
         *class Solution {
    private int[] map = new int[26];

    public int characterReplacement(String s, int k) {
        if (s == null) {
            return 0;
        }
        char[] chars = s.toCharArray();
        int left = 0;
        int right = 0;
        int historyCharMax = 0;
        for (right = 0; right < chars.length; right++) {
            int index = chars[right] - 'A';
            map[index]++;
            historyCharMax = Math.max(historyCharMax, map[index]);
            if (right - left + 1 > historyCharMax + k) { 
                map[chars[left] - 'A']--;
                left++;
            }
        }
        return chars.length - left;
    }
}
         * 
         */
    }
}