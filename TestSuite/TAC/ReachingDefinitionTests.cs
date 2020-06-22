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
    class ReachingDefinitionTests: TACTestsBase
    {
        [Test]
        public void OneBlockTest1()
        {
            var TAC = GenerateTAC(
@"
{
x = z + y;
u = x;
t = z + y;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            optimizer.Run();
            Assert.AreEqual(optimizer.IN[cfg.start].Count, 0);
            Assert.AreEqual(optimizer.IN[cfg.blocks[0]].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.start].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[0]].Count, 3);

            //{ "t = #t1", "u = x", "x = #t0"};
            var expected = new List<string>() {
"t = z + y",
"u = x",
"x = z + y"
            };
            var actual = optimizer.OUT[cfg.blocks[0]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void OneBlockTest2()
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
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            optimizer.Run();
            Assert.AreEqual(optimizer.IN[cfg.start].Count, 0);
            Assert.AreEqual(optimizer.IN[cfg.blocks[0]].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.start].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[0]].Count, 3);

            //{ "t = #t2", "u = x", "x = #t0"};
            var expected = new List<string>() {
"t = z + y",
"u = x",
"x = z + y"
            };
            var actual = optimizer.OUT[cfg.blocks[0]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void ManyBlockTest1()
        {
            var TAC = GenerateTAC(
@"
{
  n = 4;
  c = n + 5;
  if a + 3 > 4 * 2
  {
    a = a + 3;
  }
  else
  {
    b = 5;
  }
  s = 8;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            optimizer.Run();
            Assert.AreEqual(optimizer.IN[cfg.start].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.start].Count, 0);
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[0]].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[0]].Count, 2);
            var expected = new List<string>() {
"c = n + 5",
"n = 4"
            };
            var actual = optimizer.OUT[cfg.blocks[0]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[1]].Count, 2);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[1]].Count, 3);

            expected = new List<string>() {
"c = n + 5",
"n = 4"
            };
            actual = optimizer.IN[cfg.blocks[1]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"b = 5",
"c = n + 5",
"n = 4"
            };
            actual = optimizer.OUT[cfg.blocks[1]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[2]].Count, 2);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[2]].Count, 3);

            expected = new List<string>() {
"c = n + 5",
"n = 4"
            };
            actual = optimizer.IN[cfg.blocks[2]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"a = a + 3",
"c = n + 5",
"n = 4"
            };
            actual = optimizer.OUT[cfg.blocks[2]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[3]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[3]].Count, 5);

            expected = new List<string>() {
"b = 5",
"c = n + 5",
"n = 4",
"a = a + 3"
            };
            actual = optimizer.IN[cfg.blocks[3]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"s = 8",
"b = 5",
"c = n + 5",
"n = 4",
"a = a + 3"
            };
            actual = optimizer.OUT[cfg.blocks[3]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

        }

        [Test]
        public void ManyBlockTest2()
        {
            var TAC = GenerateTAC(
@"
{
i = m - 1;
j = n;
a = u1;
2: i = i + 1;
j = j - 1;
if a
    {
        a = u2;
    }
i = u3;
if b 
    {
        goto 2;
    }
}
");

            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            optimizer.Run();
            Assert.AreEqual(optimizer.IN[cfg.start].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.start].Count, 0);
            
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[0]].Count, 0);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[0]].Count, 3);
            var expected = new List<string>() {
"a = u1",
"j = n",
"i = m - 1"
            };
            var actual = optimizer.OUT[cfg.blocks[0]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
            
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[1]].Count, 6);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[1]].Count, 4);

            expected = new List<string>() {
"a = u1",
"j = n",
"i = m - 1",
"i = u3",
"j = j - 1",
"a = u2"
            };
            actual = optimizer.IN[cfg.blocks[1]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"j = j - 1",
"i = i + 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.OUT[cfg.blocks[1]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
            
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[2]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[2]].Count, 4);

            expected = new List<string>() {
"j = j - 1",
"i = i + 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.IN[cfg.blocks[2]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"j = j - 1",
"i = i + 1",
"a = u1",
"a = u2", 
            };
            actual = optimizer.OUT[cfg.blocks[2]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);
            
            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[3]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[3]].Count, 3);

            expected = new List<string>() {
"j = j - 1",
"i = i + 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.IN[cfg.blocks[3]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"a = u2",
"j = j - 1",
"i = i + 1",

            };
            actual = optimizer.OUT[cfg.blocks[3]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[4]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[4]].Count, 4);

            expected = new List<string>() {
"j = j - 1",
"i = i + 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.IN[cfg.blocks[4]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2"
            };
            actual = optimizer.OUT[cfg.blocks[4]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[5]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[5]].Count, 4);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.IN[cfg.blocks[5]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2"
            };
            actual = optimizer.OUT[cfg.blocks[5]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[6]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[6]].Count, 4);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.IN[cfg.blocks[6]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2"
            };
            actual = optimizer.OUT[cfg.blocks[6]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            //
            Assert.AreEqual(optimizer.IN[cfg.blocks[7]].Count, 4);
            Assert.AreEqual(optimizer.OUT[cfg.blocks[7]].Count, 4);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2",
            };
            actual = optimizer.IN[cfg.blocks[7]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

            expected = new List<string>() {
"i = u3",
"j = j - 1",
"a = u1",
"a = u2"
            };
            actual = optimizer.OUT[cfg.blocks[7]].Select(x => x.ToString().Trim()).ToList();
            Assert.AreEqual(actual, expected);

        }

    }
}
