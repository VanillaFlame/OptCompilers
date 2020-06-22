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

namespace TestSuite.SyntaxTree
{
    class SyntaxTreeTestsBase
    {
        protected Parser GenerateTree(string sourceCode)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            return parser;
        }
    }
}