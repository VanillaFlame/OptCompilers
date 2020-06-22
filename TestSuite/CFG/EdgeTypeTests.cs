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
            Assert.False(dict.Values.Any(type => type != EdgeType.Advancing));
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
            var dict = cfg.classifyEdges();
            Assert.AreEqual(dict[IndexEdge.Create(1, 2)], EdgeType.Cross);
            Assert.AreEqual(dict[IndexEdge.Create(5, 3)], EdgeType.Retreating);
            dict.Remove(IndexEdge.Create(1, 2));
            dict.Remove(IndexEdge.Create(5, 3));
            Assert.False(dict.Values.Any(type => type != EdgeType.Advancing));
        }

        [Test]
        public void Test3()
        {
            var cfg = GenerateCFG(
@"{
a = 5;
1:
b = c;
2:
goto 2;
goto 1;
d = a;
goto 1;
}");
            /*
             *  5 блоков
                0 {a = 5}
                1 {1: b = c}
                2 {2: goto 2}
                3 {goto 1}
                4 {d = a; goto 1}
            */
            var dict = cfg.classifyEdges();
            Assert.AreEqual(dict[IndexEdge.Create(2, 2)], EdgeType.Retreating);
            Assert.AreEqual(dict[IndexEdge.Create(4, 6)], EdgeType.Cross);
            Assert.AreEqual(dict[IndexEdge.Create(3, 1)], EdgeType.Cross);
            dict.Remove(IndexEdge.Create(2, 2));
            dict.Remove(IndexEdge.Create(4, 6));
            dict.Remove(IndexEdge.Create(3, 1));
            Assert.False(dict.Values.Any(type => type != EdgeType.Advancing));
        }
    }
}
