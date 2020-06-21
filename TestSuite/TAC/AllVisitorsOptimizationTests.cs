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

namespace TestSuite.TAC
{
    [TestFixture]
    class AllVisitorsOptimizationTests : TACTestsBase
    {
        [Test]
        public void SimpleOptimization()
        {
            var Text =
@"
{
  b = a;
  c = a;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            //var trueOpt = new TrueConditionOptVisitor();
            //parser.root.Visit(trueOpt);
            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "b = a",
                "c = a"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void IfOptimization()
        {
            var Text =
@"
{
  if a == a
  {
    b = a;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            //var trueOpt = new TrueConditionOptVisitor();
            //parser.root.Visit(trueOpt);
            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "b = a"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void ExprOptimization()
        {
            var Text =
@"
{
  x = 13;
  b = 14;
  while (x < b)
  {
    x = x * ( a - a);
    b = b;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            //var trueOpt = new TrueConditionOptVisitor();
            //parser.root.Visit(trueOpt);
            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 13",
                "b = 14",
                "#L0",
                "#t0 = x < b",
                "if #t0 goto #L1",
                "goto #L2",
                "#L1",
                "x = 0",
                "goto #L0",
                "#L2"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
