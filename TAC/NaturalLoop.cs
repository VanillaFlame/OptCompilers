using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang.TAC
{
    public class NaturalLoop
    {
        public BasicBlock Start { get; set; }

        public BasicBlock End { get; set; }

        public List<BasicBlock> Blocks { get; set; }

        public NaturalLoop(BasicBlock start, BasicBlock end)
        {
            Start = start;
            End = end;
        }

        public void AddBlock(BasicBlock block)
        {
            Blocks.Add(block);
        }
    }
}
