using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode.Array
{
    public class Array1
    {
        public int FindUnsortedSubarray(int[] nums) {
            int n = nums.Length;
            int maxn = int.MinValue, right = -1;
            int minn = int.MaxValue, left = -1;
            for (int i = 0; i < n; i++) {
                if (maxn > nums[i]) {
                    right = i;
                } else {
                    maxn = nums[i];
                }
                if (minn < nums[n - i - 1]) {
                    left = n - i - 1;
                } else {
                    minn = nums[n - i - 1];
                }
            }
            return right == -1 ? 0 : right - left + 1;
        }
        
        //第一个缺失正整数
        public int FirstMissingPositive(int[] nums) {
            var n = nums.Length;
            var changed = true;

            //辅助函数
            bool IsInRange(int x) => x > 0 && x <= nums.Length;
            void Swap(int i, int j){
                var t = nums[i];
                nums[i] = nums[j];
                nums[j] = t;
            }

            int Candy(int[] ratings) {
                var n = ratings.Length;
                var ret = 1;
                int inc = 1, dec = 0, pre = 1;
                for (var i = 1; i < n; i++) {
                    if (ratings[i] >= ratings[i - 1]) {
                        dec = 0;
                        pre = ratings[i] == ratings[i - 1] ? 1 : pre + 1;
                        ret += pre;
                        inc = pre;
                    } else {
                        dec++;
                        if (dec == inc) {
                            dec++;
                        }
                        ret += dec;
                        pre = 1;
                    }
                }
                return ret;
            }
        

       

            long ReversePairCount(int[] nums)
            {
                long Merge(int[] a,  int left, int mid, int right)
                {
                    var cnt = 0;
                    var n1 = mid - left;
                    var n2 = right - mid;
                    var l = new int[nums.Length];
                    var r = new int[nums.Length];
                    for (var i = 0; i < n1; i++) l[i] = a[left + i];
                    for (var i = 0; i < n2; i++) r[i] = a[mid + i];
                    l[n1] = l[n2] = int.MinValue;
                    var indexI = 0;
                    var indexJ = 0;
                    for (var k = left; k < right; k++)
                    {
                        if (l[indexI] <= r[indexJ])
                            a[k] = l[indexI++];
                        else
                        {
                            a[k] = r[indexJ++];
                            cnt += n1 - indexI; //mid + j - k - 1
                        }
                    }

                    return cnt;
                }

                long MSort(int[] a, int left, int right)
                {
                    var n = a.Length;
                    if (left + 1 >= right) return 0;
                    var mid = (left + right) >> 1;
                    var v1 = MSort(a, left, mid);
                    var v2 = MSort(a, mid, right);
                    var v3 = Merge(a, left, mid, right);
                    return v1 + v2 + v3;

                }

                return MSort(nums, 0, nums.Length);
            }
        
            /*不应该改变某个元素位置的情况：1. 元素值不在数组范围中 2.元素值已经等于位置 + 1（从0开始计算）
                             3. 目标位置的元素和自身相等（会死循环）*/
            bool ShouldProcess(int num, int pos) => IsInRange(num) && num != pos + 1 && nums[num - 1] != num;
        
            for(var i = 0; i < n; i++){
           
                if(ShouldProcess(nums[i], i)){ //看看当前元素需不需要移动到正确的位置。         
                    var targetPos = nums[i] - 1;  //应该换到的目标位置
                
                    /*交换了两个元素后，被替换的那个元素也可能需要处理，一直这样下去
                        ，直到遇到一个不需要处理的元素*/
                    while(true){
                        var replaced = nums[targetPos]; //被替换的元素
                        Swap(targetPos, i);   //交换
                        targetPos = nums[i] - 1;  //targetPos位置的元素换到了i位置，
                        //计算当前i位置的元素被期望换到的新位置。
                    
                        if(!ShouldProcess(replaced, i))
                            break;
                    }
                }
            }
        
       
            for(var i = 0; i < n; i++){
                if(nums[i] != i + 1)
                    return i + 1;
            }
       
            return n + 1;
        }
        //摆动排序
        public void WiggleSort(int[] nums) {
            var len = nums.Length;
            //寻找中位数，并且让左小右大
            QuickSelect(nums, 0, len, len / 2);
            
            var midPtr = len / 2;
            var mid = nums[midPtr];
            
            void Swap(int p1, int p2) {
                var num = nums[p1];
                nums[p1] = nums[p2];
                nums[p2] = num;
            }
            
            // 3-way-partition
                //ij   mid   k 中位数放在数组中部，同时将小于中位数的数放在左侧，大于中位数的数放在右侧。只需要在快速排序的partition过程的基础上，添加一个指针k用于定位大数：
                //最终  小|中|大
            int i = 0, j = 0, k = nums.Length - 1;
            while(j < k){
                if(nums[j] > mid){
                    Swap(j, k); //ij   mid   k'j'
                    --k;
                }
                else if(nums[j] < mid){
                    Swap(j, i);   //j'i'   mid   k
                    ++i;
                    ++j;
                }
                else{
                    ++j;
                }
            }
        
            if(nums.Length % 2 != 0) ++midPtr;
            var tmp1 = nums.Take(midPtr).ToArray();
            var tmp2 = nums.Skip(midPtr).ToArray();
            for(i = 0; i < tmp1.Length; ++i){
                nums[2 * i] = tmp1[tmp1.Length - 1 - i];
            }
            for(i = 0; i < tmp2.Length; ++i){
                nums[2 * i + 1] = tmp2[tmp2.Length - 1 - i];
            }
        }
    

        public void QuickSelect(int[] nums, int begin, int end, int n){
            var t = nums[end - 1];
            int i = begin, j = begin;

            void Swap(int p1, int p2)
            {
                var num = nums[p1];
                nums[p1] = nums[p2];
                nums[p2] = num;
            }
            while(j < end){
                if (nums[j] <= t)
                    Swap(i++, j++);
                else
                    ++j;
            }

            if (i - 1 > n) QuickSelect(nums, begin, i - 1, n);
            else if(i <= n) QuickSelect(nums, i, end, n);
        }

        
        
        //寻找旋转数组最小值 元素唯一
        public int FindRotateArrayMin(int[] nums) {
            var l = 0;
            var n = nums.Length;
            var r = n - 1;
            while(l < r){ // not use equal
                var m = l + ((r - l) >> 1);
                if(nums[m] > nums[r])
                    l = m + 1;
                else{
                    r = m;
                }
            }
            return nums[l];
        }

        public int FindMinInRotationArray(int[] nums)
        {
            var n = nums.Length;
            var left = 0;
            var right = n - 1;
            while (left < right){
                var mid = left + (right - left) / 2;
                if (nums[mid] > nums[right]){
                    // 中间数字大于右边数字，比如[3,4,5,1,2]，则左侧是有序上升的，最小值在右侧
                    left = mid + 1;
                }
                else if (nums[mid] < nums[right])
                {
                    right = mid; // 中间数字小于等于右边数字，比如[6,7,1,2,3,4,5]，则右侧是有序上升的，最小值在左侧
                }
                else
                {
                    /*中间数字等于右边数字，比如[2,3,1,1,1]或者[4,1,2,3,3,3,3]
                    # 则重复数字可能为最小值，也可能最小值在重复值的左侧
                    # 所以将right左移一位*/
                    right -= 1;
                }
            }
            return nums[left]; 
        }
        
        public int BinarySearchInRotateArray(int[] nums, int target){
            var n = nums.Length;
            var l = 0;
            var r = n - 1;
            while (l <= r){
                var m = l + (r - l) / 2;
                if(nums[m] == target)
                    return m;
                if(nums[0] <= nums[m]){
                    //左边有序
                    if(nums[0] <= target && target < nums[m])
                        r = m - 1;
                    else{
                        l = m + 1;
                    }
                }else{
                    //在右半有序内
                    if(nums[m] < target && target <= nums[n - 1]){
                        l = m + 1;
                    }else{
                        r = m - 1;
                    }
                }
            }
            return -1;
        }
        
        //旋转数组搜索，允许重复元素
        public int BinarySearchInRotateArray2(int[] nums, int target){
            var n = nums.Length;
            var l = 0;
            var r = n - 1;
            while (l <= r){
                var m = l + (r - l) / 2;
                if(nums[m] == target)
                    return m;
                if(nums[0] <= nums[m]){
                    //左边有序
                    if(nums[0] <= target && target < nums[m])
                        r = m - 1;
                    else{
                        l = m + 1;
                    }
                }else{
                    //在右半有序内
                    if(nums[m] < target && target <= nums[n - 1]){
                        l = m + 1;
                    }else{
                        r = m - 1;
                    }
                }
            }
            return -1;
            
            //允许重复的版本
            /*
             *    def search(self, nums: List[int], target: int) -> int:
        if not nums:
            return False
        l,r=0,len(nums)-1
        while l<=r:
            # 重点在于处理重复数字
            # 左边有重复数字，将左边界右移
            while l<r and nums[l]==nums[l+1]:
                l+=1
            # 右边有重复数字，将右边界左移
            while l<r and nums[r]==nums[r-1]:
                r-=1
            mid=(l+r)//2
            if nums[mid]==target:
                return True
            # 左半部分有序
            if nums[0]<=nums[mid]:
                if nums[0]<=target<nums[mid]:
                    r=mid-1
                else:
                    l=mid+1
            else:# 右半部分有序
                if nums[mid]<target<=nums[len(nums)-1]:
                    l=mid+1
                else:
                    r=mid-1
        return False
             *
             * 
             */
        }
        
        //旋转数组搜索左边界
        public int FindLefeEdgeInRotateArray(int[] nums, int t)
        {
            var n = nums.Length;
            var l = 0;
            var r = n - 1;
            while (l <= r)
            {
                var m = l + (r - l) / 2;
                if (nums[l] == t)
                {
                    return l; //重点1：当left符合时直接返回, 因为找的是最小的索引
                }

                if (nums[m] == t)
                {
                    r = m; // 重点2：当中间值等于目标值，将右边界移到中间，因为左边可能还有相等的值
                }
                //经典，先根据是左单调还是右单调判断。
                else if (nums[0] <= nums[m])
                {
                    if (nums[0] <= t && t < nums[m])
                        r = m - 1;
                    else
                        l = m + 1;
                }else if (nums[m] > nums[n - 1])
                {
                    if (nums[m] < t && t <= nums[n - 1])
                        l = m + 1;
                    else
                        r = m - 1;
                }
                else
                {
                    l++;
                }
                
            }

            return -1;
        }
    }
    
    
}


/*
 * assignment
	:	leftHandSide assignmentOperator expression
	;

leftHandSide
	:	expressionName
	|	fieldAccess
	|	arrayAccess
	;

assignmentOperator
	:	'='
	|	'*='
	|	'/='
	|	'%='
	|	'+='
	|	'-='
	|	'<<='
	|	'>>='
	|	'>>>='
	|	'&='
	|	'^='
	|	'|='
	;
 */