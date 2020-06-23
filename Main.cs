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
using SimpleLang;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            string fileName = @"..\..\TestSuiteTxt\a.txt";
            var compiler = new Compiler();
            compiler.Compile(fileName);

            Console.ReadLine();
        }
    }
}
