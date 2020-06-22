using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang.CFG
{
    public class SampleClassIterAlgoForTransferFunc : IterAlgoGeneric<IEnumerable<TACInstruction>>
    {
        /// <inheritdoc/>
        public override Func<IEnumerable<TACInstruction>, IEnumerable<TACInstruction>, IEnumerable<TACInstruction>> CollectingOperator
            => (a, b) => a.Union(b);

        /// <inheritdoc/>
        public override Func<IEnumerable<TACInstruction>, IEnumerable<TACInstruction>, bool> Compare
            => (a, b) => !a.Except(b).Any() && !b.Except(a).Any();

        /// <inheritdoc/>
        public override IEnumerable<TACInstruction> Init { get => Enumerable.Empty<TACInstruction>(); protected set { } }

        /// <inheritdoc/>
        public override Func<BasicBlock, IEnumerable<TACInstruction>, IEnumerable<TACInstruction>> TransferFunction { get; protected set; }

        public override InOutData<IEnumerable<TACInstruction>> Execute(ControlFlowGraph graph)
        {
            TransferFunction = new ReachingTransferFunc(graph).Transfer;
            return base.Execute(graph);
        }

        public override void Run()
        {
            this.Execute(Cfg);
        }
    }
}
