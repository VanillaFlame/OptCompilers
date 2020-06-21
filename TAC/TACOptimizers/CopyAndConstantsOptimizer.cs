using System.Collections.Generic;
using System.Linq;
using SimpleLang.TAC;
namespace SimpleLang.TACOptimizers
{
    /// <summary>
    /// Copies and constants forwarding
    /// </summary>
    public class CopyAndConstantsOptimizer : TACOptimizer
    {
        /// <summary>
        /// Initialize and optimize
        /// </summary>
        /// <param name="t"> TAC </param>
        public CopyAndConstantsOptimizer(ThreeAddressCode t) : base(t)
        {
        }

        /// <summary>
        /// Do both optimizations
        /// </summary>
        override public void Run()
        {
            if (TAC == null)
                throw new System.Exception("Отсутствует TAC");
            OptimizeConstants();
            OptimizeCopy();
        }

        /// <summary>
        /// Copies forwarding
        /// </summary>
        public void OptimizeCopy()
        {
            var knownVariables = new HashSet<string>();
            var values = new Dictionary<string, string>();
            foreach (var current in Instructions)
            {
                if (!(current.Result.Equals("") || current.Result.Contains("#")))
                {
                    if (knownVariables.Contains(current.Argument1))
                    {
                        current.Argument1 = values[current.Argument1];
                    }

                    if (knownVariables.Contains(current.Argument2))
                    {
                        current.Argument2 = values[current.Argument2];
                    }

                    var arg1 = current.Argument1;
                    // если это буква
                    if (current.Argument2 == "" &&
                        (!double.TryParse(arg1, out _)) && (arg1 != "False" && arg1 != "True"))
                    {
                        knownVariables.Add(current.Result);
                        values[current.Result] = current.Argument1;
                    }
                }
                else
                {
                    if (current.Operation is "if goto" || current.Operation is "goto")
                    {
                        knownVariables.Clear();
                        values.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Constants forwarding
        /// </summary>
        public void OptimizeConstants()
        {
            var knownConstants = new HashSet<string>();
            var values = new Dictionary<string, string>();
            foreach (var current in Instructions)
            {
                if (!(current.Result.Equals("") || current.Result.Contains("#")))
                {
                    if (knownConstants.Contains(current.Argument1))
                    {
                        current.Argument1 = values[current.Argument1];
                    }

                    if (knownConstants.Contains(current.Argument2))
                    {
                        current.Argument2 = values[current.Argument2];
                    }

                    if (double.TryParse(current.Argument1, out double c1) &&
                        double.TryParse(current.Argument2, out double c2))
                    {
                        var success = true;
                        switch (current.Operation)
                        {
                            case "+": current.Argument1 = (c1 + c2).ToString(); break;
                            case "-": current.Argument1 = (c1 - c2).ToString(); break;
                            case "*": current.Argument1 = (c1 * c2).ToString(); break;
                            case "/": current.Argument1 = (c1 / c2).ToString(); break;
                            default: success = false; break;
                        }

                        if (success)
                        {
                            current.Argument2 = "";
                            current.Operation = "="; 
                        }
                    }

                    if (current.Argument2 == "" && double.TryParse(current.Argument1, out double c))
                    {
                        knownConstants.Add(current.Result);
                        values[current.Result] = current.Argument1;
                    }
                }
                else
                {
                    if (current.Operation is "if goto" || current.Operation is "goto")
                    {
                        knownConstants.Clear();
                        values.Clear();
                    }
                }
            }
        }
    }
}
