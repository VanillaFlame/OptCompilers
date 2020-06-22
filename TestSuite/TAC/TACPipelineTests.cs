using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLang.TACOptimizers;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace TestSuite.TAC
{
    [TestFixture]
    public class TACPipelineTests : TACTestsBase
    {
        [Test]
        public void Simple()
        {
            var sourceCode =
@"
{
a = 3;
a = 5;
b = 3;
c = (b + 1) * 2;
a = 4;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);
            var blocks = AllTacOptimization.Optimize(parser);
            var actual = blocks.blocks.Select(b => b.ToString().Trim()).ToList();

            var expected = new List<string>()
            {
"b = 3\n" +
"c = 8\n" +
"a = 4"
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Simple2()
        {
            var sourceCode =
@"
{
a = b + c;
d = 3 * 2 + 1;
e = (b + c) * d;
c = (b + c) * 2;
goto 1;
b = 3;
1: r = b + c;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);
            var blocks = AllTacOptimization.Optimize(parser);
            var actual = blocks.blocks.Select(b => b.ToString().Trim()).ToList();

            var expected = new List<string>()
            {
"#t0 = b + c\n" +
"a = #t0\n" +
"d = 7\n" +
"#t3 = #t0\n" +
"#t4 = #t3 * 7\n" +
"e = #t4\n" +
"#t5 = #t0\n" +
"#t6 = #t5 * 2\n" +
"c = #t6\n" +
"goto 1",

"b = 3",

"1\n" +
"#t7 = b + c\n" +
"r = #t7"
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WithConstantFoldingIter()
        {
            var sourceCode =
@"
{
b = 3;
if a 
{
goto 1;
}
b = 3;
1: r = b;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);
            var blocks = AllTacOptimization.Optimize(parser);
            var actual = blocks.blocks.Select(b => b.ToString().Trim()).ToList();

            var expected = new List<string>()
            {
"b = 3\n" +
"#t0 = !a\n" +
"#t1 = !#t0\n" +
"if #t1 goto 1",

"b = 3",

"1\n" +
"r = 3"
            };

            Assert.AreEqual(expected, actual);
        }
    }
}
