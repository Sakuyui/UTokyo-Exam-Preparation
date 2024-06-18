using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;

namespace LeetCode.DifficalProblem
{
    public class SearchProblem1
    {



        public static void TestSolver()
        {
            SimplexSolver solver = new SimplexSolver();
            int p1, p2;
            //4x + 6y <= 25
            //3x + 2y <= 10
            // max_r 14500x+9800y=r
            solver.AddVariable("x", out p1);
            solver.AddVariable("y", out p2);
            solver.SetBounds(p1, 0, 10000);
            solver.SetBounds(p2, 0, 10000);
            
            int flour;
            int sugar;
            //约束条件行
            solver.AddRow("小麦粉", out flour);
            solver.AddRow("砂糖", out sugar);
            
            solver.SetBounds(flour, 0, 25);
            solver.SetBounds(sugar, 0, 10);

            solver.SetCoefficient(flour, p1, 4);
            solver.SetCoefficient(sugar, p1, 3);

            solver.SetCoefficient(flour, p2, 6);
            solver.SetCoefficient(sugar, p2, 2);
            
            int price;
            solver.AddRow("販売価格", out price);
            solver.SetCoefficient(price, p1, 14500);
            solver.SetCoefficient(price, p2, 9800);
            solver.AddGoal(price, 1, false);

            solver.Solve(new SimplexSolverParams());
            
            // textBox1.Text += string.Format("ぺんぎんクッキー:{0:f} , らくだサブレ:{1:f}\r\n",simplex.GetValue(p1).ToDouble(), simplex.GetValue(p2).ToDouble());
            // textBox1.Text += string.Format("販売価格合計:{0:f}\r\n", simplex.GetValue(price).ToDouble(), simplex.GetValue(p2).ToDouble());
            // textBox1.Text += string.Format("小麦粉使用量:{0:f} , 砂糖使用量:{1:f}\r\n", simplex.GetValue(flour).ToDouble(), simplex.GetValue(sugar).ToDouble());

            
        }

