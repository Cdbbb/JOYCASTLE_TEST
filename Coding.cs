using System;
using System.Collections.Generic;
using System.Linq;

namespace JOYCASTLE
{

    public class TopScore
    {
        public class CommentSet
        {
            /*****************************/
            //使用小根堆维护数组最大的前k个数
            //堆顶是当前排行榜最小的分数,如果新加入数据比当前最小分数大则弹出堆顶并加入新分数重排
            //空间复杂度O(m)
            //时间复杂度均摊为O(n*log(n) + m*log(m)),由堆的重排和结算时依次弹出产生
            /*****************************/
            //进阶1:
            //即使数量到达百万, 最大开销也只是对m规模的堆比较堆顶或重排
            //产生新数据时由一次计算并分发到查看排行榜的客户端
        }
        public class LeaderboardSystem
        {
            public static List<int> GetTopScores(int[] scores, int m)
            {
                PriorityHeap<int, int> topScores = new PriorityHeap<int, int>(m);
                //PriorityQueue<int,int> topScores = new PriorityQueue<int,int>();
                for (int i = 0; i < scores.Length; i++)
                {
                    int curScore = scores[i];
                    if (topScores.Count < m) topScores.Enqueue(scores[i], scores[i]);
                    else if (topScores.Count > 0 && curScore > topScores.Peek())
                    {
                        topScores.Dequeue();
                        topScores.Enqueue(scores[i], scores[i]);
                    }
                }
                int scoreCapcity = Math.Min(topScores.Count, m);
                int[] ans = new int[scoreCapcity];
                int ai = scoreCapcity;
                while (topScores.Count != 0)
                {
                    ans[--ai] = topScores.Dequeue();
                }
                return new List<int>(ans);
            }

            public static void TestGetTopScores()
            {
                int testCount = 1000;
                int errorCount = 0;
                for (int i = 0; i < testCount; i++)
                {
                    Random random = new Random();
                    int n = random.Next(0, 1000);
                    int m = random.Next(0, 1000);
                    int[] sample = new int[n];
                    for (int j = 0; j < n; j++)
                    {
                        sample[j] = random.Next(0, 1000);
                    }
                    var ForceAns = ForceGetTopScores(sample, m);
                    var MyAns = GetTopScores(sample, m);

                    for (int j = 0; j < MyAns.Count; j++)
                    {
                        int debugF = ForceAns[j];
                        int debugM = MyAns[j];
                        if (debugF != debugM)
                        {
                            errorCount++; break;
                        }
                    }

                }
                Console.WriteLine($"GetTopScores测试 Error:{errorCount} Right:{testCount - errorCount}");
            }
            public static List<int> ForceGetTopScores(int[] scores, int m)
            {
                var copy = scores.OrderByDescending(x => x).ToArray();
                int vaild = Math.Min(scores.Length, m);
                List<int> result = new List<int>(vaild);
                for (int i = 0; i < vaild; i++)
                {
                    result.Add(copy[i]);
                }
                return result;

            }
        }

        public class PriorityHeap<Telement, TPriority>
        {

            public Telement[] iheap;
            public TPriority[] iPriority;
            public int Count;
            public int capicity;
            public Comparer<TPriority> cp;
            public PriorityHeap(int n = 10, Comparer<TPriority> comparer = null)
            {
                capicity = n;
                iheap = new Telement[n];
                iPriority = new TPriority[n];
                if (comparer == null)
                    this.cp = Comparer<TPriority>.Default;
                else
                    this.cp = comparer;
            }
            public void Enqueue(Telement element, TPriority priority)
            {
                if (Count == capicity) throw new InvalidOperationException("Heap is Full! Expend Capcity");
                iheap[Count] = element;
                iPriority[Count++] = priority;
                UpHeapify();
            }
            private void UpHeapify()
            {
                int index = Count - 1;
                while (index > 0 && cp.Compare(iPriority[(index - 1) / 2], iPriority[index]) >= 0)
                {
                    Swap(index, (index - 1) / 2);
                    index = (index - 1) / 2;
                }
            }
            private void Swap(int i, int j)
            {
                (iheap[i], iheap[j]) = (iheap[j], iheap[i]);
                (iPriority[i], iPriority[j]) = (iPriority[j], iPriority[i]);
            }
            private void DownHeapify(int index = 0)
            {
                int lesser = index * 2 + 1;
                while (lesser < Count)
                {
                    ;
                    lesser = lesser + 1 < Count && cp.Compare(iPriority[lesser + 1], iPriority[lesser]) < 0 ? lesser + 1 : lesser;
                    lesser = cp.Compare(iPriority[index], iPriority[lesser]) > 0 ? lesser : index;
                    if (index == lesser) return;
                    Swap(lesser, index);
                    index = lesser;
                    lesser = index * 2 + 1;
                }
            }
            public Telement Dequeue()
            {
                if (Count == 0) throw new InvalidOperationException("heap is empty!");
                Telement ans = iheap[0];
                Swap(0, --Count);
                DownHeapify(0);
                return ans;
            }
            public Telement Peek()
            {
                return iheap[0];
            }
            public void Clear()
            {
                Count = 0;
            }
        }


    }

