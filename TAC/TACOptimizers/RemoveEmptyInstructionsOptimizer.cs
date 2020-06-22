using SimpleLang.TACOptimizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang.TAC.TACOptimizers
{
    public class RemoveEmptyInstructionsOptimizer : TACOptimizer
    {
        public RemoveEmptyInstructionsOptimizer(ThreeAddressCode tac) : base(tac)
        {
        }

        public override void Run()
        {
            var newInstructions = new List<TACInstruction>();
            foreach(var i in Instructions)
            {
                if ((i.Operation != "" || i.HasLabel) && i.Operation != "Empty")
                {
                    newInstructions.Add(i);
                }
            }
            Instructions = newInstructions;
        }
    }
}
