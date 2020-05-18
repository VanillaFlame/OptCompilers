using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System.Collections.Generic;
using System.Linq;
namespace SimpleLang.Optimizers

{
	/// <summary>
    /// Copies and constants forwarding
    /// </summary>
    public class CopyAndConstantsOptimizer
    {
        HashSet<string> knownVariables = new HashSet<string>();
        Dictionary<string, string> varsValues = new Dictionary<string, string>();
        HashSet<string> knownConstants = new HashSet<string>();

		/// <summary>
        /// ThreeAddressCode, source modified after optimization
        /// </summary>
        public ThreeAddressCode tac { get; set; } = null;

		/// <summary>
        /// Initialize
        /// </summary>
        public CopyAndConstantsOptimizer(){}

        /// <summary>
        /// Initialize and optimize
        /// </summary>
        /// <param name="t"> TAC </param>
        public CopyAndConstantsOptimizer(ThreeAddressCode t)
        {
            tac = t;
            Optimize();
        }

		/// <summary>
        /// Do both optimizations
        /// </summary>
        public void Optimize()
        {
            if (tac == null)
                throw new System.Exception();
            OptimizeCopy();
            OptimizeConstants();
            
            
        }

		/// <summary>
        /// Copies forwarding
        /// </summary>
        public void OptimizeCopy()
        {
            var current = tac.First;
            while (current != null)
            {
                if (current.Value is TacAssignmentNode tas)
                {
                    if (knownVariables.Contains(tas.FirstOperand))
                    {
                        tas.FirstOperand = varsValues[tas.FirstOperand];
                        current.Value = tas;
                    }

                    if (knownVariables.Contains(tas.SecondOperand))
                    {
                        tas.SecondOperand = varsValues[tas.SecondOperand];
                        current.Value = tas;
                    }

                    //по идее еще нужна проверка что это буква, ну пусть костылек
                    if (tas.RightPart == tas.FirstOperand && tas.FirstOperand.Split(' ').Count() == 1)
                    {
                        knownVariables.Add(tas.LeftPartIdentifier);
                        varsValues[tas.LeftPartIdentifier] = tas.RightPart;
                    }
                }
                current = current.Next;
            }           
        }

		/// <summary>
        /// Constants forwarding
        /// </summary>
        public void OptimizeConstants()
        {
            var current = tac.First;
            while (current != null)
            {
                if (current.Value is TacAssignmentNode tas)
                {
                    if (knownConstants.Contains(tas.FirstOperand))
                    {
                        tas.FirstOperand = varsValues[tas.FirstOperand];
                        current.Value = tas;
                    }

                    if (knownConstants.Contains(tas.SecondOperand))
                    {
                        tas.SecondOperand = varsValues[tas.SecondOperand];
                        current.Value = tas;
                    }

                    // сплюсуем константы
                    if (double.TryParse(tas.FirstOperand, out double c1) && 
                        double.TryParse(tas.SecondOperand, out double c2))
                    {
                        tas.SecondOperand = null;
                        switch(tas.Operation)
                        {
                            //Or, And, Plus, Minus, Prod, Div, Less, LessOrEqual, Greater, GreaterOrEqual, Equal, NotEqual
                            case "Plus": tas.FirstOperand = (c1 + c2).ToString(); break;
                            case "Minus": tas.FirstOperand = (c1 - c2).ToString(); break;
                            case "Prod": tas.FirstOperand = (c1 * c2).ToString(); break;
                            case "Div": tas.FirstOperand = (c1 / c2).ToString(); break;
                        }
                        tas.Operation = null;
                    }

                    //SecondOperand либо равен нулю, либо не константа
                    if (double.TryParse(tas.FirstOperand, out double c))
                    {
                        knownConstants.Add(tas.LeftPartIdentifier);
                        varsValues[tas.LeftPartIdentifier] = tas.FirstOperand;
                    }
                }
                current = current.Next;
            }
        }
    }
}
