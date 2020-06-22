using NUnit.Framework;
using SimpleLang.CFG;
using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.TAC
{
    [TestFixture]
    public class ConstantPropagationTests : TACTestsBase
    {
        [Test]
        public void Simple()
        {
            var TAC = GenerateTAC(
@"
{
a = 3;
b = 3;
c = a + b;
a = d;
e = a;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ConstantPropagationIter();
            optimizer.Cfg = cfg;
            optimizer.Instructions = TAC.Instructions;
            optimizer.Blocks = blocks.blocks;
            optimizer.Run();
            var actual = optimizer.Instructions.Select(i => i.ToString().Trim()).ToList();

            var expected = new List<string>()
            {
                "a = 3",
                "b = 3",
                "#t0 = 3 + 3",
                "c = #t0",
                "a = d",
                "e = a"
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Nested()
        {
            var TAC = GenerateTAC(
@"
{
b = 3;
if a 
{
goto 1;
}
b = 3;
1: r = b;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ConstantPropagationIter();
            optimizer.Cfg = cfg;
            optimizer.Instructions = TAC.Instructions;
            optimizer.Blocks = blocks.blocks;
            optimizer.Run();
            var actual = optimizer.Instructions.Select(i => i.ToString().Trim()).ToList();

            var expected = new List<string>()
            {
                "b = 3",
                "if a goto #L0",
                "goto #L1",
                "#L0",
                "goto 1",
                "#L1",
                "b = 3",
                "1",
                "r = 3"
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CantPropogate()
        {
            var TAC = GenerateTAC(
@"
{
b = 3;
if a 
{
goto 1;
}
b = 2;
1: r = b;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ConstantPropagationIter();
            optimizer.Cfg = cfg;
            optimizer.Instructions = TAC.Instructions;
            optimizer.Blocks = blocks.blocks;
            optimizer.Run();
            var actual = optimizer.Instructions.Select(i => i.ToString().Trim()).ToList();

            var expected = new List<string>()
            {
                "b = 3",
                "if a goto #L0",
                "goto #L1",
                "#L0",
                "goto 1",
                "#L1",
                "b = 2",
                "1",
                "r = b"
            };
            Assert.AreEqual(expected, actual);
        }
    }
}
