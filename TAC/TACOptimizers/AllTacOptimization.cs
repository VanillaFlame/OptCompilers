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
        private static int GlobalOptimizationCount = 0;

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
                        GlobalOptimizationCount = 0;
                    }
                } while (CountOptimization < TACOptimizersOnBlock.Count);
                newTacInstruction.AddRange(previos);
                blockCount++;
            }
            ++GlobalOptimizationCount;
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
                    GlobalOptimizationCount = 0;
                }
            } while (AllOptimizationCount < TACOptimizersAllBlock.Count);
            ++GlobalOptimizationCount;
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
                BasicBlock.clearIndexCounter();
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
                    GlobalOptimizationCount = 0;
                }
            } while (AllOptimizationCount < IterAlgoOptimizers.Count);
            ++GlobalOptimizationCount;
            return prevBlocks;
        }

        public static TACBaseBlocks Optimize(Parser parser, bool debugInfo = false)
        {
            var TACGenerator = new TACGenerationVisitor();
            parser.root.Visit(TACGenerator);
            var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
            TACBlocks.GenBaseBlocks();
            TACBaseBlocks result = TACBlocks;
            var i = 1;
            do
            {
                var oneBlockOptimizations = OptimizeBlock(result);
                if (debugInfo)
                {
                    Console.WriteLine("===============TAC EachBlockOpt: Stage {0}===============", i.ToString());
                    PrintInstructions(oneBlockOptimizations);
                    ++i;
                }
                result = AllOptimization(oneBlockOptimizations);
                if (debugInfo)
                {
                    Console.WriteLine("===============TAC AllBlocksOpt: Stage {0}===============", i.ToString());
                    PrintInstructions(result.BlockMerging());
                    ++i;
                }
                result = IterAlgoOptimizations(result);
                if (debugInfo)
                {
                    Console.WriteLine("=================TAC IterAlgs: Stage {0}=================", i.ToString());
                    PrintInstructions(result.BlockMerging());
                    ++i;
                }
            } while (GlobalOptimizationCount < 3);
            return result;
        }

        private static void PrintInstructions(List<TACInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                Console.WriteLine(instruction);
            }
        }
    }
}