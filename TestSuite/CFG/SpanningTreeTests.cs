using NUnit.Framework;
using SimpleLang;
using SimpleLang.CFG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.CFG
{
    [TestFixture]
    public class SpanningTreeTests : CFGTestsBase
    {
        [Test]
        public void Simple()
        {
            BasicBlock.clearIndexCounter();
            var cfg = GenerateCFG(
@"
{
a = 3;
b = 2;
c = a + b;
}
");
            var spanningTree = new SpanningTree(cfg);
            var actual = spanningTree.dfst;
            var expected = new List<IndexEdge>()
            {
                new IndexEdge(1, 0),
                new IndexEdge(0, 2)
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IfCase()
        {
            BasicBlock.clearIndexCounter();
            var cfg = GenerateCFG(
@"
{
a = 3;
b = 2;
c = a + b;
if t
{
a = a;
}
else 
{
b = b;
}
}
");
            var spanningTree = new SpanningTree(cfg);
            var actual = spanningTree.dfst;
            var expected = new List<IndexEdge>()
            {
                new IndexEdge(4, 0),
                new IndexEdge(0, 2),
                new IndexEdge(2, 3),
                new IndexEdge(3, 5),
                new IndexEdge(0, 1)
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Complex()
        {
            BasicBlock.clearIndexCounter();
            var cfg = GenerateCFG(
@"
{
a = 3;
if t
{
a = a;
while (true)
{
d = c;
}
}
else 
{
b = b;
}
}
");
            var spanningTree = new SpanningTree(cfg);
            var actual = spanningTree.dfst;
            var expected = new List<IndexEdge>()
            {
                new IndexEdge(8, 0),
                new IndexEdge(0, 2),
                new IndexEdge(2, 3),
                new IndexEdge(3, 5),
                new IndexEdge(3, 4),
                new IndexEdge(4, 6),
                new IndexEdge(6, 7),
                new IndexEdge(7, 9),
                new IndexEdge(0, 1)
            };
            Assert.AreEqual(expected, actual);
        }
    }
}
