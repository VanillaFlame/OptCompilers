using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleLang.TAC;
using SimpleLang.CFG;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;


namespace TestSuite.TAC
{

        [TestFixture]
        public class Aktiv_Varieble_Tests : TACTestsBase
        {
            [Test]
            public void usedef()
            {
                var TAC = GenerateTAC(
    @"
{
i = k + 1;
j = l + 1;
k = i;
l = j;
}
");
                var blocks = new TACBaseBlocks(TAC.Instructions);
                blocks.GenBaseBlocks();
                var cfg = new ControlFlowGraph(blocks.blocks);
                ActiveVariableOptimizer optimizer = new ActiveVariableOptimizer(cfg);
               
            }
        
    }
}
