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
    class DeadAliveVariablesTest:TACTestsBase
    {
        [Test]
        public void OneBlock1()
        {
            var TAC = GenerateTAC(
@"
{
  a = d;
  x = a;
  a = e;
  x = b;
  y = x + z;
}
");

            var optimizer = new DeadAliveOptimize(TAC);
            optimizer.Run();
            var actual = optimizer.Instructions;
            var expected = new List<TACInstruction>() {
                new TACInstruction("Empty",null,null,null,"a"),
                new TACInstruction("Empty",null,null,null,"x"),
                new TACInstruction("=","e","","a",""),
                new TACInstruction("=","b","","x",""),
                new TACInstruction("+","x","z","#t0",""),
                new TACInstruction("=","#t0","","y",""),
        };
            Assert.AreEqual(actual, expected);
        }
    }
}
