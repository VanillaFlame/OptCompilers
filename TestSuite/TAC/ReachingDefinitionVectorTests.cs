using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLang;
using SimpleLang.TAC;
using SimpleLang.CFG;
using SimpleLang.TACOptimizers;
using SimpleLang.Visitors;

namespace TestSuite.TAC
{
    [TestFixture]
    class ReachingDefinitionVectorTests : TACTestsBase
    {
        [Test]
        public void OneBlockTest1()
        {
            var TAC = GenerateTAC(
@"
{
x = z + y;
u = x;
t = z + y;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            var vectorOptimizer = new ReachingDefinitionVectorOptimizer(cfg);
            CheckEquality(optimizer, vectorOptimizer);
        }

        [Test]
        public void OneBlockTest2()
        {
            var TAC = GenerateTAC(
@"
{
x = z + y;
u = x;
t = 2 * 3;
t = z + y;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            var vectorOptimizer = new ReachingDefinitionVectorOptimizer(cfg);
            CheckEquality(optimizer, vectorOptimizer);
        }

        [Test]
        public void ManyBlockTest1()
        {
            var TAC = GenerateTAC(
@"
{
  n = 4;
  c = n + 5;
  if a + 3 > 4 * 2
  {
    a = a + 3;
  }
  else
  {
    b = 5;
  }
  s = 8;
}
");
            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            var vectorOptimizer = new ReachingDefinitionVectorOptimizer(cfg);
            CheckEquality(optimizer, vectorOptimizer);
        }

        [Test]
        public void ManyBlockTest2()
        {
            var TAC = GenerateTAC(
@"
{
i = m - 1;
j = n;
a = u1;
2: i = i + 1;
j = j - 1;
if a
    {
        a = u2;
    }
i = u3;
if b 
    {
        goto 2;
    }
}
");

            var blocks = new TACBaseBlocks(TAC.Instructions);
            blocks.GenBaseBlocks();
            var cfg = new ControlFlowGraph(blocks.blocks);
            var optimizer = new ReachingDefinitionOptimizer(cfg);
            var vectorOptimizer = new ReachingDefinitionVectorOptimizer(cfg);
            CheckEquality(optimizer, vectorOptimizer);
        }

        [Test]
        public void Operations()
        {
            var array1 = new InOutVector(new BitArray(new[] {true,true,false,true}));
            var array2 = new InOutVector(new BitArray(new[] { false, false, false, true }));
            var array3 = new InOutVector(new BitArray(new[] { true, false, true, false }));
            var array1copy = array1;
            // -a
            Assert.AreEqual((-array1).Data, new BitArray(new[] { false, false, true, false }));
            // []
            Assert.AreEqual(array1[0], true);
            Assert.AreEqual(array1[2], false);
            // == !=
            Assert.False(array1 == array2);
            Assert.True(array2 != array3);
            Assert.True(array1copy == array1);
            Assert.True(array1copy != array2);
            // + -
            Assert.AreEqual((array1 + array2).Data, new BitArray(new[] { true, true, false, true }));
            Assert.AreEqual((array2 + array3).Data, new BitArray(new[] { true, false, true, true }));
            Assert.AreEqual((array1 - array2).Data, new BitArray(new[] { true, true, false, false }));
            Assert.AreEqual((array3 - array2).Data, new BitArray(new[] { true, false, true, false }));
        }

        void CheckEquality(ReachingDefinitionOptimizer a, ReachingDefinitionVectorOptimizer b)
        {
            a.Run(); b.Run();
            var assignList = new List<TACInstruction>();
            foreach (var instr in b.graph.blocks.SelectMany(block => block.Instructions))
                if (!(instr.Result.Equals("")
                           || instr.Result.Contains("#")))
                    assignList.Add(instr);
            // сравнить ин ауты, получившиеся из векторов и из списков
            foreach (var block in a.graph.blocks)
            {
                var inB = a.IN[block];
                var outB = a.OUT[block];

                var inBTransformed = new InOutVector(assignList.Count);
                foreach (var instr in inB)
                {
                    inBTransformed[assignList.IndexOf(instr)] = true;
                }

                var outBTransformed = new InOutVector(assignList.Count);
                foreach (var instr in outB)
                {
                    outBTransformed[assignList.IndexOf(instr)] = true;
                }

                var inBVector = b.INvector[block];
                var outBVector = b.OUTvector[block];

                Assert.AreEqual(inBTransformed.Data, inBVector.Data);
                Assert.AreEqual(outBTransformed.Data, outBVector.Data);
            }
        }
    }
}
