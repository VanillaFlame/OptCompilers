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
                "#t3 = x + x",
                "x = #t3"
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
                "#t1 = x + 1",
                "x = #t1",
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
                "#t1 = x + 1",
                "x = #t1",
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
                "#t4 = x + 1",
                "x = #t4",
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
                "#t1 = x + 1",
                "x = #t1",
                "#t2 = x > a",
                "if #t2 goto #L2",
                "goto #L3",
                "#L2",
                "goto 777",
                "#L3",
                "i = i + 1",
                "goto #L0",
                "#L1",
                "777",
                "#t3 = x + 1",
                "x = #t3"
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
                "#t1 = x + 1",
                "x = #t1",
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

    [TestFixture]
    public class TrueConditionOptTests : TACTestsBase
    {
        [Test]
        public void SimpleIf()
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

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "if True goto #L0",
                "goto #L1",
                "#L0",
                "b = a",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void SimpleWhile()
        {
            var Text =
@"
{
  while (a == a)
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

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "#L0",
                "if True goto #L1",
                "goto #L2",
                "#L1",
                "b = a",
                "goto #L0",
                "#L2"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void SimpleFor()
        {
            var Text =
@"
{
  for i = a..a
  {
    if b >= b
    {
      b = a;
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

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "i = a",
                "#L0\t#t0 = i > a",
                "if #t0 goto #L1",
                "if True goto #L2",
                "goto #L3",
                "#L2",
                "b = a",
                "#L3",
                "i = i + 1",
                "goto #L0",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void NestedIf()
        {
            var Text =
@"
{
  if a == a
  {
    if b >= b
    {
      c = a;
    }
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

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "if True goto #L0",
                "goto #L1",
                "#L0",
                "if True goto #L2",
                "goto #L3",
                "#L2",
                "c = a",
                "#L3",
                "b = a",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void NestedIfElse()
        {
            var Text =
@"
{
  if a == a
  {
    if b >= b
    {
      c = a;
    }
    else
    {
      b = a;
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

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "if True goto #L0",
                "goto #L1",
                "#L0",
                "if True goto #L2",
                "b = a",
                "goto #L3",
                "#L2",
                "c = a",
                "#L3",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void SimpleIfElse()
        {
            var Text =
@"
{
  if a == a
  {
    b = a;
  }
  else
  {
    c = a;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "if True goto #L0",
                "c = a",
                "goto #L1",
                "#L0",
                "b = a",
                "#L1"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void IfAterIf()
        {
            var Text =
@"
{
  if a == a
  {
    b = a;
  }

  if b == b
  {
    c = b;
  }
  else
  {
    c = a;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "if True goto #L0",
                "goto #L1",
                "#L0",
                "b = a",
                "#L1",
                "if True goto #L2",
                "c = a",
                "goto #L3",
                "#L2",
                "c = b",
                "#L3"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }


    [TestFixture]
    public class TrueIfOptTests : TACTestsBase
    {
        [Test]
        public void SimpleIfElse()
        {
            var Text =
@"
{
  if true
  {
    b = a;
  }
  else
  {
    c = a;
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

            var expected = new List<string>()
            {
                "b = a"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void SimpleIf()
        {
            var Text =
@"
{
  if true
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

            var trueIfOpt = new TrueIfOptVisitor();
            parser.root.Visit(trueIfOpt);

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
        public void NestedIf()
        {
            var Text =
@"
{
  if true
  {
    if true
    {
      b = a;
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

            var trueIfOpt = new TrueIfOptVisitor();
            parser.root.Visit(trueIfOpt);

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
        public void IfInsideWhile()
        {
            var Text =
@"
{
  while (true)
  {
    if true
    {
      b = a;
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

            var trueIfOpt = new TrueIfOptVisitor();
            parser.root.Visit(trueIfOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "#L0",
                "if True goto #L1",
                "goto #L2",
                "#L1",
                "b = a",
                "goto #L0",
                "#L2"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void TrueConditionOptAndTrueIfOpt()
        {
            var Text =
@"
{
  if a == a
  {
    b = a;
    c = a;
  }
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var trueOpt = new TrueConditionOptVisitor();
            parser.root.Visit(trueOpt);

            var trueIfOpt = new TrueIfOptVisitor();
            parser.root.Visit(trueIfOpt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "b = a",
                "c = a"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }


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
                "#t0 = x",
                "x = #t0",
                "#t1 = 1",
                "x = #t1",
                "#t2 = 0",
                "x = #t2",
                "#t3 = x",
                "x = #t3"
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
                "#t1 = x",
                "x = #t1",
                "#t2 = 0",
                "x = #t2",
                "#t3 = x",
                "x = #t3",
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
                "#t0 = x",
                "x = #t0",
                "#t1 = x",
                "x = #t1"
            };

            var actual = AIOptimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
