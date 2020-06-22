using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleParser;
using SimpleScanner;
using NUnit.Framework;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleLang.Visitors;
using SimpleLang.TAC;
using ProgramTree;

namespace TestSuite.SyntaxTree
{
    class AlgebraicIdentityProdDivSum : SyntaxTreeTestsBase
    {
        [TestCase("a = exp * 1;", "a = exp;")]
        [TestCase("a = 1 * exp;", "a = exp;")]
        [TestCase("a = b / 1;", "a = b;")]
        [TestCase("a = c + 0;", "a = c;")]
        [TestCase("a = 0 + d;", "a = d;")]
        [TestCase("a = c/c/1*1*1/1;", "a = c/c;")]
        [TestCase("a = 0 + c + 0*1;", "a = c;")]
        public void OneLineTests(string line, string expected)
        {
            var parser = GenerateTree(@"{" + $"\n{line}\n" + @"}");
            var expectedTree = GenerateTree(@"{" + $"\n{expected}\n" + @"}");
            var ProdDiv = new AlgebraicIdentityProdDiv1Visitor();
            var Sum = new AlgebraicIdentitySum0Visitor();
            ProdDiv.Visit(parser.root);
            Sum.Visit(parser.root);
            var actual = parser.ToString();
            var expect = expectedTree.ToString();
            Assert.AreEqual(actual, expect);
        }

        [Test]
        public void AllDividedTestCasesTest()
        {
            var Text = (@"{
a = 1;
b = 1;
c = 0;
c = c/c/1;
if 1*a + 1*b
{
c = a * b * 1;
}
q = a/1 + 5;
q = c + 0;
}");
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var ProdDiv = new AlgebraicIdentityProdDiv1Visitor();
            var Sum = new AlgebraicIdentitySum0Visitor();
            ProdDiv.Visit(parser.root);
            Sum.Visit(parser.root);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);
            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "a = 1",
                "b = 1",
                "c = 0",
                "c = c / c",
                "#t0 = a + b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "c = a * b",
                "#L1",
                "q = a + 5",
                "q = c"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        protected ThreeAddressCode GenerateTAC(string sourceCode)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            return TACGenerator.TAC;
        }
    }
}
