using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleLang.CFG;

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
            new DeadAliveOptimize(temp) // ?

            };

        private static List<TACOptimizer> TACOptimizersAllBlock = new List<TACOptimizer>() {
            new GotoOptimizer(temp)
            };

        private static List<TACOptimizer> IterAlgoOptimizers = new List<TACOptimizer>() {
            new AvailableExpressionsOptimizer(temp)
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
                    TACOptimizersOnBlock[CountOptimization].Instructions = previos;
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
                TACOptimizersAllBlock[AllOptimizationCount].Instructions = previos;
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
            var prevBlocks = blocks.blocks;
            int AllOptimizationCount = 0;
            do
            {
                IterAlgoOptimizers[AllOptimizationCount].Instructions = prevInstructions;
                IterAlgoOptimizers[AllOptimizationCount].Blocks = prevBlocks;
                var cfg = new ControlFlowGraph(prevBlocks);
                IterAlgoOptimizers[AllOptimizationCount].Cfg = cfg;
                IterAlgoOptimizers[AllOptimizationCount].Run();
                
                if (prevInstructions.SequenceEqual(TACOptimizersAllBlock[AllOptimizationCount].Instructions))
                    AllOptimizationCount++;
                else
                {
                    prevInstructions = TACOptimizersAllBlock[AllOptimizationCount].Instructions;
                    prevBlocks = IterAlgoOptimizers[AllOptimizationCount].Blocks;
                    AllOptimizationCount = 0;
                }
            } while (AllOptimizationCount < TACOptimizersAllBlock.Count);
            return new TACBaseBlocks(prevBlocks);
        }

        public static TACBaseBlocks Optimize(Parser parser)
        {
            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
            TACBlocks.GenBaseBlocks();
            var oneBlockOptimizations = OptimizeBlock(TACBlocks);
            var allBlocksOptimizations = AllOptimization(oneBlockOptimizations); ;
            var iterAlogOptimizations = IterAlgoOptimizations(allBlocksOptimizations);
            return iterAlogOptimizations;
        }
    }
}