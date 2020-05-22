using System.Collections.Generic;
using System.Linq;
using SimpleLang.TAC;
namespace SimpleLang.TACOptimizers
// не учитывает области видимости
{
	/// <summary>
    /// Copies and constants forwarding
    /// </summary>
    public class CopyAndConstantsOptimizer : TACOptimizer
    {
        HashSet<string> knownVariables = new HashSet<string>();
        Dictionary<string, string> varsValues = new Dictionary<string, string>();
        HashSet<string> knownConstants = new HashSet<string>();

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
                throw new System.Exception("Пустой TAC");
            OptimizeCopy();
            OptimizeConstants();                      
        }

		/// <summary>
        /// Copies forwarding
        /// </summary>
        public void OptimizeCopy()
        {
            foreach(var current in Instructions)
            {          
                if (current.Operation == "=")
                {
                    if (knownVariables.Contains(current.Argument1))
                    {
                        current.Argument1 = varsValues[current.Argument1];
                    }

                    if (knownVariables.Contains(current.Argument2))
                    {
                        current.Argument2 = varsValues[current.Argument2];
                    }

                    var arg1 = current.Argument1;
                    // если это буква
                    if (current.Argument2 == "" && 
                        (!double.TryParse(arg1, out _)) && (arg1 != "False" && arg1 != "True"))
                    {
                        knownVariables.Add(current.Result);
                        varsValues[current.Result] = current.Argument1;
                    }
                }
            }           
        }

		/// <summary>
        /// Constants forwarding
        /// </summary>
        public void OptimizeConstants()
        {
            foreach (var current in Instructions)
            {
                if (current.Operation.Count() > 1 && current.Operation[0] != '='
                    && current.Operation[1] == '=')
                {
                    if (knownConstants.Contains(current.Argument1))
                    {
                        current.Argument1 = varsValues[current.Argument1];
                    }

                    if (knownConstants.Contains(current.Argument2))
                    {
                        current.Argument2 = varsValues[current.Argument2];
                    }

                    
                    if (double.TryParse(current.Argument1, out double c1) && 
                        double.TryParse(current.Argument2, out double c2))
                    {
                        var succ = true;
                        switch(current.Operation)
                        {
                            case "+": current.Argument1 = (c1 + c2).ToString(); break;
                            case "-": current.Argument1 = (c1 - c2).ToString(); break;
                            case "*": current.Argument1 = (c1 * c2).ToString(); break;
                            case "/": current.Argument1 = (c1 / c2).ToString(); break;
                            default: succ = false; break;
                        }

                        if (succ)
                        {
                            current.Argument2 = "";
                            current.Operation = "";
                        }
                    }
                    /*
                    {
                        var ctmp = new ConstantFoldingOptimizer(TAC);
                        ctmp.Run();
                    }
                    */
                    //TODO: таки использовать как-то этот оптимайзер? 

                    if (current.Argument2 == "" && double.TryParse(current.Argument1, out double c))
                    {
                        knownConstants.Add(current.Result);
                        varsValues[current.Result] = current.Argument1;
                    }
                }
            }
        }
    }
}
