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

        public bool RawLoopsAreEqual(NaturalLoop first, string[] second)
        {
            return first.EqualsToString(second);
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
        public void OneLoop2()
        {
            var actual = GetLoops(
@"
{
n = 4;
1: c = n + 5;
if a + 3 > 4 * 2
{
a = a + 3;
}
else
{
b = 5;
}
s = 8;
goto 1;
i = 3;
} 
");
            var expected = new string[] {
"1\n" +
"#t0 = n + 5\n" +
"c = #t0\n" +
"#t1 = a + 3\n" +
"#t2 = 4 * 2\n" +
"#t3 = #t1 > #t2\n" +
"if #t3 goto #L0",

"#L1\n" +
"s = 8\n" +
"goto 1",

"b = 5\n" +
"goto #L1",

"#L0\n" +
"#t4 = a + 3\n" +
"a = #t4"
            };
            Assert.IsTrue(actual.Count == 1, "Loops are not equal");
            Assert.IsTrue(RawLoopsAreEqual(actual[0], expected), "Loops are not equal");
        }

        [Test]
        public void ManyLoops()
        {
            var actual = GetLoops(
@"
{
3: q = 1;
if q 
{
goto 3;
}
n = 4;
1: c = n + 5;
if a + 3 > 4 * 2
{
a = a + 3;
}
else
{
b = 5;
}
s = 8;
goto 1;
i = 3;
} 
");

            var loop1 = new string[] {
"#L0\ngoto 3",

"3\nq = 1\nif q goto #L0"
            };

            var loop2 = new string[] {
"1\n" +
"#t0 = n + 5\n" +
"c = #t0\n" +
"#t1 = a + 3\n" +
"#t2 = 4 * 2\n" +
"#t3 = #t1 > #t2\n" +
"if #t3 goto #L2",

"#L3\n" +
"s = 8\n" +
"goto 1",

"b = 5\n" +
"goto #L3",

"#L2\n" +
"#t4 = a + 3\n" +
"a = #t4"
            };

            Assert.IsTrue(actual.Count == 2, "Loops are not equal");
            Assert.IsTrue(RawLoopsAreEqual(actual[0], loop1), "Loops are not equal");
            Assert.IsTrue(RawLoopsAreEqual(actual[1], loop2), "Loops are not equal");
        }
    }
}
