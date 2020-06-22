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
"x = z + y\n" +
"u = x\n" +
"t = 2 * 3\n" +
"t = x"
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
"x = 1 + y\n" +
"t = x\n" +
"y = 2\n" +
"z = 1 + y"
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
"a = b + c\n" +
"d = a\n" +
"b = a\n" +
"t = b + c\n" +
"a = d * e\n" +
"if x goto #L0",

"goto #L1",

"#L0\n" +
"goto 1",

"#L1",

"3\n" +
"o = a\n" +
"a = a\n" +
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