        public static void SolverTest2()
        {
              // 解く問題を初期化(IDisposableではない)
            var solver = new SimplexSolver();
            // 使用する変数のID・制約式のID・目的関数のIDを宣言する
            int x, y, z = 0, e1, e2, e3;
            // 最適化の方向を設定する
            // AddGoal(目的関数の数値が代入される変数のID, (不明), 最大化するならfalse・最小化するならtrue)
            solver.AddRow("目的関数値", out z);
            solver.AddGoal(z, 1, false);
            // 制約式の数・名前・範囲
            solver.AddRow("条件1", out e1);
            solver.AddRow("条件2", out e2);
            solver.AddRow("条件3", out e3);
            solver.SetBounds(e1, double.NegativeInfinity, 13.5);
            solver.SetBounds(e2, double.NegativeInfinity,10.0);
            solver.SetBounds(e3, 7.0, double.PositiveInfinity);
            // 変数の数・名前・範囲
            // SetIntegralityメソッドで整数条件を付与できることがポイント
            solver.AddVariable("X", out x);
            solver.AddVariable("Y", out y);
            solver.SetBounds(x, 0.0, double.PositiveInfinity);
            solver.SetBounds(y, 0.0, double.PositiveInfinity);
            solver.SetIntegrality(x, true);　//整数规划
            solver.SetIntegrality(y, true);
            // 目的関数の係数
            solver.SetCoefficient(z, x, 5);
            solver.SetCoefficient(z, y, 4);
            // 制約式の係数
            solver.SetCoefficient(e1, x, 1.5);
            solver.SetCoefficient(e1, y, 3);
            solver.SetCoefficient(e2, x, 3);
            solver.SetCoefficient(e2, y, 1);
            solver.SetCoefficient(e3, x, 1);
            solver.SetCoefficient(e3, y, 2);
            // 最適化
            solver.Solve(new SimplexSolverParams());
            // 結果表示
            // GetValueメソッドの返り値はRational型……要するに分数なので、
            // ToDouble()メソッドでdouble型にすると分かりやすい
            Console.WriteLine($"Z = {solver.GetValue(z).ToDouble()}");
            for (int i = 0; i < solver.VariableCount; ++i) {
                int valueId = solver.VariableIndices.ElementAt(i);
                string valueName = (string)solver.VariableKeys.ElementAt(i);
                Console.WriteLine($"{valueName} = {solver.GetValue(valueId).ToDouble()}");
            }
            
            
            
            solver = new SimplexSolver();
 
            // 答えは製品1を[x1]コ、製品2を[x2]コとする。
            // それぞれの変域制限も可能、ここでは0-100で設定しておく。
            int x1, x2;
            solver.AddVariable("製品1", out x1);
            solver.SetBounds(x1, 0, 100);
            solver.AddVariable("製品2", out x2);
            solver.SetBounds(x2, 0, 100);
 
            // 製品1, 2の特徴である原材料A, Bと価格の行を作成。
            int zairyouA, zairyouB, price;
            solver.AddRow("材料A", out zairyouA);
            solver.AddRow("材料B", out zairyouB);
            solver.AddRow("価格", out price);
            // 材料A: 製品1は1kg使う、製品2は1kg使う、トータル4kgまで。
            solver.SetCoefficient(zairyouA, x1, 1);
            solver.SetCoefficient(zairyouA, x2, 1);
            solver.SetBounds(zairyouA, 0, 4);
            // 材料B: 製品1は3kg使う、製品2は1kg使う、トータル6kgまで。
            solver.SetCoefficient(zairyouB, x1, 3);
            solver.SetCoefficient(zairyouB, x2, 1);
            solver.SetBounds(zairyouB, 0, 6);
 
            // 価格(売上)を最大化するのが目的なのでAddGoalにfalseを指定する。
            // 最小化したい場合はtrueを指定
            solver.SetCoefficient(price, x1, 8);
            solver.SetCoefficient(price, x2, 6);
            solver.AddGoal(price, 1, false);

            
            SolverContext context = new SolverContext();
            Model model = context.CreateModel();

            //変数の作成
            Decision b2 = new Decision(Domain.Integer, "b2");
            Decision b3 = new Decision(Domain.Integer, "b3");
            Decision b4 = new Decision(Domain.Integer, "b4");

            //modelに変数を追加
            model.AddDecision(b2);
            model.AddDecision(b3);
            model.AddDecision(b4);

            //制約追加
            model.AddConstraint("limit1", 10 <= b2 <= 30);
            model.AddConstraint("limit2", 2 <= b3 <= 10);
            model.AddConstraint("limit3", 2 <= b4 <= 100);
            model.AddConstraint("limit4", b2 / b3 <= b4 <= b2 / b3);

            //ゴールの設定
            model.AddGoal("Goal", GoalKind.Maximize, b4);

            // Solution
            Solution solution = context.Solve();
            Report report = solution.GetReport();
            Console.WriteLine(report);
            
            /*
             * ===Solution Details===
Goals:
Goal: 15

Decisions:
b2: 30
b3: 2
b4: 15
             */
        }
        
        /*
         *class Solution:
    def domino(self, n: int, m: int, broken: List[List[int]]) -> int:
        #相邻坐标（r,c）的和，奇偶性不同。 所以转化为二部图，求增广路径（折过来，着过去，W型路线）
        Row, Col = n, m

        match = [[None for _ in range(Col)] for _ in range(Row)]
        for r, c in broken:
            match[r][c] = '#'           #这个点不能被匹配

        def dfs(r: int, c: int, visited: set()) -> bool:        #find增广路径  模板 visit直接在参数里传递了
            visited.add((r, c))
            for nr, nc in ((r-1,c), (r+1,c), (r,c-1), (r,c+1)): #有连接的边
                if 0<= nr < Row and 0<= nc < Col:               #有连接的边
                    nxt = match[nr][nc]     #寻找增广路径

                    if nxt in visited:      #没访问过，且不是坑（坑已经在visited里面了）
                        continue
                    if nxt == None or dfs(nxt[0], nxt[1], visited) == True: #如果还没配对，或者配的对可以找到增广路径
                        match[r][c] = (nr, nc)
                        match[nr][nc] = (r, c)
                        return True
            return False

        res = 0
        for r in range(Row):
            for c in range(Col):
                if (r + c) % 2 == 0:            #从偶集开始，从奇数集开始也行
                    if (match[r][c] != '#'):
                        if dfs(r, c, {'#'}) == True:
                            res += 1
        return res
         * 
         */
        
