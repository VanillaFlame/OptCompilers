using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TAC
{
    public class TACBaseBlocks
    {

        //public  List<List<TACInstruction>> blocks { get; private set; } //&?
        public List<BasicBlock> blocks { get;  set; }
        private List<TACInstruction> instructions;

        public TACBaseBlocks(List<TACInstruction> instructions)
        {
            this.instructions = instructions;
            blocks = new List<BasicBlock>();
        }

        public TACBaseBlocks(List<BasicBlock> blocks)
        {
            this.blocks = blocks;
            instructions = new List<TACInstruction>();
            foreach (var block in blocks)
            {
                foreach (var instr in block.Instructions)
                {
                    instructions.Add(instr);
                }
            }
        }

        public void GenBaseBlocks()
        {
            var list = new List<int>();
            list.Add(0);

            for (int i = 1; i < instructions.Count; ++i)
            {
                if (instructions[i - 1].Operation.Contains("goto"))
                {
                    var label = "";
                    if (instructions[i - 1].Operation.Equals("goto"))
                        label = instructions[i - 1].Argument1;
                    else if (instructions[i - 1].Operation.Equals("if goto"))
                        label = instructions[i - 1].Argument2;

                    for (int j = 1; j < instructions.Count; ++j)
                    {
                        if (instructions[j].HasLabel && instructions[j].Label.Equals(label))
                            list.Add(j);
                    }
                    list.Add(i);
                }
            }

            if (instructions[instructions.Count - 1].Operation.Equals("goto"))
                for (int j = 1; j < instructions.Count; ++j)
                {
                    if (instructions[j].HasLabel && instructions[j].Label.Equals(instructions[instructions.Count - 1].Argument1))
                        list.Add(j);
                }


            var result = list.Distinct().ToList();
            result.Sort();


            for (int i = 0; i < result.Count - 1; ++i)
            {
                var block = new List<TACInstruction>();
                for (int j = result[i]; j < result[i + 1]; ++j)
                {
                    block.Add(instructions[j]);
                }
                var baseBlock = new BasicBlock(block);
                blocks.Add(baseBlock);
            }

            var last = new List<TACInstruction>();
            for (int i = result[result.Count - 1]; i < instructions.Count; ++i)
            {
                last.Add(instructions[i]);
            }
            var lastBaseBlock = new BasicBlock(last);
            blocks.Add(lastBaseBlock);
        }

        public List<TACInstruction> BlockMerging()
        {
            var merging = new List<TACInstruction>();

            foreach (var block in blocks)
            {
                foreach (var instr in block.Instructions)
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

                foreach (var instr in block.Instructions)
                {
                    stringBuilder.Append(instr.ToString() + '\n');
                }
                stringBuilder.Append("______________________________________________________\n");
            }
            return stringBuilder.ToString();
        }

    }
}
