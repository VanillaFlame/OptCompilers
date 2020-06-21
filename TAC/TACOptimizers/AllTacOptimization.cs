using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Visitors;
using SimpleParser;

namespace SimpleLang.TACOptimizers
{
    static class AllTacOptimization
    {

        private static List<TACInstruction> AllInstruction = new TACGenerationVisitor().Instructions;

        private static ThreeAddressCode temp = new ThreeAddressCode(AllInstruction);

        private static List<TACOptimizer> TACOptimizersOnBlock = new List<TACOptimizer>() {
            new DefUseOptimizer(temp),
            new AlgebraicIdentitiesOptimizer(temp),
            new ConstantFoldingOptimizer(temp),
            //new DeadAliveOptimize(temp),
            };

        private static List<TACOptimizer> TACOptimizersAllBlock = new List<TACOptimizer>() {
            new GotoOptimizer(temp)
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
            return new TACBaseBlocks(previos);
        }

        public static TACBaseBlocks Optimize(Parser parser)
        {
            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
            TACBlocks.GenBaseBlocks();
            var preResalut = OptimizeBlock(TACBlocks);
            return AllOptimization(preResalut);
        }
    }
}