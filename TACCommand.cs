using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public class TACCommand
    {
        public string Operation { get; set; }
        public string Argument1 { get; set; }
        public string Argument2 { get; set; }
        public string Result { get; set; }
        public string Label { get; set; }
        public bool HasLabel { get { return Label != ""; } }

        public TACCommand(string op, string arg1, string arg2, string res, string label = "")
        {
            Operation = op;
            Argument1 = arg1;
            Argument2 = arg2;
            Result = res;
            Label = label;
        }
    }
}
