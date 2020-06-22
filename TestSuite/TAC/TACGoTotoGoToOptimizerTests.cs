using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace TestSuite.TAC
{
    [TestFixture]
    public class TACGoTotoGoToOptimizerTests : TACTestsBase
    {
        [Test]
        public void GoTotoGoToSimple()
        {
            var sourceCode = @"
{
a = 1;
b = 5;
if a > b
{
goto 6;
}
6: a = 4;
} 
";
            var TACGen = GenerateTAC(sourceCode);

            var GoTotoGoToOptimizer = new GoTotoGoTo_EmptyOptimizer(TACGen);
            GoTotoGoToOptimizer.Run();
            var TACGenAfter = GenerateTAC(sourceCode);

            var actual = TACGenAfter.Instructions.Select(instruction => instruction.ToString().Trim());

            var expected = new List<string>()
            {
                "a = 1",
                "b = 5",
                "#t0 = a > b",
                "if #t0 goto #L0",
                "goto #L1",
                "#L0",
                "goto 6",
                "#L1",
                "6",
                "a = 4"
            };

            CollectionAssert.AreEqual(expected, actual);

        }

        [Test]
        public void GoTotoGoToBasicTestCase()
        {
            var sourceCode = @"
{
int a;
goto 4;
1: goto 2;
2: goto 3;
3: goto 4;
4: a = 4;
}
"
;
            var TACGen = GenerateTAC(sourceCode);

            var GoTotoGoToOptimizer = new GoTotoGoTo_EmptyOptimizer(TACGen);
            GoTotoGoToOptimizer.Run();
            var TACGenAfter = GenerateTAC(sourceCode);

            var actual = TACGenAfter.Instructions.Select(instruction => instruction.ToString().Trim());

            var expected = new List<string>()
            {
                "goto 4",
                "1",
                "goto 2",
                "2",
                "goto 3",
                "3",
                "goto 4",
                "4",
                "a = 4",
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GoTotoGoToLabelIfAlwaysTrue()
        {
            var sourceCode = @"
{
b = 0;
goto 1;
a = 1;
1: if true
{
goto 2;
}
2: a = 5;
}
"
;
            var TACGen = GenerateTAC(sourceCode);

            var GoTotoGoToOptimizer = new GoTotoGoTo_EmptyOptimizer(TACGen);
            GoTotoGoToOptimizer.Run();
            var TACGenAfter = GenerateTAC(sourceCode);

            var actual = TACGenAfter.Instructions.Select(instruction => instruction.ToString().Trim());

            var expected = new List<string>()
            {
                "b = 0",
                "goto 1",
                "a = 1",
                "1",
                "if True goto #L0",
                "goto #L1",
                "#L0",
                "goto 2",
                "#L1",
                "2",
                "a = 5",
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GoTotoGoToIfElse()
        {
            var sourceCode = @"
{
int a, b;
b = 5;
if a > b
{
goto 6;
} else
{
goto 4;
}
6: a = 4;
4: a = 6;
}
"
;
            var TACGen = GenerateTAC(sourceCode);

            var GoTotoGoToOptimizer = new GoTotoGoTo_EmptyOptimizer(TACGen);
            GoTotoGoToOptimizer.Run();
            var TACGenAfter = GenerateTAC(sourceCode);

            var actual = TACGenAfter.Instructions.Select(instruction => instruction.ToString().Trim());

            var expected = new List<string>()
            {
                "b = 5",
                "#t0 = a > b",
                "if #t0 goto #L0",
                "goto 4",
                "goto #L1",
                "#L0",
                "goto 6",
                "#L1",
                "6",
                "a = 4",
                "4",
                "a = 6",
            };

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}