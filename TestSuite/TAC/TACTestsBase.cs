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

namespace TestSuite.TAC
{
    public class TACTestsBase
    {
        protected ThreeAddressCode GenerateTAC(string sourceCode)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(sourceCode, 0);

            Parser parser = new Parser(scanner);
            parser.Parse();

            var parentFiller = new FillParentsVisitor();
            parser.root.Visit(parentFiller);

            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            return TACGenerator.TAC;
            /*
            var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
            TACBlocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(TACBlocks.blocks);
            */
        }
    }
}
