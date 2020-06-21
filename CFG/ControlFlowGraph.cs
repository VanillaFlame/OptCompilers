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
        public List<List<(int vertex, BasicBlock block)>> _children;
        public List<List<(int vertex, BasicBlock block)>> _parents;
        public List<BasicBlock> blocks;
        public int BlockCount => blocks.Count;

        /// <summary>
        /// Создает Control Flow Graph
        /// </summary>
        /// <param name="blocks"> Упорядоченная коллекция базовых блоков</param>
        public ControlFlowGraph(List<BasicBlock> blocks)
        {
            this.blocks = blocks;
            _children = new List<List<(int, BasicBlock)>>(blocks.Count);
            _parents = new List<List<(int, BasicBlock)>>(blocks.Count);
            if (blocks.Count == 0)
                return;
            CreateCFG();
        }

        void CreateCFG()
        {
            start.Out.Add(blocks[0]);
            blocks[0].In.Add(start);
            for (var i = 0; i < blocks.Count; ++i)
            {
                _children.Add(new List<(int, BasicBlock)>());
                _parents.Add(new List<(int, BasicBlock)>());
            }
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

                    var gotoOutBlock = blocks.FindIndex(block =>
                                string.Equals(block.Instructions.First().Label, label));
                    if (targetBlock == null)
                        throw new Exception("Goto ведет к несуществующей метке");
                    _children[i].Add((gotoOutBlock, blocks[gotoOutBlock]));
                    _parents[gotoOutBlock].Add((i, blocks[i]));
                    curBlock.Out.Add(targetBlock);
                    targetBlock.In.Add(curBlock);
                }

                if (!(op is "goto"))
                {
                    var nextBlock = blocks[i + 1];
                    curBlock.Out.Add(nextBlock);
                    nextBlock.In.Add(curBlock);
                    _children[i].Add((i + 1, blocks[i + 1]));
                    _parents[i + 1].Add((i, blocks[i]));
                }                
            }

                end.In.Add(blocks[blocks.Count - 1]);
                blocks[blocks.Count - 1].Out.Add(end);
        }
    }
}
