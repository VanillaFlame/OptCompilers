using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestSuite.TAC.TACGeneration
{
    [TestFixture]
    public class TACGenerationTests : TACTestsBase
    {
        [Test]
        public void SimpleGeneration()
        {
            var TAC = GenerateTAC(
@"
{
int x;
x = 14;
y = 2 * (a + b) - c;
x = x + x;
}
");
            var expected = new List<string>()
            {
                "x = 14",
                "#t0 = a + b",
                "#t1 = 2 * #t0",
                "#t2 = #t1 - c",
                "y = #t2",
                "#t3 = x + x",
                "x = #t3"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
