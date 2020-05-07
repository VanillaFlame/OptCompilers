using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public abstract class TACOptimizer
    {
        public ThreeAddressCode TAC { get; private set; }
        public List<TACInstruction> Instructions { get; set; }
        public TACOptimizer(ThreeAddressCode tac)
        {
            TAC = tac;
            Instructions = tac.Instructions;
        }
        public abstract void Run();
    }
}
