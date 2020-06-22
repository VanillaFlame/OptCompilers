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
    public class AlgebraicIdentitiesTests : TACTestsBase
    {
        [Test]
        public void SimpleExample()
        {
            var TAC = GenerateTAC(
@"
{
  x = 7;
  x = x + 0;
  x = x / x;
  x = x * 0;
  x = x - 0;
}
");
            var AIOptimizer = new AlgebraicIdentitiesOptimizer(TAC);
            AIOptimizer.Run();
            var expected = new List<string>()
            {
                "x = 7",
                "x = x",
                "x = 1",
                "x = 0",
                "x = x"
            };
            var actual = AIOptimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void InsideIf()
        {
            var TAC = GenerateTAC(
@"
{
  if a > b
  {
    x = 0 + x;
    x = x - x;
    x = x * 1;
  }
}
");
            var AIOptimizer = new AlgebraicIdentitiesOptimizer(TAC);
            AIOptimizer.Run();
            var expected = new List<string>()
            {
                "#t0 = a > b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "x = x",
                "x = 0",
                "x = x",
                "#L1"
            };
            var actual = AIOptimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void TrueIfAndAlgebraicIdentities()
        {
            var Text =
@"
{
  if true
  {
    x = x / 1;
    x = 1 * x;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var trueIfOpt = new TrueIfOptVisitor();
            parser.root.Visit(trueIfOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var AIOptimizer = new AlgebraicIdentitiesOptimizer(TAC);
            AIOptimizer.Run();

            var expected = new List<string>()
            {
                "x = x",
                "x = x"
            };

            var actual = AIOptimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
