using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleLang;
using TestSuite.TAC;
using SimpleLang.CFG;
using SimpleLang.TAC;

namespace TestSuite.CFG
{
    [TestFixture]
    internal class IterAlgoGenericTests : TACTestsBase
    {
        [Test]
        public void SampleClassIterAlgoForTransferFunc()
        {
            var TAC = GenerateTAC(@"
{
int a,b,c;
b = 10;
a = b + 1;
if a < c
{
c = b - a;
} else
{
c = b + a;
}
write(c);
}");
            var TACBlocks = new TACBaseBlocks(TAC.Instructions);
            var cfg = new ControlFlowGraph(TACBlocks.blocks);
            var TransferFunc = new SampleClassIterAlgoForTransferFunc();
            var resultTransferFunc = TransferFunc.Execute(cfg);
            var In = new HashSet<string>();
            var Out = new HashSet<string>();
            var actual = new List<(HashSet<string> IN, HashSet<string> OUT)>();
            foreach (var x in cfg.blocks.Select(z => resultTransferFunc[z]))
            {
                foreach (var y in x.In)
                {
                    In.Add(y.ToString());
                }

                foreach (var y in x.Out)
                {
                    Out.Add(y.ToString());
                }
                actual.Add((new HashSet<string>(In), new HashSet<string>(Out)));
                In.Clear(); Out.Clear();
            }

            var expected =
                new List<(HashSet<string> IN, HashSet<string> OUT)>()
                {
                    (new HashSet<string>(){"c"}, new HashSet<string>(){ "c" }),
                    (new HashSet<string>(){"c"}, new HashSet<string>(){"a", "b"}),
                    (new HashSet<string>(){"a", "b"}, new HashSet<string>(){ "c" }),
                    (new HashSet<string>(){"a", "b"}, new HashSet<string>(){"c"}),
                    (new HashSet<string>(){"c"}, new HashSet<string>(){ }),
                    (new HashSet<string>(){ }, new HashSet<string>(){ })
                };

            AssertSet(expected, actual);
        }

        [Test]
        public void ReachingDefinitionIterativeTest()
        {
            var TAC = GenerateTAC(@"
{
int a,b,c;
b = 12;
a = b + 1;
if a < c
{
c = b - a;
} else
{
c = b + a;
}
write(c);
}
");

            var TACBlocks = new TACBaseBlocks(TAC.Instructions);
            var cfg = new ControlFlowGraph(TACBlocks.blocks);
            var TransferFunc = new SampleClassIterAlgoForTransferFunc();
            var resTransferFunc = TransferFunc.Execute(cfg);
            var In = new List<TACInstruction>();
            var Out = new List<TACInstruction>();
            var actual = new List<(List<TACInstruction> IN, List<TACInstruction> OUT)>();
            foreach (var x in resTransferFunc)
            {
                foreach (var y in x.Value.In)
                {
                    In.Add(y);
                }

                foreach (var y in x.Value.Out)
                {
                    Out.Add(y);
                }
                actual.Add((new List<TACInstruction>(In), new List<TACInstruction>(Out)));
                In.Clear(); Out.Clear();
            }

            var expected =
                new List<(List<TACInstruction> IN, List<TACInstruction> OUT)>()
                {
                    (new List<TACInstruction>(){}, new List<TACInstruction>(){}),
                    (new List<TACInstruction>(){}, new List<TACInstruction>(){TAC.Instructions[2], TAC.Instructions[0]}),
                    (new List<TACInstruction>(){TAC.Instructions[2], TAC.Instructions[0]}, new List<TACInstruction>(){ TAC.Instructions[6], TAC.Instructions[2], TAC.Instructions[0] }),
                    (new List<TACInstruction>(){TAC.Instructions[2], TAC.Instructions[0]}, new List<TACInstruction>(){ TAC.Instructions[9], TAC.Instructions[2], TAC.Instructions[0] }),
                    (new List<TACInstruction>(){TAC.Instructions[6], TAC.Instructions[2], TAC.Instructions[0], TAC.Instructions[9]}, new List<TACInstruction>(){TAC.Instructions[6], TAC.Instructions[2], TAC.Instructions[0], TAC.Instructions[9]}),
                    (new List<TACInstruction>(){TAC.Instructions[6], TAC.Instructions[2], TAC.Instructions[0], TAC.Instructions[9]}, new List<TACInstruction>(){ TAC.Instructions[6], TAC.Instructions[2], TAC.Instructions[0], TAC.Instructions[9]})
                };

            AssertSet(expected, actual);
        }

        private void AssertSet(
            List<(HashSet<string> IN, HashSet<string> OUT)> expected,
            List<(HashSet<string> IN, HashSet<string> OUT)> actual)
        {
            for (var i = 0; i < expected.Count; ++i)
            {
                Assert.True(expected[i].IN.SetEquals(actual[i].IN));
                Assert.True(expected[i].OUT.SetEquals(actual[i].OUT));
            }
        }

        private void AssertSet(
            List<(List<TACInstruction> IN, List<TACInstruction> OUT)> expected,
            List<(List<TACInstruction> IN, List<TACInstruction> OUT)> actual)
        {
            for (var i = 0; i < expected.Count; ++i)
            {
                for (var j = 0; j < expected[i].IN.Count; j++)
                {
                    Assert.True(expected[i].IN[j].ToString().Equals(actual[i].IN[j].ToString()));
                }

                for (var j = 0; j < expected[i].OUT.Count; j++)
                {
                    Assert.True(expected[i].OUT[j].ToString().Equals(actual[i].OUT[j].ToString()));
                }
            }
        }
    }
}
