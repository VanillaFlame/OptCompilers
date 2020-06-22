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
"b = 3\nc = b\na = 4"
            };

            Assert.AreEqual(expected, actual);
        }
    }
}
