using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public class DeadAliveOptimize : TACOptimizer
    {
        public DeadAliveOptimize(List<TACInstruction> instructions) : base(instructions)
        {
        }

        public override void Run()
        {
            var newInscructions = new List<TACInstruction>();
            var assigmentInfo = new Dictionary<string, bool>();

            var last = Instructions.Last();
            newInscructions.Add(last);
            assigmentInfo.Add(last.Result, false);
            if (!int.TryParse(last.Argument1, out _) && last.Argument1 != "True" && last.Argument1 != "False")
                assigmentInfo[last.Argument1] = true;
            if (!int.TryParse(last.Argument2, out _) && last.Argument2 != "True" && last.Argument2 != "False")
                assigmentInfo[last.Argument2] = true;
            for(int i = Instructions.Count - 2; i >= 2; i--)
            {
                var inst = Instructions[i];
                if (inst.Operation == "Empty")
                {
                    newInscructions.Add(inst);
                    continue;
                }
                if (assigmentInfo.ContainsKey(inst.Result) && !assigmentInfo[inst.Result])
                {
                    newInscructions.Add(new TACInstruction("Empty", null, null,  null, null));
                    continue;
                }

                assigmentInfo[inst.Result] = false;
                if (!int.TryParse(inst.Argument1, out _) && inst.Argument1 != "True" && inst.Argument1 != "False")
                    assigmentInfo[inst.Argument1] = true;
                if (!int.TryParse(inst.Argument2, out _) && inst.Argument2 != "True" && inst.Argument2 != "False")
                    assigmentInfo[inst.Argument2] = true;
                newInscructions.Add(inst);
            }
            newInscructions.Reverse();
            Instructions = newInscructions;
        }
    }
}
