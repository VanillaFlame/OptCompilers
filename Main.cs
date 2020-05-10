using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleLang.TACOptimizers;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            string FileName = @"..\..\CE.txt";
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


                var TACGenerator = new TACGenerationVisitor();
                /*parser.root.Visit(TACGenerator);
                foreach (var c in TACGenerator.Instructions)
                {
                    Console.WriteLine(c.Label + ": \t" + c.Operation + '\t' + c.Argument1 + '\t' + c.Argument2 + '\t' + c.Result);
                }

                Console.WriteLine("================================================================================");

                var AIOptimizer = new AlgebraicIdentitiesOptimizer(TACGenerator.Instructions);
                AIOptimizer.Run();

                foreach (var c in AIOptimizer.Instructions)
                {
                    Console.WriteLine(c.Label + ": \t" + c.Operation + '\t' + c.Argument1 + '\t' + c.Argument2 + '\t' + c.Result);
                }*/
                Console.WriteLine("================================================================================");
                TACGenerator = new TACGenerationVisitor();
                parser.root.Visit(TACGenerator);
                foreach (var c in TACGenerator.Instructions)
                {
                    Console.WriteLine(c.Label + ": \t" + c.Operation + '\t' + c.Argument1 + '\t' + c.Argument2 + '\t' + c.Result);
                }
                Console.WriteLine("================================================================================");
                var CEOptimizer = new CommonExpressionsOptimizer(TACGenerator.Instructions);
                CEOptimizer.Run();

                foreach (var c in CEOptimizer.Instructions)
                {
                    Console.WriteLine(c.Label + ": \t" + c.Operation + '\t' + c.Argument1 + '\t' + c.Argument2 + '\t' + c.Result);
                }
                /*Console.WriteLine("================================================================================");
                TACGenerator = new TACGenerationVisitor();
                parser.root.Visit(TACGenerator);
                foreach (var c in TACGenerator.Instructions)
                {
                    Console.WriteLine(c.Label + ": \t" + c.Operation + '\t' + c.Argument1 + '\t' + c.Argument2 + '\t' + c.Result);
                }
                Console.WriteLine("================================================================================");
                var CFoptimizer = new ConstantFoldingOptimizer(TACGenerator.Instructions);
                CFoptimizer.Run();

                foreach (var c in CEOptimizer.Instructions)
                {
                    Console.WriteLine(c.Label + ": \t" + c.Operation + '\t' + c.Argument1 + '\t' + c.Argument2 + '\t' + c.Result);
                }*/
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
