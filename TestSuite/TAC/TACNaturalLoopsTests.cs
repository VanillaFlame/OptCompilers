using NUnit.Framework;
using SimpleLang.CFG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLang.TAC;

namespace TestSuite.TAC
{
    [TestFixture]
    public class TACNaturalLoopsTests : TACTestsBase
    {
        public List<NaturalLoop> GetLoops(string sourceCode)
        {
            var TAC = GenerateTAC(sourceCode);
            var TACBlocks = new TACBaseBlocks(TAC.Instructions);
            TACBlocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(TACBlocks.blocks);
            var dt = new DominatorsTree(cfg);
            dt.GenDominatorsTree();
            var loops = TACUtils.GetNaturalLoops(cfg, dt.dominators);
            return loops;
        }

        public NaturalLoop GetLoopFromBlocksCode(params string[] blocksCode)
        {
            var result = new NaturalLoop(null, null);
            foreach (var code in blocksCode)
            {
                var block = GenerateTAC(code);
                var TACBlock = new TACBaseBlocks(block.Instructions);
                TACBlock.GenBaseBlocks();
                result.AddBlock(TACBlock.blocks[0]);
            }
            return result;
        }

        public bool LoopsAreEqual(List<NaturalLoop> first, List<NaturalLoop> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            foreach (var l in first)
            {
                if (!second.Contains(l))
                {
                    return false;
                }
            }
            foreach (var l in second)
            {
                if (!first.Contains(l))
                {
                    return false;
                }
            }
            return true;
        }

        [Test]
        public void NoLoops()
        {
            var actual = GetLoops(
@"
{
x = 14;
goto 1;
1: y = 2;
goto 2;
2: y = 3;
}
");
            var expected = new List<NaturalLoop>();
            Assert.IsTrue(LoopsAreEqual(actual, expected), "Loops are not equal");
        }

        [Test]
        public void OneLoop()
        {
            var actual = GetLoops(
@"
{
z = 0;
1: x = 0;
y = 1;
goto 2;
2: x = 1;
goto 1;
y = 123;
}
");
            var loop1 = GetLoopFromBlocksCode(
@"
{
1: x = 0;
y = 1;
goto 2;
}
",
@"
{
2: x = 1;
goto 1;
}
");
            var expected = new List<NaturalLoop>()
            {
                loop1
            };

            Assert.IsTrue(LoopsAreEqual(actual, expected), "Loops are not equal");
        }

        [Test]
        public void ManyLoops()
        {
            var actual = GetLoops(
@"
{
z = 0;
1: x = 0;
y = 1;
goto 2;
2: x = 1;
goto 1;
y = 123;
}
");
            var loop1 = GetLoopFromBlocksCode(
@"
{
1: x = 0;
y = 1;
goto 2;
}
",
@"
{
2: x = 1;
goto 1;
}
");
            var expected = new List<NaturalLoop>()
            {
                loop1
            };

            Assert.IsTrue(LoopsAreEqual(actual, expected), "Loops are not equal");
        }
    }
}
