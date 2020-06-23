using NUnit.Framework;
using SimpleLang;
using SimpleLang.TACOptimizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.TAC
{
    [TestFixture]
    public class CommonExpressionsOptimizerTests : TACTestsBase
    {
        [Test]
        public void Simple()
        {
            var TAC = GenerateTAC(
@"
{
a = 3 + 5;
b = 3 + 5;
f = a + b;
e = 3 * a;
r = a + b;
}
"
            );
            var optimizer = new CommonExpressionsOptimizer(TAC);
            optimizer.Run();

            var actual = optimizer.Instructions.Select(i => i.ToString().Trim()).ToList();
            var expected = new List<string>()
            {
                "a = 3 + 5",
                "b = a",
                "f = a + b",
                "e = 3 * a",
                "r = f"
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CantPropagate()
        {
            var TAC = GenerateTAC(
@"
{
a = 3 + 5;
b = 3 + 5;
f = a + b;
a = 3;
r = a + b;
}
"
            );
            var optimizer = new CommonExpressionsOptimizer(TAC);
            optimizer.Run();

            var actual = optimizer.Instructions.Select(i => i.ToString().Trim()).ToList();
            var expected = new List<string>()
            {
                "a = 3 + 5",
                "b = a",
                "f = a + b",
                "a = 3",
                "r = a + b"
            };
            Assert.AreEqual(expected, actual);
        }
    }
}
