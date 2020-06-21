using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.CFG
{
    public class ReachingTransferFunc
    {
        private ILookup<string, TACInstruction> def_b;
        private ILookup<BasicBlock, TACInstruction> gen_b;
        private ILookup<BasicBlock, TACInstruction> kill_b;

        private void Defs(List<BasicBlock> blocks)
        {
            List<TACInstruction> defs = new List<TACInstruction>();
            foreach (var block in blocks)
                foreach (var instruction in block.Instructions)
                    if (instruction.Operation == "=")
                        defs.Add(instruction);
            def_b = defs.ToLookup(x => x.Result, x => x);
        }

        private void Kills(List<BasicBlock> blocks)
        {
            List<(BasicBlock, TACInstruction)> gen = new List<ValueTuple<BasicBlock, TACInstruction>>();
            List<(BasicBlock, TACInstruction)> kill = new List<ValueTuple<BasicBlock, TACInstruction>>();
            foreach (var block in blocks)
            {
                var flag = new HashSet<string>();
                foreach (var instruction in block.Instructions.Reverse<TACInstruction>())
                { 
                    if (!flag.Contains(instruction.Result) && instruction.Operation == "=")
                    {
                        gen.Add((block, instruction));
                        flag.Add(instruction.Result);
                    }
                    foreach (var exclude_def in def_b[instruction.Result].Where(x => x != instruction))
                        kill.Add((block, exclude_def));
                }
            }
            gen_b = gen.ToLookup(x => x.Item1, x => x.Item2);
            kill = kill.Distinct().ToList();
            kill_b = kill.ToLookup(x => x.Item1, x => x.Item2);
        }

        public IEnumerable<TACInstruction> ApplyTransferFunc(IEnumerable<TACInstruction> In, BasicBlock block) =>
           gen_b[block].Union(In.Except(kill_b[block]));
        public IEnumerable<TACInstruction> Transfer(BasicBlock basicBlock, IEnumerable<TACInstruction> input) =>
            ApplyTransferFunc(input, basicBlock);

        public ReachingTransferFunc(ControlFlowGraph g)
        {
            Defs(g.blocks);
            Kills(g.blocks);
        }

        public ReachingTransferFunc(List<BasicBlock> g)
        {
            Defs(g);
            Kills(g);
        }       
    }
}