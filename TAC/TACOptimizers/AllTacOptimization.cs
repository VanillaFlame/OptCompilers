using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleLang.CFG;
using SimpleLang.TAC.TACOptimizers;

namespace SimpleLang.TACOptimizers
{
    public static class AllTacOptimization
    {

        private static List<TACInstruction> AllInstruction = new TACGenerationVisitor().Instructions;

        private static ThreeAddressCode temp = new ThreeAddressCode(AllInstruction);

        private static List<TACOptimizer> TACOptimizersOnBlock = new List<TACOptimizer>() {
            new DefUseOptimizer(temp),
            new AlgebraicIdentitiesOptimizer(temp),
            new ConstantFoldingOptimizer(temp),
            new CopyAndConstantsOptimizer(temp),
            new CommonExpressionsOptimizer(temp),
            new RemoveEmptyInstructionsOptimizer(temp),
            new DeadAliveOptimize(temp)

            };

        private static List<TACOptimizer> TACOptimizersAllBlock = new List<TACOptimizer>() {
            new GotoOptimizer(temp)
            };

        private static List<TACOptimizer> IterAlgoOptimizers = new List<TACOptimizer>() {
            new AvailableExpressionsOptimizer(temp),
            new ConstantPropagationIter()
        };

        private static List<TACInstruction> OptimizeBlock(TACBaseBlocks BaseBlocks)
        {
            var basicBlocks = BaseBlocks.blocks;
            List<TACInstruction> previos;
            List<TACInstruction> newTacInstruction = new List<TACInstruction>();
            int CountOptimization;
            int blockCount = 0;
            foreach (var block in basicBlocks)
            {
                previos = block.Instructions;
                CountOptimization = 0;
                do
                {
                    TACOptimizersOnBlock[CountOptimization].Instructions = previos.Copy();
                    TACOptimizersOnBlock[CountOptimization].Run();
                    if (previos.SequenceEqual(TACOptimizersOnBlock[CountOptimization].Instructions))
                    {
                        //previos = TACOptimizersOnBlock[CountOptimization].Instructions;
                        CountOptimization++;
                    }
                    else
                    {
                        previos = TACOptimizersOnBlock[CountOptimization].Instructions;
                        CountOptimization = 0;
                    }
                } while (CountOptimization < TACOptimizersOnBlock.Count);
                newTacInstruction.AddRange(previos);
                blockCount++;
            }
            return newTacInstruction;
        }


        private static TACBaseBlocks AllOptimization(List<TACInstruction> Instructions)
        {

            List<TACInstruction> previos;
            previos = Instructions;
            int AllOptimizationCount = 0;
            do
            {
                TACOptimizersAllBlock[AllOptimizationCount].Instructions = previos.Copy();
                TACOptimizersAllBlock[AllOptimizationCount].Run();
                if (previos.SequenceEqual(TACOptimizersAllBlock[AllOptimizationCount].Instructions))
                    AllOptimizationCount++;
                else
                {
                    previos = TACOptimizersAllBlock[AllOptimizationCount].Instructions;
                    AllOptimizationCount = 0;
                }
            } while (AllOptimizationCount < TACOptimizersAllBlock.Count);
            var res = new TACBaseBlocks(previos);
            res.GenBaseBlocks();
            return res;
        }

        private static TACBaseBlocks IterAlgoOptimizations(TACBaseBlocks blocks)
        {
            var prevInstructions = blocks.BlockMerging();
            int AllOptimizationCount = 0;
            TACBaseBlocks prevBlocks;
            do
            {
                IterAlgoOptimizers[AllOptimizationCount].Instructions = prevInstructions.Copy();
                prevBlocks = new TACBaseBlocks(IterAlgoOptimizers[AllOptimizationCount].Instructions);
                prevBlocks.GenBaseBlocks();
                IterAlgoOptimizers[AllOptimizationCount].Blocks = prevBlocks.blocks;
                var cfg = new ControlFlowGraph(prevBlocks.blocks);
                IterAlgoOptimizers[AllOptimizationCount].Cfg = cfg;
                IterAlgoOptimizers[AllOptimizationCount].Run();
                
                if (prevInstructions.SequenceEqual(IterAlgoOptimizers[AllOptimizationCount].Instructions))
                    AllOptimizationCount++;
                else
                {
                    prevInstructions = IterAlgoOptimizers[AllOptimizationCount].Instructions;
                    AllOptimizationCount = 0;
                }
            } while (AllOptimizationCount < IterAlgoOptimizers.Count);
            return prevBlocks;
        }

        public static TACBaseBlocks Optimize(Parser parser)
        {
            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
            TACBlocks.GenBaseBlocks();
            //loop
            var oneBlockOptimizations = OptimizeBlock(TACBlocks);
            var allBlocksOptimizations = AllOptimization(oneBlockOptimizations); ;
            var iterAlogOptimizations = IterAlgoOptimizations(allBlocksOptimizations);
            return iterAlogOptimizations;
        }
    }
}