    public class EnergyField
    {
        public class EnergyFieldSystem
        {
            public class CommentSet
            {
                /*****************************/
                //先遍历一次左右区间找到区间最大值索引
                //梯形最大值只会在两端和两个最大值围成的区间内出现
                //空间复杂度 O(1) , 常数个变量
                //时间复杂度最坏 O(n*n /4 ) ,当两个区间最大值都在n/2邻域出现
                //感觉可以用单调队列做, 但是无奈分析不出 (r-l)*(h(r) + h(l))的单调性 ,屡次尝试后作罢...
                /*****************************/
                //进阶1:
                //如果是在计算中修改内存中的数据:
                //更新时首先判断在左区间或右区间, 如果高度超过区间最大值则修改相应的l,r索引, 否则忽视
                //进阶2:
                //本算法可以涵盖这种情况
                /*****************************/
                //这要求玩家在探索到尽可能多的能量塔之后再建设能量场,是对玩家决策时机的考察
                //==>相当于"决策窗口内收益最大化"
                //地图中随机产生不同程度能倍化增益的时限buff,兼具玩家的决策力,随机性,爽感和buff过期后希望再开一把的成瘾性
            }

            public static float MaxEnergyFieldQueue(int[] heights)
            {
                int n = heights.Length;
                int[] q = new int[n];
                float ans = 0;
                int l = 0, r = 0;
                for (int i = 0; i < n; i++)
                {
                    while (r > l && (heights[q[r - 1]] + heights[q[l]]) * q[r - 1] >= i * (heights[i] + heights[q[l]]))
                    {
                        int curI = q[--r];
                        float curArea = curI - q[l];
                        curArea = (curArea / 2) * (heights[curI] + heights[q[l]]);
                        ans = Math.Max(curArea, ans);
                    }
                    q[r++] = i;
                }

                while (r-- > l)
                {
                    int curI = q[r--];
                    float curArea = curI - q[l];
                    curArea = (curArea / 2) * (heights[curI] + heights[q[l]]);
                    ans = Math.Max(curArea, ans);
                }
                return ans;
            }

            public static float MaxEnergyField(int[] heights)
            {
                int n = heights.Length;
                if (n == 0 || n == 1) return 0;
                float ans = 0;
                int lmax = 0, rmax = n - 1;
                for (int i = 0; i <= n / 2; i++)
                {
                    if (heights[lmax] < heights[i]) lmax = i;
                }
                for (int i = n / 2; i < n; i++)
                {
                    if (heights[rmax] < heights[i]) rmax = i;
                }
                for (int l = 0; l <= lmax; l++)
                {
                    for (int r = rmax; r < n; r++)
                    {
                        float curArea = (r - l) / 2f;
                        curArea = curArea * (heights[r] + heights[l]);
                        ans = Math.Max(ans, curArea);
                    }
                }
                return ans;
            }

            public static float MaxEnergyFieldlr(int[] heights)
            {
                int l = 0; int r = heights.Length - 1;
                float ans = 0;
                while (l < r)
                {
                    float curArea = (float)(r - l) * (heights[l] + heights[r]) / 2f;
                    ans = Math.Max(ans, curArea);
                    if (heights[l] < heights[r])
                    {
                        l++;
                    }
                    else
                    {
                        r--;
                    }
                }
                return ans;
            }


