using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.CFG;
using TestSuite.TAC;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace TestSuite.CFG
{
    using InOutInfo = InOutData<IEnumerable<TACInstruction>>;
    [TestFixture]
    class TransferFuncForIterativeAlgorithm : CFGTestsBase
    {

        protected (List<BasicBlock> basicBlocks, InOutInfo inOutInfo) GenGraphAndGetInOutInfo(string program)
        {
            var TAC = GenerateTAC(program);
            var TACBlocks = new TACBaseBlocks(TAC.Instructions);
            TACBlocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(TACBlocks.blocks);
            var inOutInfo = new SampleClassIterAlgoForTransferFunc().Execute(cfg);
            return (TACBlocks.blocks, inOutInfo);
        }

        [Test]

        public void TestMultipleIfStatements()
        {
            (var blocks, var inOutInfo) = GenGraphAndGetInOutInfo(@"
{
int a, b;
a = 5;
if a > 0
{
a = 0;
}
else
{
a = 1;
}
b = a;
}
");
            Assert.AreEqual(4, inOutInfo.Count);

            var falseBranch = blocks[1].Instructions.Take(1);
            var trueBranch = blocks[2].Instructions.Take(1);
            var lastBlock = blocks[3].Instructions.Skip(1);
            CollectionAssert.AreEquivalent(falseBranch.Concat(trueBranch), inOutInfo[blocks[3]].In.Take(1).Concat(new List<TACInstruction>() { new TACInstruction("", "", "", "", "#L0") }));
            CollectionAssert.AreEquivalent(falseBranch.Concat(trueBranch).Concat(lastBlock), inOutInfo[blocks[3]].Out.Take(2).Concat(new List<TACInstruction>() { new TACInstruction("", "", "", "", "#L0") }));
        }

        [Test]
        public void BasicTest()
        {
            (var blocks, var inOutInfo) = GenGraphAndGetInOutInfo(@"
{
int one;
one = 1;
}
");
            // only one basic block + entry and exit
            Assert.AreEqual(1, inOutInfo.Count);

            Assert.AreEqual(0, inOutInfo[blocks[0]].In.Count());
            Assert.AreEqual(1, inOutInfo[blocks[0]].Out.Count());
            Assert.AreEqual(blocks[0].Instructions, inOutInfo[blocks[0]].Out);
        }

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
