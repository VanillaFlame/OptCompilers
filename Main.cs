using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.TAC;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleLang.TACOptimizers;
using SimpleLang.CFG;


namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string FileName = @"..\..\TestSuiteTxt\a.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Error");
                else
                {
                    Console.WriteLine("Syntax tree has been built");
                    //foreach (var st in parser.root.StList)
                    //Console.WriteLine(st);
                }

                var parentFiller = new FillParentsVisitor();
                parser.root.Visit(parentFiller);

                AllVisitorsOptimization.Optimization(parser);
                
                var prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);


                var TACGenerator = new TACGenerationVisitor();
                parser.root.Visit(TACGenerator);

                //var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
                //TACBlocks.GenBaseBlocks();

                //Console.WriteLine(TACBlocks);
                //Console.WriteLine("================================================================================");

                var BaseBlock = AllTacOptimization.Optimize(parser);
                BaseBlock.GenBaseBlocks();
                Console.WriteLine(BaseBlock);
                Console.WriteLine("================================================================================");

                /*
                // Tree optimizations
                var prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

                var trueOpt = new TrueConditionOptVisitor();
                parser.root.Visit(trueOpt);

                Console.WriteLine("========================================================================");
                prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

                var trueIfOpt = new TrueIfOptVisitor();
                parser.root.Visit(trueIfOpt);

                Console.WriteLine("========================================================================");
                prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);
                */

                /*
                var TACGenerator = new TACGenerationVisitor();
                parser.root.Visit(TACGenerator);
                Console.WriteLine("Three Address Code:");
                Console.WriteLine(TACGenerator.TAC);
                Console.WriteLine("================================================================================");
                */

                /*
                // TAC optimizations
                var AIOptimizer = new AlgebraicIdentitiesOptimizer(TACGenerator.TAC);
                AIOptimizer.Run();
                Console.WriteLine("Algebraic Identities Optimizer:");
                Console.WriteLine(AIOptimizer.TAC);

                Console.WriteLine("================================================================================");

                var GTOptimizer = new GotoOptimizer(TACGenerator.TAC);
                GTOptimizer.Run();
                Console.WriteLine("GOTO Optimizer:");
                Console.WriteLine(GTOptimizer.TAC);
                
                Console.WriteLine("================================================================================");
                
                var GoTotoGoTOptimizer = new GoTotoGoTo_EmptyOptimizer(TACGenerator.TAC);
                GoTotoGoTOptimizer.Run();
                Console.WriteLine("Before GoTo to GoTo & Empty Remove Optimizer:");
                Console.WriteLine(GoTotoGoTOptimizer.TAC);
                
                Console.WriteLine("================================================================================");
                */

                /*
                var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
                TACBlocks.GenBaseBlocks();

                Console.WriteLine(TACBlocks);
                Console.WriteLine("================================================================================");
                
                var TACGenerator = new TACGenerationVisitor();
                parser.root.Visit(TACGenerator);
                var TACBlocks = new TACBaseBlocks(TACGenerator.Instructions);
                TACBlocks.GenBaseBlocks();
                Console.WriteLine(TACBlocks.ToString());
                Console.WriteLine("================================================================================");
                */

                /*var DefUseOptimizer = new DefUseOptimizer(TACGenerator.TAC);
                DefUseOptimizer.Run();
                Console.WriteLine("Def use Optimizer:");
                Console.WriteLine(DefUseOptimizer.TAC);*/

                /*
                foreach (var block in TACBlocks.blocks)
                {
                    var DefUseOptimizer = new DefUseOptimizer(new ThreeAddressCode(block.Instructions));
                    DefUseOptimizer.Run();
                    Console.WriteLine("Def use Optimizer:");
                    Console.WriteLine(DefUseOptimizer.TAC);
                }
                */
                /*
                var cfg = new ControlFlowGraph(TACBlocks.blocks);
                var availableExprOptimizer = new AvailableExpressionsOptimizer();
                availableExprOptimizer.Run(cfg, TACBlocks.blocks);
                Console.WriteLine(TACBlocks.ToString());
                Console.WriteLine("================================================================================");
                */
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }

            Console.ReadLine();
        }

    }
}
