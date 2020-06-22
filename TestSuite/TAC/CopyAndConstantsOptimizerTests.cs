using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.CFG;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;

namespace TestSuite.TAC
{
    [TestFixture]
    class CopyAndConstantsOptimizerTests : TACTestsBase
    {
        [Test]
        public void PresentationCopy()
        {
            var TAC = GenerateTAC(
@"
{
a = b;
c = b - a;
d = c + 1;
e = d * a;
a = x - y;
k = c + a;
}
");
            var expectedTac = GenerateTAC(
@"
{
a = b;
c = b - b;
d = c + 1;
e = d * b;
a = x - y;
k = c + a;
}
");
            var cAndC = new CopyAndConstantsOptimizer(TAC);
            cAndC.Run();
            Assert.AreEqual(cAndC.TAC.ToString(), expectedTac.ToString());
        }

        [Test]
        public void PresentationConstant()
        {
            var TAC = GenerateTAC(
@"
{
a = b;
c = 0;
d = c + 1;
e = d * b;
a = x - y;
k = c + a;
}
");
            var expectedTac = GenerateTAC(
@"
{
a = b;
c = 0;
d = 0 + 1;
e = d * b;
a = x - y;
k = 0 + a;
}
");
            var cAndC = new CopyAndConstantsOptimizer(TAC);
            cAndC.Run();
            Assert.AreEqual(cAndC.TAC.ToString(), expectedTac.ToString());
        }

        [Test]
        public void Test1()
        {
            var TAC = GenerateTAC(
@"
{
a = 1;
b = a;
c = 3;
c = b;
z = a;
}
");
            var expectedTac = GenerateTAC(
@"
{
a = 1;
b = 1;
c = 3;
c = 1;
z = 1;
}
");
            var cAndC = new CopyAndConstantsOptimizer(TAC);
            cAndC.Run();
            Assert.AreEqual(cAndC.TAC.ToString(), expectedTac.ToString());
        }

        [Test]
        public void Test2()
        {
            var TAC = GenerateTAC(
@"
{
a = 1;
b = 2;
c = a + b;
d = 3 + b;
e = 1 / 2;
f = e;
}
");
            var expectedTac = GenerateTAC(
@"
{
a = 1;
b = 2;
c = 1 + 2;
d = 3 + 2;
e = 1 / 2;
f = e;
}
");
            var cAndC = new CopyAndConstantsOptimizer(TAC);
            cAndC.Run();
            Assert.AreEqual(cAndC.TAC.ToString(), expectedTac.ToString());
        }
    }
}
