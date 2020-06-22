using SimpleLang.TAC;
using SimpleLang.Visitors;
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
            var i = 1;
            while (i < Instructions.Count - 2)
            {
                if (Instructions[i] != null && Instructions[i].Operation.Equals("if goto"))
                {
                    if (Instructions[i + 1].Operation.Equals("goto"))
                    {
                        var tempName = TACGenerationVisitor.GenerateTempName();
                        var notInstruction = new TACInstruction("!", Instructions[i].Argument1, "", tempName);
                        Instructions[i].Argument1 = tempName;
                        Instructions[i].Argument2 = Instructions[i + 1].Argument1;
                        Instructions[i + 1] = null;
                        Instructions[i + 2] = null;
                        Instructions.Insert(i, notInstruction);
                        ++i;
                    }
                }
                ++i;
            }
            Instructions = Instructions.Where(x => x != null).ToList();
        }
    }
}
