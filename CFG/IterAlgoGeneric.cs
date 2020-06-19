using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG
{
    public enum directed { forward, back }

    public abstract class IterAlgoGeneric<T> where T : IEnumerable<T>
    {
        public abstract Func<T, T, T> CollectingOperator { get; }
        public abstract Func<T, T, bool> Compare { get; }
        public abstract T Init { get; protected set; }
        public virtual T InitFirst { get => Init; protected set { } }
        public abstract Func<BasicBlock, T, T> TransferFunction { get; protected set; }
        public virtual directed directed => directed.forward;
        public virtual InOutData<T> Execute(ControlFlowGraph graph)
        {
            GetInitData(graph, out var blocks, out var data,
                out var InitBlocks, out var InitVals, out var combine);

            var outChanged = true;
            while (outChanged)
            {
                outChanged = false;
                foreach (var block in blocks)
                {
                    var inset = InitBlocks(block).Aggregate(Init, (x, y) => CollectingOperator(x, InitVals(y)));
                    var outset = TransferFunction(block, inset);
                    if (!Compare(outset, InitVals(block)))
                        outChanged = true;
                    data[block] = combine(inset, outset);
                }
            }
            return data;
        }

        private void GetInitData(ControlFlowGraph graph, out IEnumerable<BasicBlock> blocks, out InOutData<T> data, out Func<BasicBlock, IEnumerable<BasicBlock>> InitBlocks, out Func<BasicBlock, T> InitVals, out Func<T, T, (T, T)> combine)
        {
            var start = directed == directed.back
                ? graph.blocks.Last()
                : graph.blocks.First();
            blocks = graph.blocks.Except(new[] { start });

            var dataTemp = new InOutData<T>
            {
                [start] = (InitFirst, InitFirst)
            };
            foreach (var block in blocks)
            {
                dataTemp[block] = (Init, Init);
            }
            data = dataTemp;

            switch (directed)
            {
                case directed.forward:
                    InitBlocks = x => graph._parents[x.Index].Select(z => z.block);

                    InitVals = x => dataTemp[x].Out;
                    combine = (x, y) => (x, y);
                    break;
                case directed.back:
                    InitBlocks = x => graph._children[x.Index].Select(z => z.block);
                    InitVals = x => dataTemp[x].In;
                    combine = (x, y) => (y, x);
                    break;
                default:
                    throw new NotImplementedException("Neither forward nor back");
            }
        }
    }
    public class InOutData<T> : Dictionary<BasicBlock, (T In, T Out)>
       where T : IEnumerable<T>
    {
        public override string ToString()
        {
            var InOut = new StringBuilder();
            InOut.AppendLine("++++");
            foreach (var kv in this)
                InOut.AppendLine(kv.Key + ":\n" + kv.Value);
            InOut.AppendLine("++++");
            return InOut.ToString();
        }

        public InOutData() { }

        public InOutData(Dictionary<BasicBlock, (T, T)> dictionary)
        {
            foreach (var b in dictionary)
            {
                this[b.Key] = b.Value;
            }
        }
    }
}
