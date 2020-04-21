using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public class AlgebraicIdentitiesOptimizer
    {
        public List<TACCommand> TACCommands { get; set; }
        public AlgebraicIdentitiesOptimizer(List<TACCommand> commands)
        {
            TACCommands = commands;
        }
        public void Execute()
        {
            foreach (var c in TACCommands)
            {
                if (c.Operation.Equals("+"))
                {
                    if (c.Argument1.Equals("0"))
                    {
                        c.Argument1 = c.Argument2;
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                    else if (c.Argument2.Equals("0"))
                    {
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                }   

                if (c.Operation.Equals("-"))
                {
                    if (c.Argument2.Equals("0"))
                    {
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                    else if (c.Argument1.Equals(c.Argument2))
                    {
                        c.Argument1 = "0";
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                }

                if (c.Operation.Equals("*"))
                {
                    if (c.Argument1.Equals("1"))
                    {
                        c.Argument1 = c.Argument2;
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                    else if (c.Argument2.Equals("1"))
                    {
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                    else if (c.Argument1.Equals("0") || c.Argument2.Equals("0"))
                    {
                        c.Argument1 = "0";
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                }

                if (c.Operation.Equals("/"))
                {
                    if (c.Argument2.Equals("1"))
                    {
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                    else if (c.Argument1.Equals(c.Argument2))
                    {
                        c.Argument1 = "1";
                        c.Argument2 = "";
                        c.Operation = "=";
                    }
                }
            }
        }
    }
}
