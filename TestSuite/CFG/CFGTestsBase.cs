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
    class CFGTestsBase
    {
        protected ControlFlowGraph GenerateCFG(string sourceCode)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);

            var blocks = new TACBaseBlocks(TACGenerator.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            return cfg;
        }
    }
}
