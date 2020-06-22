using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public class DeadAliveOptimize : TACOptimizer
    {
        public DeadAliveOptimize(ThreeAddressCode instructions) : base(instructions)
        {
        }

        public override void Run()
        {
            var newInscructions = new List<TACInstruction>();
            var assigmentInfo = new Dictionary<string, bool>();

            var last = Instructions.Last();
            newInscructions.Add(last);
            assigmentInfo.Add(last.Result, false);
            if (!int.TryParse(last.Argument1, out _) && last.Argument1 != "true" && last.Argument1 != "false")
                assigmentInfo[last.Argument1] = true;
            if (!int.TryParse(last.Argument2, out _) && last.Argument2 != "true" && last.Argument2 != "false")
                assigmentInfo[last.Argument2] = true;
            for (int i = Instructions.Count - 2; i >= 0; i--)
            {
                var inst = Instructions[i];
                if (inst.Operation == "Empty")
                {
                    newInscructions.Add(inst);
                    continue;
                }
                if (assigmentInfo.ContainsKey(inst.Result) && !assigmentInfo[inst.Result])
                {
                    newInscructions.Add(new TACInstruction("Empty", null, null, null, inst.Result));
                    continue;
                }

                assigmentInfo[inst.Result] = false;
                if (!int.TryParse(inst.Argument1, out _) && inst.Argument1 != "true" && inst.Argument1 != "false")
                    assigmentInfo[inst.Argument1] = true;
                if (!int.TryParse(inst.Argument2, out _) && inst.Argument2 != "true" && inst.Argument2 != "false")
                    assigmentInfo[inst.Argument2] = true;
                newInscructions.Add(inst);
            }
            newInscructions.Reverse();
            Instructions = newInscructions;
        }
    }
}
