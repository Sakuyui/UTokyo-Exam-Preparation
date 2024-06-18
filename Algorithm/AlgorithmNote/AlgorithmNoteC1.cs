using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.AlgorithmNote
{
    public class AlgorithmNoteC1
    {
        /*
         *
         */
        
        
        /*
         *  dp - 最大子数组和
         */
        public int MaxSubArray(int[] nums)
        {
            //以i结尾的子数组和
            // dp[i] = max(dp[i - 1] + A[i], A[i])
            //降维吧
            var preState = nums[0];
            var res = preState;
            for (var i = 1; i < nums.Length; i++)
            {
                preState = Math.Max(preState + nums[i], nums[i]);
                res = Math.Max(res, preState);
            }
            return res;
        }
         
        
        /*
         * DP-最长递增子序列 LIS
         * 对于二维的话，先第一维度按递增排序，相同的话按第二维度降序
         */
        public int BinaryLis(int[] nums)
        {
            //类似取牌算法,在牌堆中二分搜索插入。最后牌堆数就是结果
            var top = new int[nums.Length];
            var piles = 0;
            foreach (var poker in nums)
            {
                //BinarySearch 搜索左边界,扔进去
                var left = 0;
                var right = piles;
                while (left < right)
                {
                    var mid = (left + right) / 2;
                    if (top[mid] > poker)
                    {
                        right = mid; // [left,mid)
                    }else if (top[mid] < poker)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid; //继续寻找左边界
                    }
                }
                //未找到就创建牌堆
                if (left == piles)
                    piles++;
                top[left] = poker;
            }

            return piles;
        }
        public int Lis(int[] nums)
        {
            //dp[i] 以i为结尾的最长递增子序列长度
            var dp = new int[nums.Length + 1];
            for (var i = 1; i <= nums.Length; i++)
            {
                var max = 0;
                //可以使用二分
                for (var j = 0; j < i - 1; j++)
                {
                    if (nums[j] < nums[i - 1])
                    {
                        max = Math.Max(dp[j + 1] + 1, max);
                    }
                }
                dp[i] = max;
            }

            return dp[nums.Length];
        }
        
        
        public int EditDistance(string str1, string str2)
        {
            var s1Len = str1.Length;
            var s2Len = str2.Length;
            int[,] dp = new int[str1.Length + 1, str2.Length + 1];
            for (var i = 1; i <= s1Len; i++)
            {
                for (var j = 1; j <= s2Len; j++)
                {
                    if (str1[i] == str2[j])
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {
                        dp[i, j] =
                            Math.Min(Math.Min(dp[i - 1, j - 1], dp[i, j - 1]), dp[i - 1, j])
                            + 1;
                    }
                }
            }
            return dp[str1.Length, str2.Length];
        }


        /*
         * 凑零钱*
        */
        public int CoinAmount(int[] nums, int sum)
        {
            var dp = new int[sum + 1];
            for (var i = 1; i < sum; i++)
            {
                dp[i] = Math.Min(dp[i - 1] + 1, dp[(i - 2 < 0) ? 0 : i - 1] + 1);
                dp[i] = Math.Min(dp[i], dp[i - 5 < 0 ? 0 : i - 5] + 1);
            }
            return dp[sum];
        }
        
        
        
        
        
        /*
         * 二分搜索右边界
         */
        public int BinarySearchRightEdge(int[] nums, int target)
        {
            var left = 0;
            var right = nums.Length;
            while (left < right)
            {
                var mid = (left + right) / 2;
                if (nums[mid] == target)
                {
                    left = mid + 1;
                }
                else if(nums[mid] < target)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid;
                }
            }

            return left - 1; // mid = left -1;
        }
        
        
        /*
         * 滑动窗口最长无重复子串
         */

        public int LengthOfLongestSubstring(string s)
        {
            Dictionary<char, int> windows = new Dictionary<char, int>();
            var left = 0;
            var right = 0;
            var res = 0;
            //窗口右指针
            while (right < s.Length)
            {
                var c = s[right];
                right++;
                if (windows.ContainsKey(c))
                    windows[c]++;
                else
                    windows[c] = 1;
                //搜索左窗口
                while (windows[c] > 1)
                {
                    char d = s[left];
                    left++;
                    windows[d]--;
                }

                res = Math.Max(res, right - left);
            }

            return res;
        }
        
        
        
        
        /*
         * 滑动窗口 - 字符串排列 
         */
        public bool CheckStrPermutationInclusion(string t, string s)
        {
            var need = new Dictionary<char, int>();
            var window = new Dictionary<char, int>();

            foreach (var c in t)
            {
                if (need.ContainsKey(c))
                    need[c]++;
                else
                    need[c] = 1;
            }
            int left = 0, right = 0;
            int vaild = 0;
            //模板
            while (right < t.Length)
            {
                char c = s[right];
                right++;
                if (need.ContainsKey(c))
                {
                    window[c]++;
                    if (window[c] == need[c])
                        vaild++;
                }
                
                //收缩
                while (right - left >= t.Length)
                {
                    if (vaild == need.Count)
                        return true;
                    char d = s[left];
                    left++;
                    if (need.ContainsKey(d))
                    {
                        if (window[d] == need[d])
                            vaild--;
                        window[d]--;
                    }
                }

                
            }
            
            return false;
        }


        /*
         * 左边界二分搜索
         */
        public int BinarySearchLeftEdge(int[] nums, int target)
        {
            var left = 0;
            var right = nums.Length - 1;
            while (left <= right)
            {
                var mid = (right + left) / 2;
                if (nums[mid] == target)
                {
                    right = mid - 1;
                }
                else if(nums[mid] < target)
                {
                    left = mid + 1;
                }
                else if(nums[mid] > target)
                {
                    right = mid - 1;
                }
            }
            return left + 1;
        }
        
        
        /*
         * ！！！ BFS 解开密码的最少次数
         * BFS模板惹
         */
        public int MinTryPwdToOpenBox(string[] deadends, string target)
        {
            //肯定先要有一个队列
            var q = new Queue<string>();
            //扔起点
            q.Enqueue("0000");
            
            //上面那些是基础模板，下面是为实际问题补充的
            var visited = new HashSet<string>(); //记录搜索过的，防止重复
            var deads = deadends.Select(p => p).ToHashSet();
            if (deads.Contains(target))
                return -1;
            visited.Add("0000");
            var step = 0;
            
            while (q.Count > 0)
            {
                //每层进行
                var layerSize = q.Count;
                for (var i = 0; i < layerSize; i++)
                {
                    var cur = q.Dequeue();
                    if(deads.Contains(cur))
                        continue;
                    if (cur == target)
                        return step;
                    
                    //插入子节点
                    for (var j = 0; j < 4; j++)
                    {
                        var up = plusOne(cur, j);
                        if(!visited.Contains(up))
                            q.Enqueue(up);
                        var down = downOne(cur, j);
                        if(!visited.Contains(down))
                            q.Enqueue(down);
                    }
                }
                step++;
            }
            return -1;
        }
        
        
        /*
         * 转动密码锁的双向BFS优化
         * (双向BFS必须有限制，必须知道目标节点)
         */

        public int BiBfsMinTryPwdToOpenBox(IEnumerable<string> deadends, string target)
        {
            var deads = deadends.ToHashSet();
            //这里用集合代替队列。
            var q1 = new HashSet<string>(); //搜索队列1
            var q2 =  new HashSet<string>(); //搜索队列2
            var visited = new HashSet<string>(); //记忆，防重复
            q1.Add("0000");
            q2.Add(target);
            var step = 0;
            //注意结束条件
            while (q1.Count > 0 && q2.Count > 0)
            {
                var tmp = new HashSet<string>();
                //extend(q1)
                //遍历一层
                foreach (var e in q1)
                {
                    //判断条件
                    if(deads.Contains(e)) 
                        continue;
                    if (q2.Contains(e))
                        return step;
                    //入队
                    for (var j = 0; j < 4; j++)
                    {
                        string up = plusOne(e, j);
                        if (!visited.Contains(up))
                            tmp.Add(up);
                        string down = downOne(e, j);
                        if (!visited.Contains(down))
                            tmp.Add(down);
                    }
                }

                step++;
                //从另一个位置遍历
                q1 = q2;
                q2 = tmp;
            }

            return -1;
        }
        //拨动某一位
        public string plusOne(string s, int j)
        {
            var ch = s.ToCharArray();
            if (ch[j] == '9')
                ch[j] = '0';
            else
                ch[j] = (char) (ch[j] + 1);
            return ch.ToString();
        }

        public string downOne(string s, int j)
        {
            var ch = s.ToCharArray();
            if (ch[j] == '0')
                ch[j] = '9';
            else
                ch[j] = (char) (ch[j] - 1);
            return ch.ToString();
        }
        
        
        
        
        /*
         * !!!!!!!!BFS法 二叉树最小深度
         */
        public int BinanryTreeMinHeight<T>(BinaryTreeNode<T> root)
        {
            var depth = 0;
            var queue = new Queue<BinaryTreeNode<T>>();
            if (root == null)
                return 0;
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                //每层遍历的方式（记录层数惹）
                var layerSize = queue.Count;
                depth++;
                for (var i = 0; i < layerSize; i++)
                {
                    var node = queue.Dequeue();
                    if (node.LeftNode == null && node.RightNode == null)
                        return depth;
                    if(node.LeftNode != null)
                        queue.Enqueue(node.LeftNode);
                    if(node.RightNode != null)
                        queue.Enqueue(node.RightNode);
                }
            }
            return depth;
        }
        
        public int CountOne1(int num)
        {
            var c = 0;
            for (var i = 0; i < 32; i++)
            {
                c += (num >> i) & 1;
            }

            return c;
        }

        public int CountOne2(int num)
        {
            var c = 0;
            for (var x = num; x != 0; x &= (x - 1), c++) ;
            return c;
        }
    }

  
}