        //最接近目标和子序列
        /*
         * int minAbsDifference(vector<int>& nums, int goal) {
        int n = sz(nums);
        vt<int> f1, f2; // 分别存前半部分和后半部分
        for (int i = 0; i < n / 2; ++ i) f1.pb(nums[i]);
        for (int i = n / 2; i < n; ++ i) f2.pb(nums[i]);
        int pr = n / 2, af = n - n / 2; // pr->pre, af->after
        
        vt<int> task;
        for (int mask = 0; mask < (1 << pr); ++ mask){
            int sum = 0;
            for (int b = 0; b < pr; ++ b){
                if ((mask >> b) & 1) sum += f1[b];
            }
            task.pb(sum); // 枚举可能的子集并归入一个数组内
        }
        sort(all(task));
        
        int ans = inf;
        for (int mask = 0; mask < (1 << af); ++ mask){
            int sum = 0;
            for (int b = 0; b < af; ++ b){
                if ((mask >> b) & 1) sum += f2[b];
            }
            // sum + 剩余 = goal, 剩余 = goal - sum
            auto tar = lb(all(task), goal - sum);
            if (tar != task.end()) ans = min(ans, abs(sum + *tar - goal)); // 大于等于部分 *tar >= goal - sum --> sum + *tar >= goal
            if (tar != task.begin()) ans = min(ans, abs(sum + *prev(tar) - goal)); // 小于部分
        }
        return ans;
    }
         * 
         */
        //摘樱桃。从0，0到n-1,n-1，再返回。能够最多摘多少
        public int CherryPickup(int[][] grid) {
            //假设有两个人在走，最终结果等于两个人从(0,0)走到(n-1,n-1)的路径和的最大值
            var n = grid.Length;
            var dp = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(-1, n).ToArray(),n).ToArray(),n).ToArray();
                // int[n][n][n] = {-1...};

                int Dfs(int i, int j, int k)
                {
                    var l = i + j - k; //获取第二个人的y坐标。走t步一定i + j = t
        
                    //边界条件，无法走的情况
                    if (i >= n || j >= n || k >= n || l >= n || l < 0|| grid[i][j] == -1 || grid[k][l] == -1)
                        return int.MinValue + 100; //防溢出
        
                    if (dp[i][j][k] != -1) 
                        return dp[i][j][k]; //记忆化
        
                    if (i == n - 1 && j == n - 1 && k == n - 1) 
                        return dp[i][j][k] = grid[i][j];
        
                    var res = 
                        System.Math.Max(System.Math.Max(Dfs(i + 1, j, k + 1), Dfs( i + 1, j, k)), 
                            System.Math.Max(Dfs(i, j + 1, k + 1), Dfs(i, j + 1, k)));
                    
                    res += grid[i][j] + grid[k][l] + (i == k && j == l && grid[i][j] == 1 ? -1 : 0);
                    return dp[i][j][k] = res; 
                }
                
            return System.Math.Max(0, Dfs(0, 0, 0));
        
        }


        public static void Sat()
        {
            ConstraintSystem s1 = ConstraintSystem.CreateSolver();

            CspTerm t1 = s1.CreateBoolean("v1");
            CspTerm t2 = s1.CreateBoolean("v2");
            CspTerm t3 = s1.CreateBoolean("v3");
            CspTerm t4 = s1.CreateBoolean("v4");
            
            CspTerm tOr12 = s1.Or(s1.Not(t1), s1.Not(t2));
            CspTerm tOr13 = s1.Or(s1.Not(t1), s1.Not(t3));
            CspTerm tOr14 = s1.Or(s1.Not(t1), s1.Not(t4));

            CspTerm tOr23 = s1.Or(s1.Not(t2), s1.Not(t3));
            CspTerm tOr24 = s1.Or(s1.Not(t2), s1.Not(t4));

            CspTerm tOr34 = s1.Or(s1.Not(t3), s1.Not(t4));

            CspTerm tOr = s1.Or(t1, t2, t3, t4);


            
            s1.AddConstraints(tOr12);
            s1.AddConstraints(tOr13);
            s1.AddConstraints(tOr14);
            s1.AddConstraints(tOr23);
            s1.AddConstraints(tOr24);
            s1.AddConstraints(tOr34);
            s1.AddConstraints(tOr);

            ConstraintSolverSolution solution1 = s1.Solve();
        }
    }
}