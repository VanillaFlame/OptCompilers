using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleParser;
using SimpleScanner;


namespace TestSuite.TAC
{
    [TestFixture]
    class ConstantFoldingOptimizerTests : TACTestsBase
    {
        [Test]
        public void SimpleExample()
        {
            var TAC = GenerateTAC(
@"
{
  x = 7 + 8;
  y = 3 * 6;
  z = 5 / 5;
}
");
            var optimizer = new ConstantFoldingOptimizer(TAC);
            optimizer.Run();
            var expected = new List<string>()
            {
                "x = 15",
                "y = 18",
                "z = 1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleExample2()
        {
            var TAC = GenerateTAC(
@"
{
  x = 7 - 8 * 0 + 3;
}
");
            var optimizer = new ConstantFoldingOptimizer(TAC);
            optimizer.Run();
            var expected = new List<string>()
            {
                "#t0 = 0",
                "#t1 = 7 - #t0",
                "#t2 = #t1 + 3",
                "x = #t2"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SubsequentOptimizations()
        {
            var TAC = GenerateTAC(
@"
{
  y = 6 / 2 - 2 * 1;
  x = y * 4;
}
");
            var optimizer = new ConstantFoldingOptimizer(TAC);
            optimizer.Run();

            var expected = new List<string>()
            {
                "#t0 = 3",
                "#t1 = 2",
                "#t2 = #t0 - #t1",
                "y = #t2",
                "x = y * 4"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
