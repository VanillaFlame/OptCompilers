using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleParser;
using SimpleScanner;

namespace TestSuite.TAC.TACGeneration
{
    [TestFixture]
    public class TACGenerationTests : TACTestsBase
    {
        [Test]
        public void SimpleGeneration()
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
            var expected = new List<string>()
            {
                "x = 14",
                "#t0 = a + b",
                "#t1 = 2 * #t0",
                "#t2 = #t1 - c",
                "y = #t2",
                "x = x + x"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void IfGeneration()
        {
            var TAC = GenerateTAC(
@"
{
  int x;
  x = 14;
  if x < 15
  {
    x = x + 1;
  }
}
");
            var expected = new List<string>()
            {
                "x = 14",
                "#t0 = x < 15",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "x = x + 1",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void SimpleForGeneration()
        {
            var TAC = GenerateTAC(
@"
{
  for i = 0..a
  {
    x = x + 1;
  }
}
");
            var expected = new List<string>()
            {
                "i = 0",
                "#L0\t#t0 = i > a",
                "if #t0 goto #L1",
                "x = x + 1",
                "i = i + 1",
                "goto #L0",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void ForGeneration()
        {
            var TAC = GenerateTAC(
@"
{
  for i = 0..(a + b * 2 - 3)
  {
    x = x + 1;
  }
}
");
            var expected = new List<string>()
            {
                "i = 0",
                "#t0 = b * 2",
                "#t1 = a + #t0",
                "#t2 = #t1 - 3",
                "#L0\t#t3 = i > #t2",
                "if #t3 goto #L1",
                "x = x + 1",
                "i = i + 1",
                "goto #L0",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void GotoInsideForGeneration()
        {
            var TAC = GenerateTAC(
@"
{
  for i = 0..a
  {
    x = x + 1;
    if x > a
    {
      goto 777;
    }
  }
  777: x = x + 1;
}
");
            var expected = new List<string>()
            {
                "i = 0",
                "#L0\t#t0 = i > a",
                "if #t0 goto #L1",
                "x = x + 1",
                "#t1 = x > a",
                "if #t1 goto #L2",
                "goto #L3",
                "#L2",
                "goto 777",
                "#L3",
                "i = i + 1",
                "goto #L0",
                "#L1",
                "777",
                "x = x + 1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void WhileGeneration()
        {
            var TAC = GenerateTAC(
@"
{
  x = 0;
  while (x < 15)
  {
    x = x + 1;
  }
}
");
            var expected = new List<string>()
            {
                "x = 0",
                "#L0",
                "#t0 = x < 15",
                "if #t0 goto #L1",
                "goto #L2",
                "#L1",
                "x = x + 1",
                "goto #L0",
                "#L2"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void GotoGeneration()
        {
            var TAC = GenerateTAC(
@"
{
  x = 0;
  goto 444;
  x = 3;
  444: x = 7;
  x = 9;
}
");
            var expected = new List<string>()
            {
                "x = 0",
                "goto 444",
                "x = 3",
                "444",
                "x = 7",
                "x = 9"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