            public static void TestEnergyField()
            {
                int testCount = 1000;
                int errorCount = 0;
                for (int i = 0; i < testCount; i++)
                {
                    Random random = new Random();
                    int n = random.Next(0, 100);
                    int[] sample = new int[n];
                    for (int j = 0; j < n; j++)
                    {
                        sample[j] = random.Next(0, 100);
                    }
                    var ForceAns = ForceMaxEnergyField(sample);
                    var MyAns = MaxEnergyField(sample);

                    if (!ForceAns.Equals(MyAns))
                        errorCount++;

                }
                Console.WriteLine($"MaxEnergyField测试: Error:{errorCount} Right:{testCount - errorCount}");
            }
            public static float ForceMaxEnergyField(int[] heights)
            {
                int n = heights.Length;
                float max = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        max = Math.Max(max, Math.Abs(i - j) * (float)(heights[i] + heights[j]) / 2f);
                    }
                }
                return max;
            }
        }
    }

    public class TreasureHunt
    {
        public class CommentSet
        {
            /*****************************/
            //打家劫舍问题 ,一维动态规划可以压缩为常数个变量
            //空间复杂度 O(1)
            //时间复杂度 O(n) 一次遍历, 维护pre和prepre
            /*****************************/
            //进阶1:
            //用二维数组维护每个宝箱是否使用钥匙的情况, 最终两者状态的最大值
            //进阶2:
            //在dp数组初始化时考虑负数的情况, 后续只需要延续前一状态即可
            /*****************************/
            //本机制涉及 风险-收益曲线的调整, 要么挂重debuff+高收益刀尖舔血,要么普通收益普通风险励志局一把
            //例如有不同品质的宝箱和陷阱相连出现, 并伴随游戏进程给予钥匙和保护屏障
            //玩家会根据自己承担风险的阈值来做出不同选择进而影响游戏走向

        }
        public class TreasureHuntSystem
        {
            public static int MaxTreasureValue(int[] treasures)
            {
                int n = treasures.Length;
                if (n == 1) { return treasures[0]; }
                int prepre = Math.Max(treasures[0], 0);
                int pre = Math.Max(prepre, 0);
                for (int i = 2; i < n; i++)
                {
                    int cur = Math.Max(prepre + treasures[i], pre);
                    prepre = pre;
                    pre = cur;
                }
                return pre;
            }

            public static void TestMaxTreasureValue()
            {
                //暴力递归会爆栈...先略一下
                Console.WriteLine($"MaxTreasureValue测试 Error:0 Right:0");
            }

            public static int MaxTreasureValue2D(int[] treasures)
            {
                int n = treasures.Length;
                if (n == 1) { return treasures[0]; }
                int[,] dp = new int[n, 2];
                dp[0, 0] = Math.Max(treasures[0], 0);
                dp[0, 1] = Math.Max(treasures[0], 0);
                dp[1, 0] = Math.Max(dp[0, 0], 0);
                dp[1, 1] = Math.Max(dp[0, 0] + treasures[1], dp[0, 0] + 0);
                for (int i = 2; i < n; i++)
                {
                    dp[i, 0] = Math.Max(dp[i - 1, 0], dp[i - 2, 0] + treasures[i]);
                    dp[i, 1] = Math.Max(dp[i - 1, 1], dp[i - 1, 0] + treasures[i]);
                }
                return Math.Max(dp[n - 1, 0], dp[n - 1, 1]);
            }

        }
    }

    public class TalentAssessment
    {
        public class CommentSet
        {
            /*****************************/
            //利用数组有序性, 每次排除k/=2个元素, 到达边界情况时可直接求解
            //空间复杂度 O(1) 常数个变量
            //时间复杂度 O(log(m+n))  每次迭代排除k/=2个元素, k∈(1,m+n)
            /*****************************/
            //进阶1:
            //如果是需要更新的动态结构, 使用对顶堆各自维护一半大和一半小的元素
            //根据中位数的奇偶性和堆的情况弹出堆顶元素计算即可
            //对于删除操作则可以使用索引堆来根据学徒的唯一标识(如数组下标)在堆中删除即可
            //下面贴了一个自己实现的
            //进阶2:
            //比较直观的做法还是堆维护,当前做法两个衍生到K个的的情况还是不太好想Orz
            /*****************************/
            //从玩家角度出发, 如果想尽可能提升自己的魔法能力值中位数, 就需要将有限的技能点集中到几个魔法中
            //花费太多点数在高阶魔法上并没有办法改变中位数, 而点出低阶魔法又容易拉低中位数
            //通常用中位数来代表一个系统容易达到的水平, 可以用来衡量一个地图中任务的报酬量级,或者一个战区玩家的普遍水平
        }
        public class TalentAssessmentSystem
        {
            public static double FindMedianTalentIndex(int[] fireAbility, int[] iceAbility)
            {
                int flen = fireAbility.Length;
                int ilen = iceAbility.Length;
                int totalLength = flen + ilen;
                if (totalLength % 2 == 0)
                    return (GetK(totalLength / 2) + GetK(totalLength / 2 + 1)) / 2.0;
                else
                    return GetK(totalLength / 2 + 1);

                int GetK(int k)
                {
                    int fireI = 0, iceI = 0;

                    while (true)
                    {
                        // 边界情况
                        if (fireI == flen) return iceAbility[iceI + k - 1];
                        if (iceI == ilen) return fireAbility[fireI + k - 1];
                        if (k == 1) return Math.Min(fireAbility[fireI], iceAbility[iceI]);

                        int nextFireI = Math.Min(fireI + k / 2, flen) - 1;
                        int nextIceI = Math.Min(iceI + k / 2, ilen) - 1;
                        if (fireAbility[nextFireI] <= iceAbility[nextIceI])
                        {
                            k -= (nextFireI - fireI + 1);
                            fireI = nextFireI + 1;
                        }
                        else
                        {
                            k -= (nextIceI - iceI + 1);
                            iceI = nextIceI + 1;
                        }
                    }
                }
            }

            public static void TestFindMedianTalentIndex()
            {
                int testCount = 10000;
                int errorCount = 0;
                for (int i = 0; i < testCount; i++)
                {
                    Random random = new Random();
                    int n = random.Next(0, 1000);
                    int m = random.Next(0, 1000);
                    int[] sample1 = new int[n];
                    int[] sample2 = new int[m];
                    for (int j = 0; j < n; j++)
                    {
                        sample1[j] = random.Next(0, 1000);
                    }
                    for (int j = 0; j < m; j++)
                    {
                        sample2[j] = random.Next(0, 1000);
                    }
                    var ForceAns = ForceFindMedianTalentIndex(sample1, sample2);
                    Array.Sort(sample1);
                    Array.Sort(sample2);
                    var MyAns = FindMedianTalentIndex(sample1, sample2);

                    if (MyAns != ForceAns) errorCount++;

                }
                Console.WriteLine($"FindMedianTalentIndex测试 Error:{errorCount} Right:{testCount - errorCount}");
            }
        }
        public static double ForceFindMedianTalentIndex(int[] fireAbility, int[] iceAbility)
        {
            int[] abilities = new int[fireAbility.Length + iceAbility.Length];
            Array.Copy(fireAbility, abilities, fireAbility.Length);
            Array.Copy(iceAbility, 0, abilities, fireAbility.Length, iceAbility.Length);
            Array.Sort(abilities);
            if (abilities.Length % 2 == 0)
            {
                double l = abilities[abilities.Length / 2];
                l = (l + abilities[abilities.Length / 2 - 1]) / 2.0;
                return l;
            }
            else
                return abilities[abilities.Length / 2];
        }

        public class ArrayBaseHeap<T>
        {
            public struct vpPairs<vpT>
            {
                public int vpI;
                public vpT pri;
                public vpPairs(int vpi, vpT pri)
                {
                    this.vpI = vpi;
                    this.pri = pri;
                }
            }

            public vpPairs<T>[] iheap;
            public int count;
            public Dictionary<int, int> where;
            public Comparer<T> cp;


            public ArrayBaseHeap(int n, Comparer<T> comparer)
            {
                where = new Dictionary<int, int>(n);
                iheap = new vpPairs<T>[n];
                this.cp = comparer;
            }
            public void Enqueue(int i, T pri)
            {
                iheap[count] = new vpPairs<T>(i, pri);
                where[i] = count;
                UpHeapify(count++);
            }
            private void UpHeapify(int index)
            {
                while (index > 0 && cp.Compare(iheap[(index - 1) / 2].pri, iheap[index].pri) >= 0)
                {
                    Swap(index, (index - 1) / 2);
                    index = (index - 1) / 2;
                }
            }
            private void Swap(int i, int j)
            {
                where[iheap[i].vpI] = j;
                where[iheap[j].vpI] = i;
                (iheap[i], iheap[j]) = (iheap[j], iheap[i]);
            }
            private void DownHeapify(int index)
            {
                int lesser = index * 2 + 1;
                while (lesser < count)
                {
                    ;
                    lesser = lesser + 1 < count && cp.Compare(iheap[lesser + 1].pri, iheap[lesser].pri) < 0 ? lesser + 1 : lesser;
                    lesser = cp.Compare(iheap[index].pri, iheap[lesser].pri) > 0 ? lesser : index;
                    if (index == lesser) return;
                    Swap(lesser, index);
                    index = lesser;
                    lesser = index * 2 + 1;
                }
            }
            public int Dequeue()
            {
                if (count == 0) throw new InvalidOperationException("heap is empty!");
                int ans = iheap[0].vpI;
                Swap(0, --count);
                where.Remove(ans);
                DownHeapify(0);
                return ans;
            }

            public int Peek()
            {
                return iheap[0].vpI;
            }
            public void Remove(int index)
            {
                if (where.ContainsKey(index))
                {
                    int heapIndex = where[index];
                    Swap(heapIndex, --count);
                    where.Remove(index);
                    UpHeapify(heapIndex);
                    DownHeapify(heapIndex);
                }
            }
            public void Clear()
            {
                count = 0;
                where.Clear();
            }
        }

    }

}
