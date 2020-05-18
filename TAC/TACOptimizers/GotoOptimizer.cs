using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    class GotoOptimizer: TACOptimizer
    {
        public GotoOptimizer(ThreeAddressCode tac) : base(tac)
        {
        }

        public override void Run()
        {
            for (int i = 1; i < Instructions.Count - 2; ++i)
            {
                if (Instructions[i] != null && Instructions[i].Operation.Equals("if goto"))
                {
                    if (Instructions[i + 1].Operation.Equals("goto"))
                    {
                        Instructions[i].Argument1 = "!(" + Instructions[i].Argument1 + ")";
                        Instructions[i].Argument2 = Instructions[i + 1].Argument1;
                        Instructions[i + 1] = null;
                        Instructions[i + 2] = null;
                    }
                }
            }
            Instructions = Instructions.Where(x => x != null).ToList();
        }
    }
}
