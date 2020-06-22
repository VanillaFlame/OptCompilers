using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public class ConstantFoldingOptimizer : TACOptimizer
    {
        public ConstantFoldingOptimizer(ThreeAddressCode tac) : base(tac)
        {
        }

        public override void Run()
        {
            foreach (var c in Instructions)
            {
                if (c.Operation.Equals("+") ||
                    c.Operation.Equals("-") ||
                    c.Operation.Equals("*") ||
                    c.Operation.Equals("/"))
                {
                    var arg1 = 0.0;
                    var arg2 = 0.0;
                    var arg1IsDigit = double.TryParse(c.Argument1, out arg1);
                    var arg2IsDigit = double.TryParse(c.Argument2, out arg2);
                    if (arg1IsDigit && arg2IsDigit)
                    {
                        double res = 0.0;
                        switch (c.Operation)
                        {
                            case "+":
                                res = arg1 + arg2;
                                break;
                            case "-":
                                res = arg1 - arg2;
                                break;
                            case "*":
                                res = arg1 * arg2;
                                break;
                            case "/":
                                res = arg1 / arg2;
                                break;
                        }
                        c.Argument1 = res.ToString();
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                }
                else if (c.Operation.Equals("==") ||
                    c.Operation.Equals("!=") ||
                    c.Operation.Equals("<") ||
                    c.Operation.Equals(">"))
                {
                    var arg1 = 0.0;
                    var arg2 = 0.0;
                    var arg1IsDigit = double.TryParse(c.Argument1, out arg1);
                    var arg2IsDigit = double.TryParse(c.Argument2, out arg2);
                    if (arg1IsDigit && arg2IsDigit)
                    {
                        bool res = false;
                        switch (c.Operation)
                        {
                            case "==":
                                res = arg1 == arg2;
                                break;
                            case "!=":
                                res = arg1 != arg2;
                                break;
                            case "<":
                                res = arg1 < arg2;
                                break;
                            case ">":
                                res = arg1 > arg2;
                                break;
                        }
                        c.Argument1 = res.ToString();
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                }
            }
        }
    }
}
