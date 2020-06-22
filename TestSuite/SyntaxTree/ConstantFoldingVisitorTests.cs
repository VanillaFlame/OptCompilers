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
    public class ConstantFoldingVisitorTests : TACTestsBase
    {
        [Test]
        public void SimpleExample()
        {
            var Text = (
@"
{
  x = 7 + 8;
  y = 3 * 6;
  z = 5 / 3;
}
");
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var optimizer = new ConstantFoldingVisitor();
            parser.root.Visit(optimizer);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

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
            var Text = (
@"
{
  x = 7 - 8 * 0 + 3;
}
");
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var optimizer = new ConstantFoldingVisitor();
            parser.root.Visit(optimizer);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 10"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleExample3()
        {
            var Text = (
@"
{
  x = 7 - 8 * 0 + 3;
  y = 5 / 2 - 2 * 1;
  x = y * 4;
}
");
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var optimizer = new ConstantFoldingVisitor();
            parser.root.Visit(optimizer);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TAC = TACGenerator.TAC;

            var expected = new List<string>()
            {
                "x = 10",
                "y = 0", 
                "x = y * 4"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
