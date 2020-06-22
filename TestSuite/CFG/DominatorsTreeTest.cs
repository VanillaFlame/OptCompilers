using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors.ChangeVisitors;
using ProgramTree;
using SimpleLang.CFG;


namespace TestSuite.CFG
{
    [TestFixture]
    class DominatorsTreeTest:CFGTestsBase
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
            var dominatorsTree = new DominatorsTree(cfg);
            dominatorsTree.GenDominatorsTree();
            var Actual = dominatorsTree.dominators;

            var Expect = new Dictionary<BasicBlock, List<BasicBlock>>();
            Expect.Add(cfg.blocks[0],new List<BasicBlock>(){cfg.blocks[0] });
            Expect.Add(cfg.blocks[1], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[1] });
            Expect.Add(cfg.blocks[2], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[2] });
            Expect.Add(cfg.blocks[3], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[3] });
            CollectionAssert.AreEqual(Actual, Expect);
        }

        [Test]
        public void Test2()
        {
            var cfg = GenerateCFG(
@"{
n = 4;
c = n + 5;
if a + 3 > 4 * 2
{
a = a + 3;
}
else
{
b = 5;
}
s = 8;
}");
            /*
             *  4 блока
                0 {n = 4
                   #t0 = n + 5
                   c = #t0
                   #t1 = a + 3
                   #t2 = 4 * 2
                   #t3 = #t1 > #t2
                   if #t3 goto #L0}
                1 { b = 5
                    goto #L1}
                2 { #t4 = a + 3
                    a = #t4}
                3 {s = 8}
            */
            var dominatorsTree = new DominatorsTree(cfg);
            dominatorsTree.GenDominatorsTree();
            var Actual = dominatorsTree.dominators;

            var Expect = new Dictionary<BasicBlock, List<BasicBlock>>();
            Expect.Add(cfg.blocks[0], new List<BasicBlock>() { cfg.blocks[0] });
            Expect.Add(cfg.blocks[1], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[1] });
            Expect.Add(cfg.blocks[2], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[2] });
            Expect.Add(cfg.blocks[3], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[3] });
            CollectionAssert.AreEqual(Actual, Expect);
        }

        [Test]
        public void Test3()
        {
            var cfg = GenerateCFG(
@"{
x = 14;
goto 1;
1: y = 2;
goto 2;
2: y = 3;
}");
            /*
             *  3 блока
                0 {x = 14
                    goto 1}
                1 { y = 2
                    goto 2}
                2 { y = 3}
            */
            var dominatorsTree = new DominatorsTree(cfg);
            dominatorsTree.GenDominatorsTree();
            var Actual = dominatorsTree.dominators;

            var Expect = new Dictionary<BasicBlock, List<BasicBlock>>();
            Expect.Add(cfg.blocks[0], new List<BasicBlock>() { cfg.blocks[0] });
            Expect.Add(cfg.blocks[1], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[1] });
            Expect.Add(cfg.blocks[2], new List<BasicBlock>() { cfg.blocks[0], cfg.blocks[1], cfg.blocks[2] });
            CollectionAssert.AreEqual(Actual, Expect);
        }

    }
}

