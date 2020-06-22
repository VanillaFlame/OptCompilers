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
    class NullIfElseOptTests: SyntaxTreeTestsBase
    {

        [Test]
        public void SimpleTest1()
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

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
                "c = 3"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleTest2()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    a = a;
  }
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
        public void NestedTest1()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    if ( b > a)
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

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
        public void NestedTest2()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    if ( b > a)
    {
        a = a;
    }
  }
  a = b;
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
                "a = b"
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
  if (a > b) 
  {
    if ( b > a)
    {
        a = a;
    }
    a = b;
  }
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
                "#t0 = a > b",
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
        public void SimpleElseTest1()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    a = a;
  }
  else
  {
    a = a;
  }
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
        public void SimpleElseTest2()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    a = b;
  }
  else
  {
    a = a;
  }
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
                "#t0 = a > b",
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
        public void NestedElseTest1()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    if (b < a)
    {
        a = a;
    }
    else
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

            var ChangeVisitorsOptimization = new List<ChangeVisitor>
            {
                new SameAssignmentOptVisitor(),
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
        public void NestedElseTest2()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    if (b < a)
    {
        a = a;
    }
    else
    {
        a = a;
    }
    a = b;
  }
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
                "#t0 = a > b",
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
        public void NestedElseTest3()
        {
            var Text =
 @"
{
  if (a > b) 
  {
    if (b < a)
    {
        a = b;
    }
    else
    {
        a = a;
    }
    a = b;
  }
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
                new NullIfElseOptVisitor(),
                new RemoveEmptyStatementVisitor(),
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
                "#t0 = a > b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "#t1 = b < a",
                "if #t1 goto #L2",
                "goto #L3",
                "#L2",
                "a = b",
                "#L3",
                "a = b",
                "#L1"
                };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
