using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    class CommonExpressionsOptimizer : TACOptimizer
    {
        public CommonExpressionsOptimizer(List<TACInstruction> instructions) : base(instructions)
        {
        }
        private bool Check_Varieble(string varieble, int start, int border)
        {
            bool res=true;
            for (int i = start; i < border; ++i)
            {
                res &= Instructions[i].Result != varieble;
            }
            return res;
        }
        public override void Run()
        {
            if (Instructions.Count == 0)
                return;
            for(int i=1; Instructions.Count>i;++i )
            {
                if((Instructions[i].Argument1.Length>0) &&(Instructions[i].Argument2.Length > 0))
                {
                    var op = Instructions[i].Operation;
                    var arg1 = Instructions[i].Argument1;
                    var arg2 = Instructions[i].Argument2;
                    for (int j = i-1; 0 <= j; --j)
                    {
                       if((Instructions[j].Operation == op)&&(Instructions[j].Argument1 == arg1)
                            && (Instructions[j].Argument2 == arg2) &&
                            Check_Varieble(Instructions[j].Result,j+1, i)
                            &&
                            Check_Varieble(Instructions[j].Argument1,j+1, i )
                            &&
                            Check_Varieble(Instructions[j].Argument2,j+1, i))
                        {
                            Instructions[i].Operation = "=";
                            Instructions[i].Argument1 = Instructions[j].Result;
                            Instructions[i].Argument2 = "";
                            break;
                        }
                    }
                }
            }
        }
    }
}
