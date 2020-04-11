using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.Visitors.ChangeVisitors;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            string FileName = @"..\..\b.txt";
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

                /*
                var assCounter = new AssignCountVisitor();
                parser.root.Visit(assCounter);
                Console.WriteLine("AssignCount = " + assCounter.AssignCount);
                Console.WriteLine();
                */

                var prettyPrinter = new PrettyPrinterVisitor();
                /*parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

                var trueOpt = new TrueConditionOptVisitor();
                parser.root.Visit(trueOpt);

                Console.WriteLine("========================================================================");
                prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

                var trueIfOpt = new TrueIfOptVisitor();
                parser.root.Visit(trueIfOpt);

                Console.WriteLine("========================================================================");*/
                prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

                var falseOpt = new FindFalseVisitor();
                parser.root.Visit(falseOpt);

                Console.WriteLine("========================================================================");
                prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

                var trueIfOpt = new IfFalseVisitor();
                parser.root.Visit(trueIfOpt);

                Console.WriteLine("========================================================================");
                prettyPrinter = new PrettyPrinterVisitor();
                parser.root.Visit(prettyPrinter);
                Console.WriteLine(prettyPrinter.FormattedProgram);

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
