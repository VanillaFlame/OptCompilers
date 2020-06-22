using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors.ChangeVisitors;
using ProgramTree;

namespace TestSuite.SyntaxTree
{
    [TestFixture]
    class SameAssignmentOptVisitorTests: SyntaxTreeTestsBase
    {
        [Test]
        public void SimpleTest1()
        {
            var Text =
@"
{
a = a;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>();
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleTest2()
        {
            var Text =
@"
{
a = a;
b = b;
c = c;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>();
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleTest3()
        {
            var Text =
@"
{
a = a;
a = b;
b = b;
c = c;
c = j;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "a = b",
                "c = j"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedTest1()
        {
            var Text =
@"
{
if (a < b)
{
    a = a;
    a = b;
}
b = b;
c = c;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "#t0 = a < b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "a = b",
                "#L1"
                
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedTest2()
        {
            var Text =
@"
{
if (a < b)
{
    a = a;
    a = b;
}
b = b;
c = c;
c = j;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "#t0 = a < b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "a = b",
                "#L1",
                "c = j"

            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedTest3()
        {
            var Text =
@"
{
if (a < b)
{
    a = a;
    a = b;
}
else
{
    b = b;
}
b = b;
c = c;
c = j;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "#t0 = a < b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "a = b",
                "#L1",
                "c = j"

            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void NestedTest4()
        {
            var Text =
@"
{
if (a < b)
{
    a = a;
    a = b;
}
else
{
    b = b;
    n = 4;
}
b = b;
c = c;
c = j;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new RemoveEmptyStatementVisitor()
            };


            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }


            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "#t0 = a < b",
                "if #t0 goto #L0",
                "n = 4",
                "goto #L1",
                "#L0",
                "a = b",
                "#L1",
                "c = j"

            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
