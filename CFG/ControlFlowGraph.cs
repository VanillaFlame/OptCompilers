using System.Collections.Generic;
using System.Linq;
using System;

namespace SimpleLang.CFG
{
    public class ControlFlowGraph
    {
        /// <summary>
        /// Пустой блок начала
        /// </summary>
        public BasicBlock start = new BasicBlock();

        /// <summary>
        /// Пустой блок конца
        /// </summary>
        public BasicBlock end = new BasicBlock();

        public List<BasicBlock> blocks;
        public int BlockCount => blocks.Count;

        /// <summary>
        /// Создает Control Flow Graph
        /// </summary>
        /// <param name="blocks"> Упорядоченная коллекция базовых блоков</param>
        public ControlFlowGraph(List<BasicBlock> blocks)
        {
            this.blocks = blocks;
            if (blocks.Count == 0)
                return;
            CreateCFG();
        }

        void CreateCFG()
        {
            start.Out.Add(blocks[0]);
            blocks[0].In.Add(start);
            // каждый блок является началом последующего
            for (int i = 0; i < blocks.Count - 1; i++)
            {
                var curBlock = blocks[i];
                // блок содержит GoTo в последней строке?
                var lastLine = curBlock.Instructions.Last();
                var op = lastLine.Operation;
                if (op is "goto" || op is "if goto")
                {
                    // ищем на какой блок идет переход
                    var label = lastLine.Argument2 == "" ? lastLine.Argument1 : lastLine.Argument2;
                    BasicBlock targetBlock = null;
                    // Ищем метку в первой строчке
                    foreach (var block in blocks)
                        if (block.Instructions.First().Label == label)
                            targetBlock = block;

                    if (targetBlock == null)
                        throw new Exception("Goto ведет к несуществующей метке");

                    curBlock.Out.Add(targetBlock);
                    targetBlock.In.Add(curBlock);
                }

                if (!(op is "goto"))
                {
                    var nextBlock = blocks[i + 1];
                    curBlock.Out.Add(nextBlock);
                    nextBlock.In.Add(curBlock);
                }                
            }

            // если в конце последнего блока безусловный goto, то это бесконечный цикл
            if (!(blocks[blocks.Count - 1].Instructions.Last().Operation is "goto"))
            {
                end.In.Add(blocks[blocks.Count - 1]);
                blocks[blocks.Count - 1].Out.Add(end);
            }
        }
    }
}
