using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleLang.TAC;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleParser;
using SimpleScanner;

namespace TestSuite.TAC
{

        [TestFixture]
        public class CommonExprTest : TACTestsBase
        {
            [Test]
            public void SimpleExample()
            {
                var TAC = GenerateTAC(
    @"
{
a = b + c;
b = a - d;
c = b + c;
d = a + d;
}
");
            CommonExpressionsOptimizer CommonExpOptimizer = new  CommonExpressionsOptimizer(TAC);
            CommonExpOptimizer.Run();
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
                var actual = CommonExpOptimizer.TAC.Instructions.Select(instruction => instruction.ToString().Trim());
                CollectionAssert.AreEqual(expected, actual);
            }

        private CommonExpressionsOptimizer CommonExpressionsOptimizer(ThreeAddressCode tAC)
        {
            throw new NotImplementedException();
        }
    }
    }
