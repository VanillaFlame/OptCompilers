using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.CFG;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors.ChangeVisitors;
using ProgramTree;
namespace TestSuite.CFG
{
    [TestFixture]
    class EdgeTypeTests : CFGTestsBase
    {
        [Test]
        public void Test1()
        {
            var cfg = GenerateCFG(
@"{
if (true)
{
a = 5;
}
else
{
b = 1;
}
}");
            /*
             *  4 блока
                0 {if True goto #L0}
                1 {b = 1; goto #L1}
                2 {#L0 a = 5;}
                3 {#L1}
            */
            var dict = cfg.classifyEdges();
            Assert.True(false);
        }

        [Test]
        public void Test2()
        {
            var cfg = GenerateCFG(
@"{
goto 1;
a = 5;
1:
while (a == 5)
{
x = 7;
}
if (z > 99)
{
f = 5;
}
else
{
b = 1;
}
a = 5 == 2;
}");
            /*
             *  10 блоков
                0 {goto 1}
                1 {a = 5}
                2 {1}
                3 {#L0 #t0 = a == 5; if #t0 goto #L1}
                4 {goto #L2}
                5 {#L1 x = 7; goto #L0}
                6 {#L2 #t1 = z > 99; if #t1 goto #L3}
                7 {b = 1; goto #L4}
                8 {#L3 f = 5}
                9 {#L4 #t2 = 5 == 2; a = #t2}
            */
            var baseBlocks = cfg.blocks;
            // start -> 0
            Assert.True(haveLink(cfg.start, baseBlocks[0]));
            // 0 -> 2
            Assert.True(haveLink(baseBlocks[0], baseBlocks[2]));
            // 1 -> 2
            Assert.True(haveLink(baseBlocks[1], baseBlocks[2]));
            Assert.IsEmpty(baseBlocks[1].In);
            // 2 -> 3
            Assert.True(haveLink(baseBlocks[2], baseBlocks[3]));
            // 3 -> 4; 3 -> 5
            Assert.True(haveLink(baseBlocks[3], baseBlocks[4]));
            Assert.True(haveLink(baseBlocks[3], baseBlocks[5]));
            // 4 -> 6
            Assert.True(haveLink(baseBlocks[4], baseBlocks[6]));
            // 5 -> 3
            Assert.True(haveLink(baseBlocks[5], baseBlocks[3]));
            // 6 -> 7; 6 -> 8
            Assert.True(haveLink(baseBlocks[6], baseBlocks[7]));
            Assert.True(haveLink(baseBlocks[6], baseBlocks[8]));
            // 7 -> 9
            Assert.True(haveLink(baseBlocks[7], baseBlocks[9]));
            // 8 -> 9
            Assert.True(haveLink(baseBlocks[8], baseBlocks[9]));
            // 9 -> end
            Assert.True(haveLink(baseBlocks[9], cfg.end));
        }

        bool haveLink(BasicBlock a, BasicBlock b)
            => a.Out.Contains(b) && b.In.Contains(a);
    }
}
