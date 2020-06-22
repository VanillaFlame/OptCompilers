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
namespace TestSuite.SyntaxTree
{
    [TestFixture]
    class AlwaysIfOrElseTests : SyntaxTreeTestsBase
    {
        [Test]
        public void OneLineTest1()
        {
            var parser = GenerateTree(
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
            var expectedTree = GenerateTree(
@"
{
{
a = 5;
};
}");
            var a = new AlwaysIfOrElseVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as AssignNode);
            var second = (expectedTree.root.StList[0] as AssignNode);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void OneLineTest2()
        {
            var parser = GenerateTree(
@"{
if (false)
{
a = 5;
}
else
{
b = 1;
}
}");
            var expectedTree = GenerateTree(
@"
{
{
b = 1;
};
}");
            var a = new AlwaysIfOrElseVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as AssignNode);
            var second = (expectedTree.root.StList[0] as AssignNode);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void ComplexTest()
        {
            var parser = GenerateTree(
@"{
if (false)
{
a = 7;
if (true)
{
f = 5;
}
else
{
g = 8;
}
}
else
{
if (true)
{
c = 1;
}
else
{
d = 11;
}
}
}");
            var expectedTree = GenerateTree(
@"
{
{
c = 1;
};
}");
            var a = new AlwaysIfOrElseVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as AssignNode);
            var second = (expectedTree.root.StList[0] as AssignNode);
            Assert.AreEqual(first, second);
        }
    }
}
