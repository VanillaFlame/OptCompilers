﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleParser;
using SimpleScanner;
using TestSuite.TAC;

namespace TestSuite.SyntaxTree
{
    [TestFixture]
    class MinusSelfTest:TACTestsBase
    {

        [Test]
        public void Test1()
        {
            var Text =
@"{
b = 5;
a = b-b;
}
";
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var Opt = new MinusSelf();
            parser.root.Visit(Opt);

            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);

            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);

            var expected = new List<string>()
            {
                "b = 5",
                "a = 0"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Test2()
        {
            var text =
@"{
            c = 4;
            b = 5;
            a = c-c;
            c = b-b;
            }";
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);
            var Opt = new MinusSelf();
            parser.root.Visit(Opt);
            var prettyPrinter = new PrettyPrinterVisitor();
            parser.root.Visit(prettyPrinter);
            var TAC = GenerateTAC(prettyPrinter.FormattedProgram);
            var expected = new List<string>()
            {
                "c = 4",
                "b = 5",
                "a = 0",
                "c = 0"
            };
            var actual = TAC.Instructions.Select(instruction => instruction.ToString().Trim());
            CollectionAssert.AreEqual(expected, actual);

        }


    }
}
