using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.TAC;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleLang.TACOptimizers;
using SimpleLang.CFG;



namespace SimpleLang.TACOptimizers
{
    class ActiveVariableOptimizer
    {
        Dictionary<int, List<string>> In_Akt_Ver = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> Out_Akt_Ver = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> use_B = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> def_B = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> all_B = new Dictionary<int, List<string>>();
        ActiveVariableOptimizer(ControlFlowGraph cfg)
        {
            var cur_b = cfg.start;
            List<int> cheked = new List<int>();
            Find_Atktiv_Ver(cur_b, cfg, cheked);

        }

        void Find_Atktiv_Ver(BasicBlock cur_b, ControlFlowGraph cfg, List<int> cheked)
        {
            if (cur_b != cfg.end)
            {
                if (cur_b != cfg.start && !cheked.Contains(cur_b.Index))
                {
                    Filling(cur_b);
                    cheked.Add(cur_b.Index);
                    foreach (var blok in cur_b.Out)
                    {
                        Find_Atktiv_Ver(blok, cfg, cheked);
                    }
                }
            }
        }

        void Filling(BasicBlock cur_b)
        {
            all_B[cur_b.Index] = new List<string>();
            for (int i = 0; i < cur_b.Instructions.Count; ++i)
            {
                var arg1 = cur_b.Instructions[i].Argument1;
                var arg2 = cur_b.Instructions[i].Argument2;
                var res = cur_b.Instructions[i].Result;
                if (!all_B[cur_b.Index].Contains(arg1))
                {
                    if (arg1.Length > 0)
                        all_B[cur_b.Index].Add(arg1);
                }
                if (!all_B[cur_b.Index].Contains(arg2))
                {
                    if (arg2.Length > 0)
                        all_B[cur_b.Index].Add(arg2);
                }
                if (!all_B[cur_b.Index].Contains(res))
                {
                    if (arg1.Length > 0)
                        all_B[cur_b.Index].Add(res);
                }
            }
            for (int i = 0; i < all_B[cur_b.Index].Count; ++i)
            {
                bool flag_useble = true;
                var cur_arg = all_B[cur_b.Index][i];
                for (int k = cur_b.Instructions.Count - 1; k >= 0; --k)
                {
                    if ((cur_b.Instructions[k].Operation == "=") && ((cur_b.Instructions[k].Argument2 == cur_arg) ||
                        (cur_b.Instructions[k].Argument1 == cur_arg)))
                    {
                        flag_useble = false;
                    }
                    if ((cur_b.Instructions[k].Operation == "=") && (cur_b.Instructions[k].Result == cur_arg))
                    {
                        if (!def_B[cur_b.Index].Contains(cur_arg))
                        {
                            if (cur_arg.Length > 0)
                                def_B[cur_b.Index].Add(cur_arg);
                        }
                        if (flag_useble && !use_B[cur_b.Index].Contains(cur_arg))
                        {
                            if (cur_arg.Length > 0)
                                use_B[cur_b.Index].Add(cur_arg);
                        }
                    }

                }
            }
        }
    }

}
