using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public class BasicBlock
    {
        private List<TACInstruction> instructions = new List<TACInstruction>();
        private List<BasicBlock> _Out = new List<BasicBlock>();
        private List<BasicBlock> _In = new List<BasicBlock>();
        public List<TACInstruction> Instructions
        {
            get
            {
                return instructions;
            }

        }
        public List<BasicBlock> In
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

        public List<BasicBlock> Out
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
        public BasicBlock()
        {
            instructions = new List<TACInstruction>();
            _Out = new List<BasicBlock>();
            _In = new List<BasicBlock>();
        }

        public BasicBlock(List<TACInstruction> instr)
        {
            instructions = instr;
            _Out = new List<BasicBlock>();
            _In = new List<BasicBlock>();
        }
    }
}
