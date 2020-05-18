using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public class Basic_Block
    {
        private List<TACInstruction> instructions = new List<TACInstruction>();
        private List<Basic_Block> _Out = new List<Basic_Block>();
        private List<Basic_Block> _In = new List<Basic_Block>();
        public List<TACInstruction> Instructions
        {
            get
            {
                return instructions;
            }

        }
        public List<Basic_Block> In
        {
            get
            {
                return _In;
            }
            set
            {
                _In = value;
            }

        }

        public List<Basic_Block> Out
        {
            get
            {
                return _Out;
            }
            set
            {
                _Out = value;
            }

        }
        public Basic_Block()
        {
            instructions = new List<TACInstruction>();
            _Out = new List<Basic_Block>();
            _In = new List<Basic_Block>();
        }

        public Basic_Block(List<TACInstruction> instr)
        {
            instructions = instr;
            _Out = new List<Basic_Block>();
            _In = new List<Basic_Block>();
        }
    }
}
