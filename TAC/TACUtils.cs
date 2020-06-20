using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLang.CFG;

namespace SimpleLang.TAC
{
    public static class TACUtils
    {
        public static List<(BasicBlock, BasicBlock)> GetBackEdges(ControlFlowGraph cfg, Dictionary<BasicBlock, List<BasicBlock>> dominators) 
        {
            var result = new List<(BasicBlock, BasicBlock)>();
            var queue = new Queue<BasicBlock>();
            var used = new HashSet<(BasicBlock, BasicBlock)>();
            queue.Enqueue(cfg.start);
            while (queue.Count != 0) //просто обход в ширину
            {
                var cur = queue.Peek();
                queue.Dequeue();
                foreach (var edgeEnd in cur.Out)
                {
                    if (edgeEnd == cfg.start || edgeEnd == cfg.end)
                    {
                        continue;
                    }

                    if (!used.Contains((cur, edgeEnd)))
                    {
                        if (dominators.ContainsKey(cur) && dominators[cur].Contains(edgeEnd)) //обратное ребро
                        {
                            result.Add((cur, edgeEnd));
                        }
                        used.Add((cur, edgeEnd));
                        queue.Enqueue(edgeEnd);
                    }
                }
            }
            return result;
        }

        public static List<NaturalLoop> GetNaturalLoops(ControlFlowGraph cfg, Dictionary<BasicBlock, List<BasicBlock>> dominators)
        {
            var result = new List<NaturalLoop>();
            var backEdges = GetBackEdges(cfg, dominators);
            foreach (var edge in backEdges)
            {
                result.Add(GetNaturalLoop(cfg, edge));
            }
            return result;
        }

        public static NaturalLoop GetNaturalLoop(ControlFlowGraph cfg, (BasicBlock, BasicBlock) edge)
        {
            var result = new NaturalLoop(edge.Item2, edge.Item1);
            var used = new HashSet<BasicBlock>();
            used.Add(edge.Item2);
            used.Add(edge.Item1);
            var queue = new Queue<BasicBlock>();
            queue.Enqueue(edge.Item1);
            while (queue.Count != 0) //просто обход в ширину
            {
                var cur = queue.Peek();
                queue.Dequeue();
                result.Blocks.Add(cur);
                foreach (var edgeStart in cur.In)
                {
                    if (edgeStart == cfg.start || edgeStart == cfg.end)
                    {
                        continue;
                    }

                    if (!used.Contains(edgeStart))
                    {
                        used.Add(edgeStart);
                        queue.Enqueue(edgeStart);
                    }
                    
                }
            }
            result.AddBlock(edge.Item2);
            return result;
        }
    }
}
