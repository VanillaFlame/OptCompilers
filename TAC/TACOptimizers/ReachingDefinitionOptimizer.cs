using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.CFG;

namespace SimpleLang.TACOptimizers
{
    public class ReachingDefinitionOptimizer
    {
        public ControlFlowGraph graph;
        public Dictionary<BasicBlock, List<TACInstruction>> gen;
        public Dictionary<BasicBlock, List<TACInstruction>> kill;

        public Dictionary<BasicBlock, HashSet<TACInstruction>> IN;
        public Dictionary<BasicBlock, HashSet<TACInstruction>> OUT;

        public ReachingDefinitionOptimizer(List<BasicBlock> blocks)
        {
            graph = new ControlFlowGraph(blocks);
            gen = new Dictionary<BasicBlock, List<TACInstruction>>();
            kill = new Dictionary<BasicBlock, List<TACInstruction>>();

            IN = new Dictionary<BasicBlock, HashSet<TACInstruction>>();
            OUT = new Dictionary<BasicBlock, HashSet<TACInstruction>>();

        }

        public ReachingDefinitionOptimizer(ControlFlowGraph graph)
        {
            this.graph = graph;
            gen = new Dictionary<BasicBlock, List<TACInstruction>>();
            kill = new Dictionary<BasicBlock, List<TACInstruction>>();

            IN = new Dictionary<BasicBlock, HashSet<TACInstruction>>();
            OUT = new Dictionary<BasicBlock, HashSet<TACInstruction>>();

        }

        private void GetGenKillSet()
        {
            for (int i = 0; i < graph.blocks.Count; ++i)
            {
                List<TACInstruction> currGen = new List<TACInstruction>();
                HashSet<string> setGen = new HashSet<string>();
                graph.blocks[i].Instructions.Reverse();
                foreach (var instr in graph.blocks[i].Instructions)
                {
                    if (!setGen.Contains(instr.Result)
                        && !(instr.Result.Equals("")
                           || instr.Result.Contains("#")))
                    {
                        setGen.Add(instr.Result);
                        currGen.Add(instr);
                    }
                }
                graph.blocks[i].Instructions.Reverse();
                gen.Add(graph.blocks[i], currGen);
                List<TACInstruction> currKill = new List<TACInstruction>();
                for (int j = 0; j < graph.blocks.Count; ++j)
                {
                    if (j == i)
                        continue;

                    foreach (var instr in graph.blocks[j].Instructions)
                    {
                        if (setGen.Contains(instr.Result))
                        {
                            currKill.Add(instr);
                        }
                    }
                }
                kill.Add(graph.blocks[i], currKill);
            }
        }

        public void Run()
        {
            GetGenKillSet();
            IN[graph.start] = new HashSet<TACInstruction>();
            OUT[graph.start] = new HashSet<TACInstruction>();

            foreach (var block in graph.blocks)
            {
                OUT[block] = new HashSet<TACInstruction>();
            }

            bool change = true;
            while (change)
            {
                for (int i = 0; i < graph.blocks.Count; ++i)
                {
                    change = false;
                    var t = graph.blocks[i].In.SelectMany(n => OUT[n]);
                    IN[graph.blocks[i]] = new HashSet<TACInstruction>(t);
                    var prevOut = OUT[graph.blocks[i]];
                    OUT[graph.blocks[i]] = new HashSet<TACInstruction>(gen[graph.blocks[i]]);
                    OUT[graph.blocks[i]].UnionWith(IN[graph.blocks[i]].Except(kill[graph.blocks[i]]));
                    if (!prevOut.SetEquals(OUT[graph.blocks[i]]))
                        change = true;
                }
            }


        }
    }
}
