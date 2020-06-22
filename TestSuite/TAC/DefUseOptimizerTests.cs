using SimpleLang.TACOptimizers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestSuite.TAC
{
    [TestFixture]
    class DefUseOptimizerTests : TACTestsBase
    {
        [Test]
        public void SimpleExample()
        {
            var TAC = GenerateTAC(
@"
{
  a = b;
  a = c;
}
");
            var optimizer = new DefUseOptimizer(TAC);
            optimizer.Run();
            var expected = new List<string>()
            {
                "",
               "a = c"
            };
            var actual = optimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void SimpleExample2()
        {
            var TAC = GenerateTAC(
@"
{
  b = d;
  a = b;
  a = c;
}
");
            var optimizer = new DefUseOptimizer(TAC);
            optimizer.Run();
            var expected = new List<string>()
            {
               "b = d",
                "",
               "a = c"
            };
            var actual = optimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void NothingToDelete()
        {
            var TAC = GenerateTAC(
@"
{
  a = b;
  b = c;
  c = x + y;
}
");
            var optimizer = new DefUseOptimizer(TAC);
            optimizer.Run();
            var expected = new List<string>()
            {
               "a = b",
               "b = c",
               "#t0 = x + y",
               "c = #t0"
            };
            var actual = optimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void DeleteTempVariable()
        {
            var TAC = GenerateTAC(
@"
{
  a = b + d;
  b = c;
  a = b + d;
}
");
            var optimizer = new DefUseOptimizer(TAC);
            optimizer.Run();
            var expected = new List<string>()
            {
               "",
               "",
               "b = c",
               "#t1 = b + d",
               "a = #t1"
            };
            var actual = optimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
