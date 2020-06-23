using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleLang;
using TestSuite.TAC;
using SimpleLang.CFG;
using SimpleLang.TAC;

namespace TestSuite.CFG
{
    [TestFixture]
    internal class IterAlgoGenericTests : TACTestsBase
    {
        [Test]
        public void GenericPorpagation1()
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
        public void GenericPropagation2()
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

        private void AssertSet(
            List<(HashSet<string> IN, HashSet<string> OUT)> expected,
            List<(HashSet<string> IN, HashSet<string> OUT)> actual)
        {
            for (var i = 0; i < expected.Count; ++i)
            {
                Assert.True(expected[i].IN.SetEquals(actual[i].IN));
                Assert.True(expected[i].OUT.SetEquals(actual[i].OUT));
            }
        }

        private void AssertSet(
            List<(List<TACInstruction> IN, List<TACInstruction> OUT)> expected,
            List<(List<TACInstruction> IN, List<TACInstruction> OUT)> actual)
        {
            for (var i = 0; i < expected.Count; ++i)
            {
                for (var j = 0; j < expected[i].IN.Count; j++)
                {
                    Assert.True(expected[i].IN[j].ToString().Equals(actual[i].IN[j].ToString()));
                }

                for (var j = 0; j < expected[i].OUT.Count; j++)
                {
                    Assert.True(expected[i].OUT[j].ToString().Equals(actual[i].OUT[j].ToString()));
                }
            }
        }
    }
}
