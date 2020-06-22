using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleLang.TAC;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;

namespace TestSuite.TAC.TACGeneration
{
    [TestFixture]
    class TACBaseBlocksTests : TACTestsBase
    {
        [Test]
        public void SimpleOneBlockTest1()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
y = 2 * (a + b) - c;
x = x + x;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 1);

            var expected = new List<List<string>>()
            {
                new List<string>() {
                    "x = 14",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3"
                }
            };
            var actual = blocks.blocks.Select(block => block.Instructions.Select(instr => instr.ToString().Trim())).ToList();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OneBlockWithLabelTest2()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
2: y = 2 * (a + b) - c;
x = x + x;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 1);

            var expected = new List<List<string>>()
            {
                new List<string>() {
                    "x = 14",
                    "2",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3"
                }
            };
            var actual = blocks.blocks.Select(block => block.Instructions.Select(instr => instr.ToString().Trim())).ToList();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleManyBlockTest1()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
y = 2 * (a + b) - c;
x = x + x;
if x == 2
{
    y = 1;
    b = 1;
}
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 4);

            var expected = new List<List<string>>()
            {
                new List<string>() {
                    "x = 14",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3",
                    "#t4 = x == 2",
                    "if #t4 goto #L0"
                },
                new List<string>() {
                    "goto #L1"
                },
                new List<string>() {
                    "#L0",
                    "y = 1",
                    "b = 1"
                },
                new List<string>() {
                    "#L1"
                }
            };
            var actual = blocks.blocks.Select(block => block.Instructions.Select(instr => instr.ToString().Trim())).ToList();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ManyBlockWithFakeLabelTest2()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
y = 2 * (a + b) - c;
x = x + x;
if x == 2
{
4:    y = 1;
    b = 1;
}
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 4);

            var expected = new List<List<string>>()
            {
                new List<string>() {
                    "x = 14",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3",
                    "#t4 = x == 2",
                    "if #t4 goto #L0"
                },
                new List<string>() {
                    "goto #L1"
                },
                new List<string>() {
                    "#L0",
                    "4",
                    "y = 1",
                    "b = 1"
                },
                new List<string>() {
                    "#L1"
                }
            };
            var actual = blocks.blocks.Select(block => block.Instructions.Select(instr => instr.ToString().Trim())).ToList();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MergingBlocksTest1()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
y = 2 * (a + b) - c;
x = x + x;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 1);

            var expected = new List<string>()
            {
                    "x = 14",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3"
            };
            var actual = blocks.BlockMerging().Select(instructions => instructions.ToString().Trim());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MergingBlocksTest2()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
y = 2 * (a + b) - c;
x = x + x;
if x == 2
{
4:    y = 1;
    b = 1;
}
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 4);

            var expected = new List<string>()
            {
                    "x = 14",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3",
                    "#t4 = x == 2",
                    "if #t4 goto #L0",
                    "goto #L1",
                    "#L0",
                    "4",
                    "y = 1",
                    "b = 1",
                    "#L1"
            };
            var actual = blocks.BlockMerging().Select(instructions => instructions.ToString().Trim());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ManyBlocksLastGotoTest()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
2: y = 2 * (a + b) - c;
x = x + x;
goto 2;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 2);

            var expected = new List<List<string>>()
            {
                    new List<string>()
                    {
                    "x = 14"
                    },
                    new List<string>()
                    {
                    "2",
                    "#t0 = a + b",
                    "#t1 = 2 * #t0",
                    "#t2 = #t1 - c",
                    "y = #t2",
                    "#t3 = x + x",
                    "x = #t3",
                    "goto 2"
                    }
            };
            var actual = blocks.blocks.Select(block => block.Instructions.Select(instr => instr.ToString().Trim())).ToList();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GlobalManyBlocksTest()
        {
            var TAC = GenerateTAC(
@"
{
for i=1..10
{
    for j=1..10
    {
        a = 0;
    }
}
for i=1..10
{
    a = 1;
}
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            Assert.AreEqual(blocks.blocks.Count, 10);

            var expected = new List<List<string>>()
            {
                    new List<string>()
                    {
                        "i = 1"
                    },
                    new List<string>() 
                    {
                        
                        "#L0\t#t0 = i > 10",
                        "if #t0 goto #L1"
                    },
                    new List<string>()
                    {
                        "j = 1"
                    },
                    new List<string>()
                    {
                        "#L2\t#t1 = j > 10",
                        "if #t1 goto #L3"
                    },
                    new List<string>()
                    {
                        "a = 0",
                        "j = j + 1",
                        "goto #L2"
                    },
                    new List<string>()
                    {
                        "#L3",
                        "i = i + 1",
                        "goto #L0"
                    },
                    new List<string>()
                    {
                        "#L1",
                        "i = 1"
                    },
                    new List<string>()
                    {
                        "#L4\t#t2 = i > 10",
                        "if #t2 goto #L5"
                    },
                    new List<string>()
                    {
                        "a = 1",
                        "i = i + 1",
                        "goto #L4"
                    },
                    new List<string>()
                    {
                        "#L5"
                    },
            };
            var actual = blocks.blocks.Select(block => block.Instructions.Select(instr => instr.ToString().Trim())).ToList();
            Assert.AreEqual(expected, actual);

            var expectedMerg = new List<string>()
            {
                    "i = 1",
                    "#L0\t#t0 = i > 10",
                    "if #t0 goto #L1",
                    "j = 1",
                    "#L2\t#t1 = j > 10",
                    "if #t1 goto #L3",
                    "a = 0",
                    "j = j + 1",
                    "goto #L2",
                    "#L3",
                    "i = i + 1",
                    "goto #L0",
                    "#L1",
                    "i = 1",
                    "#L4\t#t2 = i > 10",
                    "if #t2 goto #L5",
                    "a = 1",
                    "i = i + 1",
                    "goto #L4",
                    "#L5"
            };

            var actualMerg = blocks.BlockMerging().Select(instructions => instructions.ToString().Trim());
            Assert.AreEqual(expectedMerg, actualMerg);
        }

    }

}



