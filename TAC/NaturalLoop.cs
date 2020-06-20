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
            Blocks = new List<BasicBlock>();
            Start = start;
            End = end;
        }

        public NaturalLoop(BasicBlock start, BasicBlock end, params BasicBlock[] blocks)
        {
            Blocks = new List<BasicBlock>();
            foreach (var b in blocks)
            {
                AddBlock(b);
            }
        }

        public void AddBlock(BasicBlock block)
        {
            Blocks.Add(block);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var another = obj as NaturalLoop;
            if (another == null) return false;
            var first = Blocks.Select(b => b.ToString().Trim());
            var second = another.Blocks.Select(b => b.ToString().Trim());
            foreach (var str in first)
            {
                if (!second.Contains(str))
                {
                    return false;
                }
            }
            foreach (var str in second)
            {
                if (!first.Contains(str))
                {
                    return false;
                }
            }
            return true;
        }

        public bool EqualsToString(string[] second)
        {
            var first = Blocks.Select(b => b.ToString().Trim());
            foreach (var str in first)
            {
                if (!second.Contains(str))
                {
                    return false;
                }
            }
            foreach (var str in second)
            {
                if (!first.Contains(str))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 54238299 + EqualityComparer<List<BasicBlock>>.Default.GetHashCode(Blocks);
        }
    }
}
