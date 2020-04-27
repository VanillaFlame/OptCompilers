using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public abstract class TACOptimizer
    {
        public List<TACInstruction> Instructions { get; set; }
        public TACOptimizer(List<TACInstruction> instructions)
        {
            Instructions = instructions;
        }
        public abstract void Run();
    }
}
