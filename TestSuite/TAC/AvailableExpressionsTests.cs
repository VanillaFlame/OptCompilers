using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleLang.TAC;
using SimpleLang.CFG;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;

namespace TestSuite.TAC
{
    [TestFixture]
    public class AvailableExpressionsTests : TACTestsBase
    {
        [Test]
        public void OneBlock1()
        {
            var TAC = GenerateTAC(
@"
{
x = z + y;
u = x;
t = 2 * 3;
t = z + y;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new AvailableExpressionsOptimizer();
            optimizer.Run(cfg, blocks.blocks);
            var actual = blocks.blocks.Select(b => b.ToString().Trim());
            var expected = new List<string>() {
"#t0 = z + y\n" +
"x = #t0\n" +
"u = x\n" +
"#t1 = 2 * 3\n" +
"t = #t1\n" +
"#t2 = #t0\n" +
"t = #t2"
        };
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void OneBlock2()
        {
            var TAC = GenerateTAC(
@"
{
x = 1 + y;
t = 1 + y;
y = 2;
z = 1 + y;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new AvailableExpressionsOptimizer();
            optimizer.Run(cfg, blocks.blocks);
            var actual = blocks.blocks.Select(b => b.ToString().Trim());
            var expected = new List<string>() {
"#t0 = 1 + y\n" +
"x = #t0\n" +
"#t1 = #t0\n" +
"t = #t1\n" +
"y = 2\n" +
"#t2 = 1 + y\n" +
"z = #t2"
        };
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void ManyBlocks()
        {
            var TAC = GenerateTAC(
@"
{
1: a = b + c;
d = b + c;
b = b + c;
t = b + c;
a = d * e;
if x
{
goto 1;
}
3: o = d * e;
a = d * e;
if y
{
goto 3;
}
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new AvailableExpressionsOptimizer();
            optimizer.Run(cfg, blocks.blocks);
            var actual = blocks.blocks.Select(b => b.ToString().Trim());
            var expected = new List<string>() {
"1\n" +
"#t0 = b + c\n" +
"a = #t0\n" +
"#t1 = #t0\n" +
"d = #t1\n" +
"#t2 = #t0\n" +
"b = #t2\n" +
"#t3 = b + c\n" +
"t = #t3\n" +
"#t4 = d * e\n" +
"a = #t4\n" +
"if x goto #L0",

"goto #L1",

"#L0\n" +
"goto 1",

"#L1",

"3\n" +
"#t5 = #t4\n" +
"o = #t5\n" +
"#t6 = #t4\n" +
"a = #t6\n" +
"if y goto #L2",

"goto #L3",

"#L2\n" +
"goto 3",

"#L3"
        };
            Assert.AreEqual(actual, expected);
        }
    }
}
