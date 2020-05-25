using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TAC
{
    public class ThreeAddressCode
    {
        public List<TACInstruction> Instructions { get; set; }

        public ThreeAddressCode(List<TACInstruction> instructions)
        {
            Instructions = instructions;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach(var instruction in Instructions)
            {
                stringBuilder.Append(instruction.ToString() + '\n');
            }

            return stringBuilder.ToString();
        }
    }
}
