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
    x = (x * (a - a)) * 1;
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


        [Test]
        public void WhileFalseOptimization()
        {
            var Text =
@"
{
  x = 13;
  b = 14;
  while (7 - 7 == 3 - 1)
  {
    x = x * (a - a);
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

            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 13",
                "b = 14"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void IfFalseOptimization()
        {
            var Text =
@"
{
  x = 13;
  b = 14;
  if (a != a)
  {
    x = x * (a - a);
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

            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 13",
                "b = 14"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void OptimizationPipeline1()
        {
            var Text =
@"
{
  x = 13;
  b = 14;
  if (2 < 1)
  {
    x = x * (a - a);
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

            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 13",
                "b = 14"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void OptimizationPipeline2()
        {
            var Text =
@"
{
  x = 13;
  b = 14;
  if (x - x + 2 < a - a + 1 + 0)
  {
    x = x * (a - a);
    b = b;
  }
  if (true) 
  {
    a = 1;
  } 
  else 
  {
    a = 1;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

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
                "a = 1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void OptimizationPipeline3()
        {
            var Text =
@"
{
  x = a - ((a - a) + a);
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 0"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void OptimizationPipeline4()
        {
            var Text =
@"
{
  if (a > b) 
  {
    a = a;
  }
  c = 3;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "c = 3"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void OptimizationPipeline5()
        {
            var Text =
@"
{
  c = 3;
  if (2 > 1) 
  {
    a = a;
  }
  else 
  {
    b = b;
    if (3 > 2) 
    {
      a = a;
    }
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            AllVisitorsOptimization.Optimization(parser);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "c = 3"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
