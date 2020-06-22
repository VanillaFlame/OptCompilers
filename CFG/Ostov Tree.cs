using System;
using System.Collections.Generic;
using System.Collections;
using SimpleLang.TAC;
namespace SimpleLang.CFG
{
    public class Ostov_Tree
    {
        public ControlFlowGraph _cfg;
        public List<IndexEdge> dfst = new List<IndexEdge>();//Остовное дерево
        Dictionary<int, bool> p = new Dictionary<int, bool>();// флаги для обхода узлов в алгоритме Traverse
        Dictionary<int, int> dfn = new Dictionary<int, int>();// Нумерация блоков в остовном дереве
        int c;//общее количество блоков
        public Ostov_Tree(ControlFlowGraph cfg)
        {
            _cfg = cfg;
            var cur_b = cfg.start;
            List<int> cheked = new List<int>();
            c =Block_Cheking(cur_b, cfg, cheked);//считаем количество блоков в ГПУ
            Traverse(cur_b, c);
        }

        int Traverse(BasicBlock n,int c)
        {
            p[n.Index] = true;
            foreach(BasicBlock s in n.Out)
                if(!p[s.Index]){
                    dfst.Add(new IndexEdge(n.Index, s.Index));
                    c=Traverse(s,c);
            }
            dfn[n.Index] = c--;
            return c;
        }

        private int Block_Cheking(BasicBlock cur_b, ControlFlowGraph cfg, List<int> cheked, int kol=0)
        {
            if (cur_b != cfg.end)
            {
                if (cur_b != cfg.start && !cheked.Contains(cur_b.Index))
                {
                    cheked.Add(cur_b.Index);
                    kol += 1;
                    foreach (var blok in cur_b.Out)
                    {
                        kol += Block_Cheking(blok, cfg, cheked);
                    }
                }                
            }
            return kol;
        }
    }
}
