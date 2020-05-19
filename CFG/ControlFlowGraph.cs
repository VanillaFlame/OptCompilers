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
        public Basic_Block start = new Basic_Block();

        /// <summary>
        /// Пустой блок конца
        /// </summary>
        public Basic_Block end = new Basic_Block();

        List<Basic_Block> blocks;

        /// <summary>
        /// Создает Control Flow Graph
        /// </summary>
        /// <param name="blocks"> Упорядоченная коллекция базовых блоков</param>
        public ControlFlowGraph(List<Basic_Block> blocks)
        {
            if (blocks.Count == 0)
                return;
            this.blocks = blocks;
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
                    Basic_Block targetBlock = null;
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
            }
        }
    }
}