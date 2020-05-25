using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public static class StringExtensions
    {
        private static HashSet<string> ArithmeticOperations = new HashSet<string>() { "+", "-", "*", "/" };

        public static bool IsArithmetic(this string str)
        {
            return ArithmeticOperations.Contains(str);
        }

        public static bool IsVariable(this string str)
        {
            return !(str.Equals("")) && (char.IsLetter(str[0]) || str[0] is '#');
        }
    }

    public class TACInstruction
    {
        public string Operation { get; set; }
        public string Argument1 { get; set; }
        public string Argument2 { get; set; }
        public string Result { get; set; }
        public string Label { get; set; }
        public bool HasLabel { get { return Label != ""; } }

        public TACInstruction(string op, string arg1, string arg2, string res, string label = "")
        {
            Operation = op;
            Argument1 = arg1;
            Argument2 = arg2;
            Result = res;
            Label = label;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Label + '\t');

            if (!Result.Equals(""))
            {
                if (!Operation.Equals("="))
                {
                    // AddInstruction(LessOrEqual, counter, end, cond, label);
                    // AddInstruction(Plus, a, b, temp, label);
                    stringBuilder.Append(Result + " = " + Argument1 + ' ' + Operation + ' ' + Argument2);
                }
                else
                {
                    // AddInstruction(Assign, a, "", temp);
                    stringBuilder.Append(Result + ' ' + Operation + ' ' + Argument1);
                }

                return stringBuilder.ToString();
            }

            switch (Operation)
            {
                case "if goto":
                    stringBuilder.Append("if " + Argument1 + " goto " + Argument2);
                    break;

                case "goto":
                    stringBuilder.Append("goto " + Argument1);
                    break;

                case "":
                    break;

                default:
                    stringBuilder.Append("Unexpected operation");
                    break;
            }

            return stringBuilder.ToString();
        }
    }
}
