using System.Collections.Generic;
using System.Linq;

namespace Leetcode.StackA
{
    public class TreeNode {
         public int val;
         public TreeNode left;
         public TreeNode right;
         public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
             this.val = val;
             this.left = left;
             this.right = right;
         }
    }
    public class Complier
    {
        public TreeNode Str2tree(string s) {
            var nodeStack = new Stack<(TreeNode node, int depth)>();
            var numStack = new Stack<int>();
            var kkStack = new Stack<int>();
            kkStack.Push(0);
            foreach(var c in s){
                if (c >= '0' && c <= '9')
                {
                    numStack.Push(c - '0');
                }else if (c == '(')
                {
                    kkStack.Push(!nodeStack.Any() ? 1 : kkStack.Peek() + 1);
                }else if (c == ')')
                {
                    var newNode = (node: new TreeNode(numStack.Pop()),d: kkStack.Pop());
                    if (!nodeStack.Any() || nodeStack.Peek().depth == newNode.d)
                    {
                        //节点栈空，或者和栈顶节点深度相同的情况。直接入栈该节点
                        nodeStack.Push(newNode);
                    }
                    else
                    {
                        var leftNode = nodeStack.Pop().node;
                        var lrNode = new List<TreeNode>();
                        lrNode.Add(leftNode);
                        //如果与栈顶节点深度不同。
                        if (nodeStack.Any() && nodeStack.Peek().depth == newNode.d + 1)
                        {
                            lrNode.Add(nodeStack.Pop().node);
                        }

                        newNode.node.left = lrNode.Count == 1? lrNode[0] : lrNode[1];
                        newNode.node.right = lrNode.Count == 1? null : lrNode[0];
                        nodeStack.Push(newNode);
                    }
                    
                }
                
            }

            return nodeStack.Peek().node;
        }
    }
}