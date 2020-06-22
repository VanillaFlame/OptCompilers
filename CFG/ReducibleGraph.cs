using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLang.TAC;

namespace SimpleLang.CFG
{
    public static class ReducibleGraph
    {
        public static List<(BasicBlock, BasicBlock)> backEdges { get; private set; }
        private static ControlFlowGraph controlFlowGraph;
        public static void GetBackEdges(ControlFlowGraph cfg, Dictionary<BasicBlock, List<BasicBlock>> dominators)
        {
            backEdges = TACUtils.GetBackEdges(cfg, dominators);
            controlFlowGraph = cfg; 
        }

        public static bool IsReducible(ControlFlowGraph cfg, Dictionary<BasicBlock, List<BasicBlock>> dominators)
        {
            GetBackEdges(cfg, dominators);
            var edgesType = CFGExtension.classifyEdges(cfg);
            foreach (var x in edgesType)
            {
                if (x.Value.Equals(EdgeType.Retreating))
                {
                    bool flag = false;
                    foreach (var b in backEdges)
                    {
                        if (x.Key.Equals(new IndexEdge(b.Item1.Index, b.Item2.Index)))
                            flag = true;
                    }
                    if (!flag)
                        return false;
                }
            }
            return true;
        }
    }
}
