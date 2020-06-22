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
    class AlwaysFalseTests : SyntaxTreeTestsBase
    {
        [Test]
        public void SelfNotEqualTest()
        {
            var parser = GenerateTree(
@"{
if (a!=a)
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
if (false)
{
a = 5;
}
else
{
b = 1;
}
}");
            var a = new FindFalseVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as AssignNode);
            var second = (expectedTree.root.StList[0] as AssignNode);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void ifFalseTest()
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
            var a = new IfFalseVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as AssignNode);
            var second = (expectedTree.root.StList[0] as AssignNode);
            Assert.AreEqual(first, second);
        }
    }
}
