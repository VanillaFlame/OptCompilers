using System.Collections.Generic;
using System.Linq;
using SimpleLang.CFG;

namespace SimpleLang.TACOptimizers
{
	/// <summary>
	/// Используйте ReachingDefinitionOptimizer, а не этот
	/// </summary>
    public class ReachingDefinitionVectorOptimizer
    {
        public ControlFlowGraph graph;
        public Dictionary<BasicBlock, List<TACInstruction>> gen;
        public Dictionary<BasicBlock, List<TACInstruction>> kill;
						
		public Dictionary<BasicBlock, InOutVector> INvector;
        public Dictionary<BasicBlock, InOutVector> OUTvector;

        public ReachingDefinitionVectorOptimizer(List<BasicBlock> blocks)
        {
            graph = new ControlFlowGraph(blocks);
            gen = new Dictionary<BasicBlock, List<TACInstruction>>();
            kill = new Dictionary<BasicBlock, List<TACInstruction>>();
			
			INvector = new Dictionary<BasicBlock, InOutVector>();
            OUTvector = new Dictionary<BasicBlock, InOutVector>();
        }

        public ReachingDefinitionVectorOptimizer(ControlFlowGraph graph)
        {
            this.graph = graph;
            gen = new Dictionary<BasicBlock, List<TACInstruction>>();
            kill = new Dictionary<BasicBlock, List<TACInstruction>>();
           
			INvector = new Dictionary<BasicBlock, InOutVector>();
            OUTvector = new Dictionary<BasicBlock, InOutVector>();
        }

        private void GetGenKillSet()
        {
            for (int i = 0; i < graph.blocks.Count; ++i)
            {
                List<TACInstruction> currGen = new List<TACInstruction>();
                HashSet<string> setGen = new HashSet<string>();
                graph.blocks[i].Instructions.Reverse();
                foreach (var instr in graph.blocks[i].Instructions)
                {
                    if (!setGen.Contains(instr.Result)
                        && !(instr.Result.Equals("")
                           || instr.Result.Contains("#")))
                    {
                        setGen.Add(instr.Result);
                        currGen.Add(instr);
                    }
                }
                graph.blocks[i].Instructions.Reverse();
                gen.Add(graph.blocks[i], currGen);
                List<TACInstruction> currKill = new List<TACInstruction>();
                for (int j = 0; j < graph.blocks.Count; ++j)
                {
                    if (j == i)
                        continue;

                    foreach (var instr in graph.blocks[j].Instructions)
                    {
                        if (setGen.Contains(instr.Result))
                        {
                            currKill.Add(instr);
                        }
                    }
                }
                kill.Add(graph.blocks[i], currKill);
            }
        }

        public void Run()
        {
            GetGenKillSet();
			
			var assignList = new List<TACInstruction>();
            foreach (var instr in graph.blocks.SelectMany(block => block.Instructions))
                if (!(instr.Result.Equals("")
                           || instr.Result.Contains("#")))
                    assignList.Add(instr);

            INvector[graph.start] = new InOutVector(assignList.Count);
            OUTvector[graph.start] = new InOutVector(assignList.Count);

            foreach (var block in graph.blocks)
            {
                OUTvector[block] = new InOutVector(assignList.Count);
            }

            bool change = true;
            while (change)
            {
                for (int i = 0; i < graph.blocks.Count; ++i)
                {
                    change = false;
                    INvector[graph.blocks[i]] = graph.blocks[i].In.Select(n => OUTvector[n])
                        .Aggregate((x, y) => x + y);
                    var prevOut = OUTvector[graph.blocks[i]];

                    var genTransform = new InOutVector(assignList.Count);
                    foreach (var instr in gen[graph.blocks[i]])
                    {
                        genTransform[assignList.IndexOf(instr)] = true;
                    }

                    var killTransform = new InOutVector(assignList.Count);
                    foreach (var instr in kill[graph.blocks[i]])
                    {
                        killTransform[assignList.IndexOf(instr)] = true;
                    }

					OUTvector[graph.blocks[i]] = 
					    genTransform + (INvector[graph.blocks[i]] - killTransform);
                    if (prevOut != OUTvector[graph.blocks[i]])
                        change = true;
                }
            }
        }
    }
}