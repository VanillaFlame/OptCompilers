using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public class BasicBlock
    {
        private static int index;

        private List<TACInstruction> instructions = new List<TACInstruction>();
        private List<BasicBlock> _Out = new List<BasicBlock>();
        private List<BasicBlock> _In = new List<BasicBlock>();

        public int Index { get; private set; }

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
            Index = index++;
        }

        public BasicBlock(List<TACInstruction> instr)
        {
            instructions = instr;
            _Out = new List<BasicBlock>();
            _In = new List<BasicBlock>();
            Index = index++;
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var second = obj as BasicBlock;
            if (second == null)
            {
                return false;
            }
            return second.Index == Index;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var i in Instructions)
            {
                builder.Append(i.ToString().Trim());
                builder.Append('\n');
            }
            return builder.ToString();
        }

        /// <summary>
        /// Обнуляет внутренный статический счетчик индексов
        /// </summary>
        public static void clearIndexCounter()
        {
            index = 0;
        }
    }
}
