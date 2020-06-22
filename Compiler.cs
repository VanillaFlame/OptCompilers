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

namespace SimpleLang
{
    public class Compiler
    {
        public void Compile(string path)
        {
            try
            {
                string Text = File.ReadAllText(path);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Parsing error");
                else
                {
                    Console.WriteLine("Syntax tree has been built");
                }

                AllVisitorsOptimization.Optimization(parser);
                var blocks = AllTacOptimization.Optimize(parser);
                var instructions = blocks.BlockMerging();

                foreach (var instruction in instructions)
                {
                    Console.WriteLine(instruction);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File {0} doesn't exist", path);
            }
            catch (LexException e)
            {
                Console.WriteLine("Lex error. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Syntax error. " + e.Message);
            }
        }
    }
}
