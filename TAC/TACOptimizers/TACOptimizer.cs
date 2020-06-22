using SimpleLang.CFG;
using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public abstract class TACOptimizer
    {

        public List<BasicBlock> Blocks { get; set; }
        public ControlFlowGraph Cfg { get; set; }
        public ThreeAddressCode TAC { get; protected set; }
        public List<TACInstruction> Instructions { get; set; }
        public TACOptimizer(ThreeAddressCode tac)
        {
            TAC = tac;
            Instructions = tac.Instructions;
        }

        public TACOptimizer(ThreeAddressCode tac, List<BasicBlock> blocks, ControlFlowGraph cfg)
        {
            TAC = tac;
            Instructions = tac.Instructions;
            Blocks = blocks;
            Cfg = cfg;
        }

        public TACOptimizer()
        {

        }

        public abstract void Run();
    }
}
