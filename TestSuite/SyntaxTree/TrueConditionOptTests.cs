using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleParser;
using SimpleScanner;
using TestSuite.TAC;

namespace TestSuite.SyntaxTree
{
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
}
