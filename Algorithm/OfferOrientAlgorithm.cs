using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Algorithm
{
    public class OfferOrientAlgorithm
    {
        /**
         * 找出不重复的数*
         */
        public int NoRepeatNum2(int[] nums)
        {
            if (nums.Length == 0) return -1;
            var res = nums[0];
            for (var i = 1; i < nums.Length; i++)
            {
                res ^= nums[i];
            }

            return res;
        }

        /**
         * 找出重复的数, 限定所有数字都在0 ~ n -1
         */
        public int RepeatNum(int[] nums)
        {
            var n = nums.Length;
            if (n <= 0) return -1;
            for (var i = 0;;)
            {
                if (nums[i] >= n) return -1; //错误,不符合限制惹
                if (nums[i] == i)
                {
                    i++;
                    continue; //如果已经符合他的位置
                }
                //不相等就放到合适的位置
                if (nums[nums[i]] == nums[i]) return nums[i]; //如果发现重复了
                //交换
                var t = nums[i];
                nums[i] = nums[nums[i]];
                nums[t] = t;
            }
        }

        /**
         * 旋转数组最小数字
         * 比如[3,4,5,1,2] 输出 1
         * 双指针 + 二分
         */
        public int MinRotatedArrayNum(int[] nums)
        {
            var n = nums.Length;
            if (n == 0) return -1;
            var left = 0;
            var right = n - 1;
            while (left <= right)
            {
                var mid = (left + right) / 2;
                if (nums[mid] >= nums[left]) left = mid;
                if (nums[mid] < nums[left]) right = mid;
            }

            return nums[right];
        }

        /**\
         * 1的个数*
         */

        public int CountOneWithLowBitClear(int i)
        {
            var res = 0;
            while (i != 0)
            {
                i &= (i - 1);
                res++;
            }
            return res;
        }

        public double Pow(int x, int n)
        {
            if (n == 0) return 1;
            var nPositive = n > 0 ? n : -n;
            var t = Pow(x, n / 2);
            var res = t * t * ( (n & 1) == 1 ? x : 1);
            return (nPositive ^ n) == 0 ? res : 1.0 / res;
        }

        /**
         * 奇偶链表，让奇数在偶数前面
         */

        public List<LinkedNode<int>> LinkedTablePutOddAhead(LinkedNode<int> head)
        {
            if (head?.Next == null) return new List<LinkedNode<int>>(new LinkedNode<int>[2]{head,head});
            //奇偶链表的头指针
            var headOdd = head; //奇数
            var headEven = head; //偶数
            //将指针移动到正确的位置
            while (headOdd.Next != null && ((headOdd.Data & 1) == 0)) headOdd = headOdd.Next;
            while (headEven.Next != null && ((headEven.Data & 1) == 1)) headEven = headEven.Next;
            //都是奇数或者都是偶数
            if((headEven.Data & 1) != 0 || (headEven.Data & 1) != 1) return new List<LinkedNode<int>>(new LinkedNode<int>[2]{head,head});
            var oddCur = headOdd;
            var evenCur = headEven;
            while (oddCur.Next != null)
            {
                oddCur = oddCur.Next;
                if ((oddCur.Data & 1) == 0)
                {
                    //把这个节点扔到偶数去
                    evenCur.Next = oddCur;
                    evenCur = oddCur;
                    //从奇链表中删除该节点
                    oddCur.Next = oddCur.Next;
                }
            }
            return new List<LinkedNode<int>>(new LinkedNode<int>[2]{headOdd,headEven});
        }
        
        /*
         * last k-th node
         */

        public LinkedNode<T> LinkedTableLastKthNode<T>(LinkedNode<T> head, int k)
        {
            var thead = head;
            var ptr1 = thead;
            //先走k步
            for (var i = 0; i < k; i++)
            {
                if (ptr1 == null) return null;
                ptr1 = ptr1.Next;
            }

            
            //一起走
            var ptr2 = thead;
            while (ptr1 != null)
            {
                ptr2 = ptr2.Next;
                ptr1 = ptr1.Next;
            }
            return ptr2;
        }

        /**
         * 链表环的入口
         */
        public LinkedNode<T> LikedTableRingEntry<T>(LinkedNode<T> head)
        {
            //先一个走两步一个走一步，让两个node相遇
            var ptr1 = head;
            var ptr2 = head;
            if (head?.Next == null) return null;
            ptr1 = ptr1.Next;
            ptr2 = ptr2.Next.Next;
            //直到相遇
            while (ptr1 != ptr2)
            {
                ptr1 = ptr1.Next; //1步
                ptr2 = ptr2.Next.Next; //2步
            }
            ptr1 = head;
            //一起走
            while (ptr1 != ptr2)
            {
                ptr1 = ptr1.Next;
                ptr2 = ptr2.Next;
            }
            return ptr1;
        }

        /**
         * 反转链表
         */
        public LinkedNode<T> LinkedTableReverse<T>(LinkedNode<T> head)
        {
            if (head?.Next == null) return head;
            LinkedTableReverse<T>(head.Next);
            //画图就能理解
            head.Next.Next = head;
            head.Next = null;
            return head;
        }


        /**
         * 树的子结构 !!!
         */

        public bool BinaryTreeIsSubStructure<T>(BinaryTreeNode<T> nodeA, BinaryTreeNode<T> nodeB)
        {
            var res = false;
            if (nodeA == null || nodeB == null) return false;
            if (nodeA.Data.Equals(nodeB.Data))
                res = BinaryTreeIsPartEqual(nodeA, nodeB);
            if (!res)
                res = BinaryTreeIsSubStructure<T>(nodeA.LeftNode, nodeB);
            if (!res)
                res = BinaryTreeIsSubStructure<T>(nodeA.RightNode, nodeB);
            return res;
        }

        public bool BinaryTreeIsPartEqual<T>(BinaryTreeNode<T> nodeA, BinaryTreeNode<T> nodeB)
        {
            if (nodeB == null) return true;
            if (nodeA == null) return false;
            if (!nodeA.Data.Equals(nodeB.Data)) return false;
            return BinaryTreeIsPartEqual<T>(nodeA.LeftNode, nodeB.LeftNode) &&
                   BinaryTreeIsPartEqual<T>(nodeA.RightNode, nodeB.RightNode);
        }
        
        
        /***
         * 合并K个有序链表
         */

        public LinkedNode<T> MergeKLinkedNode<T>(List<LinkedNode<T>> lists) where T : IComparable
        {
            var k = lists.Count;
            var listPtr = new LinkedNode<T>[k];
            var finished = new bool[k];
            if (k == 1) return lists[0];
            if (k == 0) return null;
            
            var tIndex = 0;
            for (var i = 0; i < k; i++)
            {
                listPtr[i] = lists[i];
                if (lists[i] == null)
                {
                    finished[i] = true;
                    continue;
                }
                if (lists[i].Data.CompareTo(lists[tIndex].Data) < 0)
                {
                    tIndex = i;
                }
                
            }
            var baseList = lists[tIndex];
            finished[0] = true; //do not consider base list; 
            //swap
            if (tIndex != 0)
            {
                var t = lists[tIndex];
                lists[tIndex] = lists[0];
                lists[0] = t;
            }

            LinkedNode<T> preIndex = null;
            //每次找最小的元素插入基准列
            while (finished.Any(e => !e))
            {
                var minIndex = 0;
                for (var i = 1; i < k && !finished[i]; i++)
                {
                    if (lists[i].Data.CompareTo(lists[minIndex]) < 0)
                    {
                        minIndex = i;
                    }
                }

                if (lists[minIndex].Data.CompareTo(baseList.Data) > 0)
                {
                    preIndex = baseList;
                    var t = preIndex.Next;
                    var t2 = lists[minIndex];
                    baseList.Next = lists[minIndex];
                    lists[minIndex] = lists[minIndex].Next;
                    t2.Next = t;
                    if (lists[minIndex] == null) finished[minIndex] = true;
                }
                
            }

            return baseList;
        }
        
        /*
         * 和为K的二叉树路径
         */

        public bool BinaryTreeIsSumKPathExist(BinaryTreeNode<double> root, double k)
        {
            if (root == null) return k == 0;
            if ((k - root.Data).Equals(0))
                return true;
            return BinaryTreeIsSumKPathExist(root.LeftNode, k) 
                   || BinaryTreeIsSumKPathExist(root.RightNode, k) 
                   || BinaryTreeIsSumKPathExist(root.RightNode, k - root.Data) 
                   || BinaryTreeIsSumKPathExist(root.RightNode, k - root.Data);
        }
        
        
        /*
         * Quick sort
         */
        public void QuickSort<T>(T[] nums, int from, int to) where T:IComparable
        {
            if (to - from <= 0) return; 
            //[from, to]
            var pivotIndex = new Random().Next(from, to);
            var t = nums[to];
            nums[to] = nums[pivotIndex];
            nums[pivotIndex] = t;
            var left = from;
            var right = to - 1;
            while (left < right)
            {
                if (nums[left].CompareTo(nums[right]) > 0)
                {
                    //swap
                    var tmp = nums[left];
                    nums[left] = nums[right];
                    nums[right] = tmp;
                    right--;
                }
                else
                {
                    left++;
                }
            }

            if (nums[left].CompareTo(pivotIndex) < 0)
                left++;
            var tmp2 = nums[left];
            nums[left] = nums[to];
            nums[to] = tmp2;
            //第left+1顺序统计量
            QuickSort<T>(nums,from,left - 1);
            QuickSort<T>(nums, left, to);
        }


        /**
         * First Only Appear one time
         */
        public T GetFirstOnlyAppearOnetime<T>(T[] nums)
        {
            var k = nums.Length;
            var map = new Dictionary<T,int>();
            foreach (var e in nums)
            {
                if (!map.ContainsKey(e))
                    map[e] = 1;
                else
                    map[e]++;
            }
            foreach(var e in nums)
            {
                if (map[e] > 1) return e;
            }

            return default;
        }

        public void FirstOnlyAppearOneTimeStream<T>(T[] nums)
        {
            //Key: 数据 val:第一次出现的位置
            var map = new Dictionary<T,int>();
            for (var i = 0; i < nums.Length; i++)
            {
                var e = nums[i];
                if (map.ContainsKey(e))
                {
                    map.Remove(e);
                }
                else
                {
                    map[e] = i;
                }

                var m = (from p in map orderby p.Value select p).ToList()[0].Key;
                Console.WriteLine(m);
            }
        }
        
        
        /*
         * 逆序对!!!
         */
        public int CountReversePairs(int[] nums,int[] copy,int left, int right)
        {
            if (right - left <= 0) return 0;
            var mid = (left + right) / 2;
            var lc = CountReversePairs(nums, copy, left, mid);
            var lr = CountReversePairs(nums, copy, mid + 1, right);
            return lc + lr;
        }



        public (int, int) TwoSum(int[] nums, int k)
        {;
            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            for (var i = 0; i < nums.Length; i++)
            {
                if(!dict.ContainsKey(nums[i]))
                {
                    var list = new List<int>();
                    list.Add(i);
                    dict.Add(nums[i],list);
                }
                else
                {
                    dict[nums[i]].Add(i);
                }
            }

            for (var i = 0; i < nums.Length; i++)
            {
                var s = k - nums[i];
                if (!dict.ContainsKey(s)) continue;
                if (dict[s].Any(e => e != i))
                    return (nums[i], dict[s].First(e => e != i));
            }

            return (-1, -1);
        }
        
        
    }
    
    
    
    
    /**
     * Linked Table Node's class
     */
    public class LinkedNode<T>
    {
        public T Data;
        public LinkedNode<T> Next;

        public LinkedNode(T data, LinkedNode<T> next)
        {
            Data = data;
            Next = next;
        }
    }


    /**
     * Binary Tree class
     */
    public class BinaryTreeNode<T>
    {
        public BinaryTreeNode<T> LeftNode;
        public BinaryTreeNode<T> RightNode;
        public T Data;
        public BinaryTreeNode(BinaryTreeNode<T> leftNode, BinaryTreeNode<T> rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
    
    /*
     * 快速获取最小值的栈
     */
    public class MinStack<T> where T : IComparable
    {
        public Stack<T> asistStack = new Stack<T>();
        public Stack<T> realStack = new Stack<T>();
        public void Push(T val)
        {
            realStack.Push(val);
            if (realStack.Count == 0)
            { ;
                asistStack.Push(val);
            }
            else
            {
                asistStack.Push(realStack.Peek().CompareTo(val) < 0? realStack.Peek():val);
            }
        }

        public T Pop()
        {
            if (realStack.Count == 0) return default;
            asistStack.Pop();
            return realStack.Pop();
        }

        public T Min => asistStack.Peek();
    }
}