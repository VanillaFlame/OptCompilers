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
    class NumberEqualityTests : SyntaxTreeTestsBase
    {
        [TestCase("a = 5 > 2;", "a = true;")]
        [TestCase("a = 2 > 2;", "a = false;")]
        [TestCase("a = 5 >= 5;", "a = true;")]
        [TestCase("a = 5 < 2;", "a = false;")]
        [TestCase("a = 5 < 5;", "a = false;")]
        [TestCase("a = 5 <= 5;", "a = true;")]
        [TestCase("a = 5 == 2;", "a = false;")]
        [TestCase("a = 5 == 5;", "a = true;")]
        public void OneLineTests(string line, string expected)
        {
            var parser = GenerateTree(@"{" + $"\n{line}\n" + @"}");
            var expectedTree = GenerateTree(@"{" + $"\n{expected}\n" + @"}");
            var a = new NumberEqualityVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as AssignNode).Expr as BoolValNode;
            var second = (expectedTree.root.StList[0] as AssignNode).Expr as BoolValNode;
            Assert.AreEqual(first.Val, second.Val);
        }

        [TestCase("5 == 5", "true")]
        [TestCase("5 < 2", "false")]
        public void WhileTest(string line, string expected)
        {
            var parser = GenerateTree(@"{" + $"\n while({line})\n" + 
@"{
a = 3;
}
}");            
            var expectedTree = GenerateTree(@"{" + $"\n while({expected})\n" +
@"{
a = 3;
}
}");
            var a = new NumberEqualityVisitor();
            a.Visit(parser.root);
            var first = (parser.root.StList[0] as WhileNode).Condition as BoolValNode;
            var second = (expectedTree.root.StList[0] as WhileNode).Condition as BoolValNode;
            Assert.AreEqual(first.Val, second.Val);
        }

        [TestCase("5 == 5", "true")]
        [TestCase("5 < 2", "false")]
        public void ForIfTest(string line, string expected)
        {
            var parser = GenerateTree(
@"{
for i = 1..5
{" 
+ $"\n if({line})\n" +
@"{
a = 3;
}
}
}");
            var expectedTree = GenerateTree(
@"{
for i = 1..5
{"
+ $"\n if({expected})\n" +
@"{
a = 3;
}
}
}");
            var a = new NumberEqualityVisitor();
            a.Visit(parser.root);
            var first = ((parser.root.StList[0] as ForNode)
                .Stat.StList[0] as IfNode)
                .Condition as BoolValNode;
            var second = ((expectedTree.root.StList[0] as ForNode)
                .Stat.StList[0] as IfNode)
                .Condition as BoolValNode;
            Assert.AreEqual(first.Val, second.Val);
        }
    }
}
