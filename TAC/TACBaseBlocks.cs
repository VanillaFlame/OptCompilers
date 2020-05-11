using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TAC
{
    public class TACBaseBlocks
    {

        public  List<List<TACInstruction>> blocks { get; private set; } //&?
        private List<TACInstruction> instructions;

        public TACBaseBlocks(List<TACInstruction> instructions)
        {
            this.instructions = instructions;
            blocks = new List<List<TACInstruction>>();
        }

        public void GenBaseBlocks()
        {
            var block = new List<TACInstruction>();
            block.Add(instructions[0]);

            for (int i = 1; i < instructions.Count; ++i)
            {
                if (instructions[i].HasLabel || instructions[i - 1].Operation.Contains("goto"))
                {
                    blocks.Add(block);
                    block = new List<TACInstruction>();
                }
                block.Add(instructions[i]);
            }
            blocks.Add(block);
        }

        public List<TACInstruction> BlockMerging()
        {
            var merging = new List<TACInstruction>();

            foreach (var block in blocks)
            {
                foreach (var instr in block)
                {
                    merging.Add(instr);
                }
            }
            return merging;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var block in blocks)
            {
                
                foreach (var instr in block)
                {
                    stringBuilder.Append(instr.ToString() +'\n');
                }
                stringBuilder.Append("_______________\n");
            }
            return stringBuilder.ToString();
        }

    }
}
